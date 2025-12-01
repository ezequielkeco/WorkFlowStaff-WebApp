using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkFlowStaff_WebApp.Data;

namespace WorkFlowStaff_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Dashboard";

            var totalEmpleados = await _context.Empleados.CountAsync();
            var empleadosVigentes = await _context.Empleados.CountAsync(e => e.EstadoVigente == true);
            var totalDepartamentos = await _context.Departamentos.CountAsync();

            var salarioPromedioSeguro = await _context.Empleados
                .Where(e => e.EstadoVigente == true)
                .Select(e => (decimal?)e.Salario)
                .DefaultIfEmpty(0)
                .AverageAsync();

            decimal salarioPromedio = salarioPromedioSeguro ?? 0;

            var empleadosPorDepto = await _context.Empleados
                .Where(e => e.EstadoVigente == true)
                .GroupBy(e => e.Departamento!.Nombre)
                .Select(g => new { Departamento = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            ViewData["TotalEmpleados"] = totalEmpleados;
            ViewData["EmpleadosVigentes"] = empleadosVigentes;
            ViewData["TotalDepartamentos"] = totalDepartamentos;

            ViewData["SalarioPromedio"] = salarioPromedio.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("es-DO"));

            ViewData["EmpleadosPorDepto"] = empleadosPorDepto;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}