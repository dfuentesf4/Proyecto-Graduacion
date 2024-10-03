using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.Reports.Models
{
    public class ProjectsPerMonth
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string MonthYear => $"{Month}/{Year}";
    }
}
