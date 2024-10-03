using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.Reports.Models
{
    public class ExpensesPerYear
    {
        public int Year { get; set; }
        public decimal TotalExpenses { get; set; }
    }
}
