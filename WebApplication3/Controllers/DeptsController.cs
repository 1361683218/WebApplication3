using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class DeptsController : Controller
    {
        private readonly WebdesignContext _context;

        public DeptsController(WebdesignContext context)
        {
            _context = context;
        }

        // GET: Depts
        public async Task<IActionResult> Index(string searchString)
        {
            var depts = from d in _context.Depts
                        select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                int searchId;
                if (int.TryParse(searchString, out searchId))
                {
                    depts = depts.Where(d => d.Did == searchId);
                }
            }

            return View(await depts.ToListAsync());
        }
        // GET: Depts/Search
        public async Task<IActionResult> Search(string searchString)
        {
            var depts = from d in _context.Depts
                        select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                depts = depts.Where(d => d.Dname.Contains(searchString) || d.Dtel.Contains(searchString));
            }

            return View(await depts.ToListAsync());
        }

        // GET: Depts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dept = await _context.Depts
                .FirstOrDefaultAsync(m => m.Did == id);
            if (dept == null)
            {
                return NotFound();
            }

            return View(dept);
        }

        // GET: Depts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Depts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Did,Dname,Dtel")] Dept dept)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dept);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dept);
        }

        // GET: Depts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dept = await _context.Depts.FindAsync(id);
            if (dept == null)
            {
                return NotFound();
            }
            return View(dept);
        }

        // POST: Depts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Did,Dname,Dtel")] Dept dept)
        {
            if (id != dept.Did)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dept);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeptExists(dept.Did))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dept);
        }

        // GET: Depts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dept = await _context.Depts
                .FirstOrDefaultAsync(m => m.Did == id);
            if (dept == null)
            {
                return NotFound();
            }

            return View(dept);
        }

        // POST: Depts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dept = await _context.Depts.FindAsync(id);
            if (dept != null)
            {
                _context.Depts.Remove(dept);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeptExists(int id)
        {
            return _context.Depts.Any(e => e.Did == id);
        }
    }
}