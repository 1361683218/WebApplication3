using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication3.Controllers
{
    public class PostsController : Controller
    {
        private readonly WebdesignContext _context;

        public PostsController(WebdesignContext context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var webdesignContext = _context.Posts.Include(p => p.DidNavigation);
            return View(await webdesignContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.DidNavigation)
                .FirstOrDefaultAsync(m => m.Pid == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["Did"] = new SelectList(_context.Depts, "Did", "Did");
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Pid,Pname,Did")] Post post)
        {
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Did"] = new SelectList(_context.Depts, "Did", "Did", post.Did);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["Did"] = new SelectList(_context.Depts, "Did", "Did", post.Did);
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Pid,Pname,Did")] Post post)
        {
            if (id != post.Pid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if there are any employees associated with this post
                    var employeesExist = await _context.Users.AnyAsync(u => u.Pid == id);
                    if (employeesExist)
                    {
                        // Return a view with a message indicating that employees exist and editing is not allowed
                        ViewBag.ErrorMessage = "无法编辑该职位，因为存在关联的员工。";
                        return View(post);
                    }

                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Pid))
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
            ViewData["Did"] = new SelectList(_context.Depts, "Did", "Did", post.Did);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.DidNavigation)
                .FirstOrDefaultAsync(m => m.Pid == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            // Check if there are any employees associated with this post
            var employeesExist = await _context.Users.AnyAsync(u => u.Pid == id);
            if (employeesExist)
            {
                // Return a view with a message indicating that employees exist and deletion is not allowed
                ViewBag.ErrorMessage = "无法删除该职位，因为存在关联的员工。";
                return View("Delete", post);
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Pid == id);
        }
    }
}