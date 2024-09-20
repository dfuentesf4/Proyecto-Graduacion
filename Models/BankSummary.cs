using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Models
{
    public class BankSummary
    {
        public int Id { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public decimal? DollarExchange { get; set; }

        public decimal? Amount { get; set; }

        public int? BankId { get; set; }

        public bool? IsActive { get; set; }

        public virtual Bank? Bank { get; set; }
    }
}
