using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Models
{
    public class TransferSummary
    {
        public int Id { get; set; }

        public int? Year { get; set; }

        public decimal? TotalIncome { get; set; }

        public decimal? TotalExpenses { get; set; }

        public decimal? NetIncome { get; set; }

        public decimal? RetainedEarning { get; set; }

        public decimal? BankBox { get; set; }

        public bool? IsActive { get; set; }
    }
}
