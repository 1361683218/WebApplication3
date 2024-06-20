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
    public class MpaysController : Controller
    {
        private readonly WebdesignContext _context;

        public MpaysController(WebdesignContext context)
        {
            _context = context;
        }

        // GET: Mpays
        public async Task<IActionResult> Index()
        {
            var webdesignContext = _context.Mpays.Include(m => m.PidNavigation);
            return View(await webdesignContext.ToListAsync());
        }

        // GET: Mpays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mpay = await _context.Mpays
                .Include(m => m.PidNavigation)
                .FirstOrDefaultAsync(m => m.Mid == id);
            if (mpay == null)
            {
                return NotFound();
            }

            return View(mpay);
        }

        // GET: Mpays/Create
        public IActionResult Create()
        {
            ViewData["Pid"] = new SelectList(_context.Posts, "Pid", "Pid");
            return View();
        }

        // POST: Mpays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Mid,Mlevel,Mpay1,Pid")] Mpay mpay)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mpay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Pid"] = new SelectList(_context.Posts, "Pid", "Pid", mpay.Pid);
            return View(mpay);
        }

        // GET: Mpays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mpay = await _context.Mpays.FindAsync(id);
            if (mpay == null)
            {
                return NotFound();
            }
            ViewData["Pid"] = new SelectList(_context.Posts, "Pid", "Pid", mpay.Pid);
            return View(mpay);
        }

        // POST: Mpays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Mid,Mlevel,Mpay1,Pid")] Mpay mpay)
        {
            if (id != mpay.Mid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mpay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MpayExists(mpay.Mid))
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
            ViewData["Pid"] = new SelectList(_context.Posts, "Pid", "Pid", mpay.Pid);
            return View(mpay);
        }

        // GET: Mpays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mpay = await _context.Mpays
                .Include(m => m.PidNavigation)
                .FirstOrDefaultAsync(m => m.Mid == id);
            if (mpay == null)
            {
                return NotFound();
            }

            return View(mpay);
        }

        // POST: Mpays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mpay = await _context.Mpays.FindAsync(id);
            if (mpay != null)
            {
                _context.Mpays.Remove(mpay);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MpayExists(int id)
        {
            return _context.Mpays.Any(e => e.Mid == id);
        }
    }
}
