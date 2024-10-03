using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.Reports.Models
{
    public class RevenuesVsExpenses
    {
        public int Year { get; set; }
        public decimal Expenses { get; set; }
        public decimal Revenues { get; set; }
    }
}
