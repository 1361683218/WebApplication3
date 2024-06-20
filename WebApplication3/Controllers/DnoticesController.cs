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
    public class DnoticesController : Controller
    {
        private readonly WebdesignContext _context;

        public DnoticesController(WebdesignContext context)
        {
            _context = context;
        }

        // GET: Dnotices
        public async Task<IActionResult> Index()
        {
            var webdesignContext = _context.Dnotices.Include(d => d.NpostNavigation).Include(d => d.NpostoNavigation).Include(d => d.Nu);
            return View(await webdesignContext.ToListAsync());
        }

        // GET: Dnotices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dnotice = await _context.Dnotices
                .Include(d => d.NpostNavigation)
                .Include(d => d.NpostoNavigation)
                .Include(d => d.Nu)
                .FirstOrDefaultAsync(m => m.Nuid == id);
            if (dnotice == null)
            {
                return NotFound();
            }

            return View(dnotice);
        }

        // GET: Dnotices/Create
        public IActionResult Create()
        {
            ViewData["Npost"] = new SelectList(_context.Posts, "Pid", "Pid");
            ViewData["Nposto"] = new SelectList(_context.Posts, "Pid", "Pid");
            ViewData["Nuid"] = new SelectList(_context.Users, "Uid", "Uid");
            return View();
        }

        // POST: Dnotices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nuid,Npost,Nposto,Naddtime,Ncontime")] Dnotice dnotice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dnotice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Npost"] = new SelectList(_context.Posts, "Pid", "Pid", dnotice.Npost);
            ViewData["Nposto"] = new SelectList(_context.Posts, "Pid", "Pid", dnotice.Nposto);
            ViewData["Nuid"] = new SelectList(_context.Users, "Uid", "Uid", dnotice.Nuid);
            return View(dnotice);
        }

        // GET: Dnotices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dnotice = await _context.Dnotices.FindAsync(id);
            if (dnotice == null)
            {
                return NotFound();
            }
            ViewData["Npost"] = new SelectList(_context.Posts, "Pid", "Pid", dnotice.Npost);
            ViewData["Nposto"] = new SelectList(_context.Posts, "Pid", "Pid", dnotice.Nposto);
            ViewData["Nuid"] = new SelectList(_context.Users, "Uid", "Uid", dnotice.Nuid);
            return View(dnotice);
        }

        // POST: Dnotices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nuid,Npost,Nposto,Naddtime,Ncontime")] Dnotice dnotice)
        {
            if (id != dnotice.Nuid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dnotice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DnoticeExists(dnotice.Nuid))
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
            ViewData["Npost"] = new SelectList(_context.Posts, "Pid", "Pid", dnotice.Npost);
            ViewData["Nposto"] = new SelectList(_context.Posts, "Pid", "Pid", dnotice.Nposto);
            ViewData["Nuid"] = new SelectList(_context.Users, "Uid", "Uid", dnotice.Nuid);
            return View(dnotice);
        }

        // GET: Dnotices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dnotice = await _context.Dnotices
                .Include(d => d.NpostNavigation)
                .Include(d => d.NpostoNavigation)
                .Include(d => d.Nu)
                .FirstOrDefaultAsync(m => m.Nuid == id);
            if (dnotice == null)
            {
                return NotFound();
            }

            return View(dnotice);
        }

        // POST: Dnotices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dnotice = await _context.Dnotices.FindAsync(id);
            if (dnotice != null)
            {
                _context.Dnotices.Remove(dnotice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DnoticeExists(int id)
        {
            return _context.Dnotices.Any(e => e.Nuid == id);
        }
    }
}
