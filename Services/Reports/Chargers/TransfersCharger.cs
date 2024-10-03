using HFPMapp.Services.HTTP;
using HFPMapp.Services.Reports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.Reports.Chargers
{
    public class TransfersCharger
    {
        public TransfersCharger(TransfersFromUApiClient transfersFromUApiClient)
        {
            _transfersFromUApiClient = transfersFromUApiClient;
        }

        private TransfersFromUApiClient _transfersFromUApiClient;

        public async Task<List<TransfersUsPerYear>> LoadTransfersUsPerYear()
        {
            var transfers = await _transfersFromUApiClient.GetTransfersFromUAsync();

            var transfersPerYear = transfers
                .Where(t => t.Date.HasValue && t.Amount.HasValue) // Filtrar los registros válidos
                .GroupBy(t => t.Date.Value.Year) // Agrupar por año
                .Select(g => new TransfersUsPerYear
                {
                    Year = g.Key, // Año
                    Amount = g.Sum(t => t.DepositedQs.Value) // Sumar el monto para ese año
                })
                .ToList();

            return transfersPerYear;
        }
    }
}
