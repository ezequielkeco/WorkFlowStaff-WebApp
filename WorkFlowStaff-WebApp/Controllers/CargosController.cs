using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkFlowStaff_WebApp.Data;
using WorkFlowStaff_WebApp.Models;

namespace WorkFlowStaff_WebApp.Controllers
{
    public class CargosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CargosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Cargos";
            return View(await _context.Cargos.ToListAsync());
        }

        public IActionResult GetForm(int? id)
        {
            if (id == null || id == 0)
            {
                return PartialView("_ModalCreateEdit", new Cargo());
            }

            var cargo = _context.Cargos.Find(id);
            if (cargo == null)
            {
                return NotFound();
            }
            return PartialView("_ModalCreateEdit", cargo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Cargo cargo)
        {
            if (ModelState.IsValid)
            {
                if (cargo.Id == 0)
                {
                    _context.Add(cargo);
                }
                else
                {
                    _context.Update(cargo);
                }
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return PartialView("_ModalCreateEdit", cargo);
        }

        public async Task<IActionResult> GetDeleteConfirmation(int id)
        {
            var cargo = await _context.Cargos.FirstOrDefaultAsync(m => m.Id == id);
            if (cargo == null) return NotFound();

            var employeesCount = await _context.Empleados.CountAsync(e => e.CargoId == id);
            ViewData["CanDelete"] = employeesCount == 0;
            ViewData["EmployeeCount"] = employeesCount;

            return PartialView("_ModalDelete", cargo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cargo = await _context.Cargos.FindAsync(id);
            var employeesCount = await _context.Empleados.CountAsync(e => e.CargoId == id);

            if (cargo != null && employeesCount == 0)
            {
                _context.Cargos.Remove(cargo);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "No se puede eliminar porque tiene empleados asociados." });
        }
    }
}