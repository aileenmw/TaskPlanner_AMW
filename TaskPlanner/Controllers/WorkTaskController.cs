using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskPlanner.Data;
using TaskPlanner.Models;

namespace TaskPlanner.Controllers
{
    public class WorkTaskController : Controller
    {
        private readonly DatabaseContext _context;

        public WorkTaskController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var tasks = await _context.Tasks.ToListAsync();
            return View(tasks);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(WorkTask task)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(task);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
                return BadRequest();

            var taskInDb = await _context.Tasks.FirstOrDefaultAsync(e => e.Id == id);

            if (taskInDb == null)
                return NotFound();

            return View(taskInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(WorkTask task)
        {
            if (!ModelState.IsValid)
                return View(task);

            _context.Tasks.Update(task);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
                return BadRequest();

            var taskInDb = await _context.Tasks.FirstOrDefaultAsync(e => e.Id == id);

            if (taskInDb == null)
                return NotFound();

            _context.Tasks.Remove(taskInDb);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
