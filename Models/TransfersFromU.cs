using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Models
{
    public class TransfersFromU
    {
        public int Id { get; set; }

        public DateOnly? Date { get; set; }

        public string? Folder { get; set; }

        public decimal? Amount { get; set; }

        public decimal? DollarExchange { get; set; }

        public decimal? DepositedQs { get; set; }

        public bool? IsActive { get; set; }
    }
}
