using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Models;

namespace TaskManagementApp.Controllers
{
    public class TaskItemsController : Controller
    {
        private readonly TasksContext _context;

        public TaskItemsController(TasksContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _context.Tasks.ToListAsync());

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var task = await _context.Tasks.FirstOrDefaultAsync(m => m.ID == id);
            return task == null ? NotFound() : View(task);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Description,AssignedTo,Status,SysModified,SysCreated")] Tasks task)
        {
            if (!ModelState.IsValid) return View(task);

            task.ID = Guid.NewGuid();
            task.SysModified = task.SysCreated = DateTime.Now;

            _context.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var task = await _context.Tasks.FindAsync(id);
            return task == null ? NotFound() : View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Title,Description,AssignedTo,Status,SysModified,SysCreated")] Tasks task)
        {
            if (id != task.ID || !ModelState.IsValid) return View(task);

            try
            {
                task.SysModified = DateTime.Now;

                _context.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(task.ID)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var task = await _context.Tasks.FirstOrDefaultAsync(m => m.ID == id);
            return task == null ? NotFound() : View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null) _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MarkAsCompleted(Guid? id)
        {
            if (id == null) return NotFound();

            var selectedTask = await _context.Tasks.SingleOrDefaultAsync(i => i.ID == id);

            if (selectedTask != null)
            {
                selectedTask.Status = Status.Completed;
                selectedTask.SysModified = DateTime.Now;

                try
                {
                    _context.Update(selectedTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        private bool TaskExists(Guid id) => _context.Tasks.Any(e => e.ID == id);
    }
}
