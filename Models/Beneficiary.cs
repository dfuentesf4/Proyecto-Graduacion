using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Models
{
    public class Beneficiary
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio.")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "El Proyecto es obligatorio.")]
        public int? ProjectId { get; set; }

        public bool? IsActive { get; set; }

        public virtual Project? Project { get; set; }
    }
}
