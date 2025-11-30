using WorkFlowStaff_WebApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowStaff_WebApp.Models
{
    public class Empleado
    {
        [Key]
        public int EmpleadoId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(150)]
        [Display(Name = "Nombre del Empleado")]
        public string Nombre { get; set; }

        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "Debe asignar un departamento.")]
        public int DepartamentoId { get; set; }

        [Display(Name = "Cargo")]
        [Required(ErrorMessage = "Debe asignar un cargo.")]
        public int CargoId { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "El salario es obligatorio.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Salario")]
        public decimal Salario { get; set; }

        [Display(Name = "Vigente")]
        public bool EstadoVigente { get; set; } = true;

        [NotMapped]
        [Display(Name = "Tiempo en la Empresa")]
        public string TiempoEnEmpresa
        {
            get
            {
                if (EstadoVigente)
                {
                    var span = DateTime.Now - FechaInicio;
                    int years = (int)(span.Days / 365.25);
                    int months = (int)((span.Days % 365.25) / 30.4375);
                    return $"{years} años, {months} meses";
                }
                return "No Vigente";
            }
        }

        [NotMapped]
        [Display(Name = "AFP")]
        public decimal AFP => Salario * 0.0287m;

        [NotMapped]
        [Display(Name = "ARS")]
        public decimal ARS => Salario * 0.0304m;

        [NotMapped]
        [Display(Name = "ISR")]
        public decimal ISR => (Salario > 40000) ? Salario * 0.10m : 0m;

        public Departamento? Departamento { get; set; }
        public Cargo? Cargo { get; set; }
    }
}