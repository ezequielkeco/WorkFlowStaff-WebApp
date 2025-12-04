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
        public DateTime FechaInicio { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "El salario es obligatorio.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Salario Bruto")]
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
        public decimal AFP
        {
            get
            {
                return Salario * 0.0287m;
            }
        }

        [NotMapped]
        [Display(Name = "ARS")]
        public decimal ARS
        {
            get
            {
                return Salario * 0.0304m; // 3.04%
            }
        }

        [NotMapped]
        [Display(Name = "ISR")]
        public decimal ISR
        {
            get
            {
                decimal salarioAnual = Salario * 12m;

                decimal tramo1 = 416220m;
                decimal tramo2 = 624329m;
                decimal tramo3 = 867123m;

                decimal isrAnual = 0m;

                if (salarioAnual <= tramo1)
                {
                    isrAnual = 0m;
                }
                else if (salarioAnual <= tramo2)
                {
                    isrAnual = (salarioAnual - tramo1) * 0.15m;
                }
                else if (salarioAnual <= tramo3)
                {
                    isrAnual = (tramo2 - tramo1) * 0.15m
                             + (salarioAnual - tramo2) * 0.20m;
                }
                else
                {
                    isrAnual = (tramo2 - tramo1) * 0.15m
                             + (tramo3 - tramo2) * 0.20m
                             + (salarioAnual - tramo3) * 0.25m;
                }

                return isrAnual / 12m;
            }
        }

        public Departamento? Departamento { get; set; }
        public Cargo? Cargo { get; set; }

        [NotMapped]
        [Display(Name = "Salario Neto")]
        public decimal NominaNeta => Salario - AFP - ARS - ISR;
    }
}