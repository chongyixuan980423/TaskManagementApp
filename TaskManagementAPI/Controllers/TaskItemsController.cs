using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Models;

namespace TaskManagementAPI.Controllers
{
    [Route("api/Task")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly TasksContext _context;

        public TaskItemsController(TasksContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetTasks() => Ok(await _context.Tasks.ToListAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Tasks>> GetTask(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<Tasks>> Create(Tasks task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            task.ID = Guid.NewGuid();
            task.Status = (task.Status != null) ? task.Status : Status.New;
            task.SysCreated = task.SysModified = DateTime.Now;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.ID }, task);
        }

        [HttpPost("Batch")]
        public async Task<ActionResult<IEnumerable<Tasks>>> Create(IEnumerable<Tasks> tasks)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            foreach (var task in tasks)
            {
                task.ID = Guid.NewGuid();
                task.Status = (task.Status != null) ? task.Status : Status.New;
                task.SysCreated = task.SysModified = DateTime.Now;
                _context.Tasks.Add(task);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTasks), tasks);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Tasks task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var selectedTask = await _context.Tasks.FindAsync(id);
            if (selectedTask == null)
                return NotFound();

            selectedTask.Title = (task.Title != null) ? task.Title : selectedTask.Title;
            selectedTask.Description = (task.Description != null) ? task.Description : selectedTask.Description;
            selectedTask.AssignedTo = (task.AssignedTo != null) ? task.AssignedTo : selectedTask.AssignedTo;
            selectedTask.Status = (task.Status != null) ? task.Status : selectedTask.Status;
            selectedTask.SysModified = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TaskExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(Guid id) => _context.Tasks.Any(e => e.ID == id);
    }
}
