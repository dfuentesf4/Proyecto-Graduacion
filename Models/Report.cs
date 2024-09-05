using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Models
{
    public class Report
    {
        public int Id { get; set; }

        public DateOnly? Date { get; set; }

        public int? ProjectId { get; set; }

        public string? Description { get; set; }

        public string? Results { get; set; }

        public string? Recommendations { get; set; }

        public bool? IsActive { get; set; }

        public virtual Project? Project { get; set; }
    }
}
