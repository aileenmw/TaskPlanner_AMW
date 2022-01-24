using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TaskPlanner.Data;
using TaskPlanner.Helpers;
using TaskPlanner.Models;

namespace TaskPlanner.Controllers
{
    public class ShiftController : Controller
    {

        private readonly DatabaseContext _context;

        public ShiftController(DatabaseContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            var shifts = await _context.Shifts.ToListAsync();
            return View(shifts);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Shift shift)
        {
            if (ModelState.IsValid)
            {
                _context.Shifts.Add(shift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(shift);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
                return BadRequest();

            var shiftInDb = await _context.Shifts.FirstOrDefaultAsync(e => e.Id == id);

            if (shiftInDb == null)
                return NotFound();

            return View(shiftInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Shift shift)
        {
            if (!ModelState.IsValid)
                return View(shift);

            _context.Shifts.Update(shift);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
                return BadRequest();

            var shiftInDb = await _context.Shifts.FirstOrDefaultAsync(e => e.Id == id);

            if (shiftInDb == null)
                return NotFound();

            _context.Shifts.Remove(shiftInDb);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> EditTasks(int? id)
        {
            if (id == null || id <= 0)
                return BadRequest();

            var shiftTaskInDb = await _context.Shifts.FirstOrDefaultAsync(e => e.Id == id);

            if (shiftTaskInDb == null)
                return NotFound();

            return View(shiftTaskInDb);
        }

        public async Task<IActionResult> AddShiftTasks(int? id)
        {
            if (id == null || id <= 0)
                return BadRequest();

            var shiftInDb = await _context.Shifts.FirstOrDefaultAsync(e => e.Id == id);

            if (shiftInDb == null)
                return NotFound();

            return View(shiftInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddShiftTasks(Shift shift)
        {
            if (!ModelState.IsValid)
                return View(shift);

            _context.Shifts.Update(shift);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ShiftTaskAddition(int? id)
        {
            if (id == null || id <= 0)
                return BadRequest();

            var shiftInDb =  _context.Shifts.FirstOrDefaultAsync(e => e.Id == id);

            if (shiftInDb == null)
                return NotFound();

            return View(shiftInDb);
        }

        [HttpPost]
        public IActionResult ShiftTaskAddition()
        {
            int shiftId = Convert.ToInt32(HttpContext.Request.Form["shiftId"]);     
            string taskIds = HttpContext.Request.Form["taskIds"].ToString();

            TaskPlanner.Helpers.DbQueries.addTasksToShift(shiftId, taskIds);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> ViewShiftTask(int? id)
        {
           
            return View();
        }
    }
}
