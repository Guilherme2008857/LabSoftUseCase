using AppTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppTask.Controllers
{
    public class DepartamentoController : Controller
    {
        private readonly DbTasksContext _context;

        public DepartamentoController(DbTasksContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Departamentos.ToListAsync());
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var departamento = await _context.Departamentos
                .FirstOrDefaultAsync(d => d.Codigo == id);

            if (departamento == null)
                return NotFound();

            return View(departamento);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Codigo,Nome,Sigla")] Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departamento);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(departamento);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var departamento = await _context.Departamentos.FindAsync(id);

            if (departamento == null)
                return NotFound();

            return View(departamento);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Codigo,Nome,Sigla")] Departamento departamento)
        {
            if (id != departamento.Codigo)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartamentoExists(departamento.Codigo))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(departamento);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var departamento = await _context.Departamentos
                .FirstOrDefaultAsync(d => d.Codigo == id);

            if (departamento == null)
                return NotFound();

            return View(departamento);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);

            if (departamento != null)
            {
                _context.Departamentos.Remove(departamento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DepartamentoExists(int id)
        {
            return _context.Departamentos.Any(e => e.Codigo == id);
        }
    }
}