using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HFPMapp.Models
{
    public class Privilege
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public bool? ProjectManager { get; set; }

        public bool? DonorManager { get; set; }

        public bool? AccountingManager { get; set; }

        public bool? IsActive { get; set; }

        public bool UsersManager { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
