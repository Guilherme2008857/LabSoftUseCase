using AppTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppTask.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly DbTasksContext _context;

        public FuncionarioController(DbTasksContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            var funcionarios = await _context.Funcionarios
                .Include(f => f.Departamento)
                .Include(f => f.Gerente)
                .ToListAsync();

            return View(funcionarios);
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var funcionario = await _context.Funcionarios
                .Include(f => f.Departamento)
                .Include(f => f.Gerente)
                .FirstOrDefaultAsync(f => f.Codigo == id);

            if (funcionario == null)
                return NotFound();

            return View(funcionario);
        }

        
        public async Task<IActionResult> Create()
        {
            await CarregarListas();

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Codigo,Nome,Cargo,DepartamentoId,CodigoGerente")] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                _context.Funcionarios.Add(funcionario);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            await CarregarListas(funcionario.DepartamentoId, funcionario.CodigoGerente);

            return View(funcionario);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var funcionario = await _context.Funcionarios.FindAsync(id);

            if (funcionario == null)
                return NotFound();

            await CarregarListas(
                funcionario.DepartamentoId,
                funcionario.CodigoGerente
            );

            return View(funcionario);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Codigo,Nome,Cargo,DepartamentoId,CodigoGerente")] Funcionario funcionario)
        {
            if (id != funcionario.Codigo)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funcionario);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncionarioExists(funcionario.Codigo))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await CarregarListas(
                funcionario.DepartamentoId,
                funcionario.CodigoGerente
            );

            return View(funcionario);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var funcionario = await _context.Funcionarios
                .Include(f => f.Departamento)
                .Include(f => f.Gerente)
                .FirstOrDefaultAsync(f => f.Codigo == id);

            if (funcionario == null)
                return NotFound();

            return View(funcionario);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);

            if (funcionario != null)
            {
                _context.Funcionarios.Remove(funcionario);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FuncionarioExists(int id)
        {
            return _context.Funcionarios
                .Any(e => e.Codigo == id);
        }

        private async Task CarregarListas(
            int? departamentoSelecionado = null,
            int? gerenteSelecionado = null)
        {
            ViewBag.DepartamentoId = new SelectList(
                await _context.Departamentos.ToListAsync(),
                "Codigo",
                "Nome",
                departamentoSelecionado
            );

            var funcionarios = await _context.Funcionarios
                .OrderBy(f => f.Nome)
                .ToListAsync();

            ViewBag.CodigoGerente = new SelectList(
                funcionarios,
                "Codigo",
                "Nome",
                gerenteSelecionado
            );
        }
    }
}