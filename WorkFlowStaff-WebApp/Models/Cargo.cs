using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WorkFlowStaff_WebApp.Models
{
    public class Cargo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del cargo es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Nombre del Cargo")]
        public string Nombre { get; set; }

        [StringLength(500)]
        [Display(Name = "Responsabilidades")]
        public string Responsabilidades { get; set; }

        public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}