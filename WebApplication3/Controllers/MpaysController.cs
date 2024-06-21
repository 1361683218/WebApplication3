using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                    // Check if there are any employees associated with this Mpay
                    var employeesExist = await _context.Users.AnyAsync(u => u.Pid == mpay.Pid);
                    if (employeesExist)
                    {
                        // Return a view with a message indicating that employees exist and editing is not allowed
                        ViewBag.ErrorMessage = "无法编辑该薪资，因为存在关联的员工。";
                        return View(mpay);
                    }

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
            if (mpay == null)
            {
                return NotFound();
            }

            // Check if there are any employees associated with this Mpay
            var employeesExist = await _context.Users.AnyAsync(u => u.Pid == mpay.Pid);
            if (employeesExist)
            {
                // Return a view with a message indicating that employees exist and deletion is not allowed
                ViewBag.ErrorMessage = "无法删除该薪资，因为存在关联的员工。";
                return View("Delete", mpay);
            }

            _context.Mpays.Remove(mpay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MpayExists(int id)
        {
            return _context.Mpays.Any(e => e.Mid == id);
        }
    }
}