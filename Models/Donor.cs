using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Models
{
    public class Donor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [Phone(ErrorMessage = "El número de teléfono no es válido.")]
        [StringLength(20, ErrorMessage = "El número de teléfono no puede exceder los 20 caracteres.")]
        public string? PhoneNumber { get; set; }

        [StringLength(255, ErrorMessage = "La direccion no puede sobrepasar los 255 caracteres.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateOnly? BirthDate { get; set; }

        [Required(ErrorMessage = "El género es obligatorio.")]
        [StringLength(10, ErrorMessage = "El género no puede exceder los 10 caracteres.")]
        public string? Gender { get; set; }

        public string? Observations { get; set; }

        public bool? IsActive { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede exceder los 50 caracteres.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string LastName { get; set; } = null!;
    }
}
