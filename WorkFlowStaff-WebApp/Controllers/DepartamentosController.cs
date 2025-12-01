using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkFlowStaff_WebApp.Data;
using WorkFlowStaff_WebApp.Models;

namespace WorkFlowStaff_WebApp.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Departamentos";
            return View(await _context.Departamentos.ToListAsync());
        }

        public IActionResult GetForm(int? id)
        {
            if (id == null || id == 0)
            {
                return PartialView("_ModalCreateEdit", new Departamento());
            }

            var departamento = _context.Departamentos.Find(id);
            if (departamento == null)
            {
                return NotFound();
            }
            return PartialView("_ModalCreateEdit", departamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                if (departamento.Id == 0)
                {
                    _context.Add(departamento);
                }
                else
                {
                    _context.Update(departamento);
                }
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return PartialView("_ModalCreateEdit", departamento);
        }

        public async Task<IActionResult> GetDeleteConfirmation(int id)
        {
            var departamento = await _context.Departamentos.FirstOrDefaultAsync(m => m.Id == id);
            if (departamento == null) return NotFound();

            var employeesCount = await _context.Empleados.CountAsync(e => e.DepartamentoId == id);
            ViewData["CanDelete"] = employeesCount == 0;
            ViewData["EmployeeCount"] = employeesCount;

            return PartialView("_ModalDelete", departamento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            var employeesCount = await _context.Empleados.CountAsync(e => e.DepartamentoId == id);

            if (departamento != null && employeesCount == 0)
            {
                _context.Departamentos.Remove(departamento);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "No se puede eliminar porque tiene empleados asociados." });
        }
    }
}