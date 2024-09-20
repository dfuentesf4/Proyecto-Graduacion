using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Models
{
    public class Bank
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool? IsActive { get; set; }

        public virtual ICollection<BankBook> BankBooks { get; set; } = new List<BankBook>();

        public virtual ICollection<BankSummary> BankSummaries { get; set; } = new List<BankSummary>();

        public virtual ICollection<FolderBank> FolderBanks { get; set; } = new List<FolderBank>();
    }
}
