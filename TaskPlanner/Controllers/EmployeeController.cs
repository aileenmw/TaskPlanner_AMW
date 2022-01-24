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
    public class EmployeeController : Controller
    {
        private readonly DatabaseContext _context;

        public EmployeeController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var employees = await _context.Employees.ToListAsync(); 
            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }

        public async Task<IActionResult> Edit(int?id)
        {
            if (id == null || id <= 0)
                return BadRequest();

                var employeeInDb = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employeeInDb == null)
                return NotFound();

            return View(employeeInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(Employee employee)
        {
            if (!ModelState.IsValid)
                return View(employee);

            //_context.Entry(employee).State = EntityState.Modified;
            _context.Employees.Update(employee);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
                return BadRequest();

            var employeeInDb = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employeeInDb == null)
                return NotFound();

            _context.Employees.Remove(employeeInDb);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
