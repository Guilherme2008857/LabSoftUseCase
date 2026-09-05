using AppTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppTask.Controllers
{
    public class TarefaController : Controller
    {
        private readonly DbTasksContext _context;

        public TarefaController(DbTasksContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            var tarefas = await _context.Tarefas
                .Include(t => t.Funcionario)
                .ToListAsync();

            return View(tarefas);
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var tarefa = await _context.Tarefas
                .Include(t => t.Funcionario)
                .FirstOrDefaultAsync(t => t.Codigo == id);

            if (tarefa == null)
                return NotFound();

            return View(tarefa);
        }

        
        public async Task<IActionResult> Create()
        {
            await CarregarFuncionarios();

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Codigo,Descricao,DataPlanejada,DataIniciada,DataFinalizada,DataCancelada,StatusTarefa,Prazo,FuncionarioId")] Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {
                _context.Tarefas.Add(tarefa);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            await CarregarFuncionarios(tarefa.FuncionarioId);

            return View(tarefa);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa == null)
                return NotFound();

            await CarregarFuncionarios(tarefa.FuncionarioId);

            return View(tarefa);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Codigo,Descricao,DataPlanejada,DataIniciada,DataFinalizada,DataCancelada,StatusTarefa,Prazo,FuncionarioId")] Tarefa tarefa)
        {
            if (id != tarefa.Codigo)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tarefa);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TarefaExists(tarefa.Codigo))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await CarregarFuncionarios(tarefa.FuncionarioId);

            return View(tarefa);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var tarefa = await _context.Tarefas
                .Include(t => t.Funcionario)
                .FirstOrDefaultAsync(t => t.Codigo == id);

            if (tarefa == null)
                return NotFound();

            return View(tarefa);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa != null)
            {
                _context.Tarefas.Remove(tarefa);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TarefaExists(int id)
        {
            return _context.Tarefas
                .Any(e => e.Codigo == id);
        }

        private async Task CarregarFuncionarios(int? funcionarioSelecionado = null)
        {
            var funcionarios = await _context.Funcionarios
                .OrderBy(f => f.Nome)
                .ToListAsync();

            ViewBag.FuncionarioId = new SelectList(
                funcionarios,
                "Codigo",
                "Nome",
                funcionarioSelecionado
            );
        }
    }
}