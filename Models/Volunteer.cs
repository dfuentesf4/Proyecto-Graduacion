using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Models
{
    public class Volunteer
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Role { get; set; }

        public int? ProjectId { get; set; }

        public bool? IsActive { get; set; }

        public virtual Project? Project { get; set; }

    }
}
