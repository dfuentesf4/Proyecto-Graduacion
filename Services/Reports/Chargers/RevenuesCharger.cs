using HFPMapp.Services.HTTP;
using HFPMapp.Services.Reports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.Reports.Chargers
{
    public class RevenuesCharger
    {
        public RevenuesCharger(RevenuesDetailApiClient revenuesDetailApiClient)
        {
            _revenuesApiClient = revenuesDetailApiClient;
        }

        private RevenuesDetailApiClient _revenuesApiClient;

        public async Task<List<RevenuesPerYear>> LoadRevenuesPerYear()
        {
            var revenuesDetails = await _revenuesApiClient.GetRevenuesDetailsAsync();

            var revenuesPerYear = revenuesDetails
                .GroupBy(x => x.Year.Value)
                .Select(g => new RevenuesPerYear
                {
                    Year = g.Key,
                    TotalRevenues = g.Sum(x => x.Amount.Value)
                })
                .ToList();

            return revenuesPerYear;
        }
    }
}
