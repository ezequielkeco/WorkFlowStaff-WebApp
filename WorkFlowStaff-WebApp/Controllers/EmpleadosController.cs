using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkFlowStaff_WebApp.Data;
using WorkFlowStaff_WebApp.Models;

namespace WorkFlowStaff_WebApp.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpleadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        private void PopulateDropdowns()
        {
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "Id", "Nombre");
            ViewData["CargoId"] = new SelectList(_context.Cargos, "Id", "Nombre");
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Empleados";

            var empleados = await _context.Empleados
                .Include(e => e.Cargo)
                .Include(e => e.Departamento)
                .ToListAsync();

            return View(empleados);
        }

        public IActionResult GetForm(int? id)
        {
            PopulateDropdowns();

            if (id == null || id == 0)
            {
                return PartialView("_ModalCreateEdit", new Empleado());
            }

            var empleado = _context.Empleados.Find(id);
            if (empleado == null) return NotFound();

            return PartialView("_ModalCreateEdit", empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Empleado empleado)
        {

            if (ModelState.IsValid)
            {
                if (empleado.EmpleadoId == 0)
                {
                    _context.Add(empleado);
                }
                else
                {
                    _context.Update(empleado);
                }
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            PopulateDropdowns();
            return PartialView("_ModalCreateEdit", empleado);
        }

        public async Task<IActionResult> GetDeleteConfirmation(int id)
        {
            var empleado = await _context.Empleados.FirstOrDefaultAsync(m => m.EmpleadoId == id);
            if (empleado == null) return NotFound();

            return PartialView("_ModalDelete", empleado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);

            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "El empleado no pudo ser encontrado o ya fue eliminado." });
        }
    }
}