using HFPMapp.Services.HTTP;
using HFPMapp.Services.Reports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.Reports.Chargers
{
    public class ExpensesCharger
    {

        public ExpensesCharger(ExpensesDetailApiClient expensesDetailApiClient)
        {
            _expensesDetailApiClient = expensesDetailApiClient;
        }

        private ExpensesDetailApiClient _expensesDetailApiClient;

        public async Task<List<ExpensesPerYear>> LoadExpensesPerYear()
        {
            var expensesDetails = await _expensesDetailApiClient.GetExpensesDetailsAsync();

            var expensesPerYear = expensesDetails
                .GroupBy(x => x.Year.Value)
                .Select(g => new ExpensesPerYear
                {
                    Year = g.Key,
                    TotalExpenses = g.Sum(x => x.Amount.Value)
                })
                .ToList();

            return expensesPerYear;
        }
    }
}
