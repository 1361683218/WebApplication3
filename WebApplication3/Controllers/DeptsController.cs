using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using System.Linq;
using System.Threading.Tasks;

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

            if (!string.IsNullOrEmpty(searchString))
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

            if (!string.IsNullOrEmpty(searchString))
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
                    // Check if there are any employees associated with this department
                    var employeesExist = await _context.Users.AnyAsync(u => u.Did == id);
                    if (employeesExist)
                    {
                        // Return a view with a message indicating that employees exist and editing is not allowed
                        ViewBag.ErrorMessage = "无法编辑该部门，因为存在关联的员工。";
                        return View(dept);
                    }

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
            if (dept == null)
            {
                return NotFound();
            }

            // Check if there are any employees associated with this department
            var employeesExist = await _context.Users.AnyAsync(u => u.Did == id);
            if (employeesExist)
            {
                // Return a view with a message indicating that employees exist and deletion is not allowed
                ViewBag.ErrorMessage = "无法删除该部门，因为存在关联的员工。";
                return View("Delete", dept);
            }

            _context.Depts.Remove(dept);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeptExists(int id)
        {
            return _context.Depts.Any(e => e.Did == id);
        }
    }
}