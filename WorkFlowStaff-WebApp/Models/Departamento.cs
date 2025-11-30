using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WorkFlowStaff_WebApp.Models
{
    public class Departamento
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del departamento es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Nombre del Departamento")]
        public string Nombre { get; set; }

        [StringLength(500)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}