using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio.")]
        [StringLength(20, ErrorMessage = "El Nombre no puede exceder los 255 caracteres.")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateOnly? StartDate { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        public DateOnly? EndDate { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string? State { get; set; }

        [Required(ErrorMessage = "El presupuesto es obligatorio.")]
        public decimal? Budget { get; set; }

        public string? Location { get; set; }

        public bool? IsActive { get; set; }

        public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

        public virtual ICollection<Beneficiary> Beneficiaries { get; set; } = new List<Beneficiary>();

        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

        public virtual ICollection<Volunteer> Volunteers { get; set; } = new List<Volunteer>();
    }
}
