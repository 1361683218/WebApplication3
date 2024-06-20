using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class UsersController : Controller
    {
        private readonly WebdesignContext _context;

        public UsersController(WebdesignContext context)
        {
            _context = context;
        }
        // GET: Users/Import
        public IActionResult Import()
        {
            return View();
        }
        // GET: Users/Import


        // POST: Users/Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length == 0)
                return BadRequest("文件不能为空");

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var user = new User
                        {
                            Uid = int.TryParse(worksheet.Cells[row, 1].Value?.ToString()?.Trim(), out int uid) ? uid : 0,
                            Uname = worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty,
                            Usex = worksheet.Cells[row, 3].Value?.ToString()?.Trim() ?? string.Empty,
                            Urzsj = worksheet.Cells[row, 4].Value?.ToString()?.Trim() ?? string.Empty,
                            Utel = worksheet.Cells[row, 5].Value?.ToString()?.Trim(),
                            Ustatus = worksheet.Cells[row, 6].Value?.ToString()?.Trim() ?? string.Empty,
                            Uadress = worksheet.Cells[row, 7].Value?.ToString()?.Trim(),
                            Pid = int.TryParse(worksheet.Cells[row, 8].Value?.ToString()?.Trim(), out int pid) ? pid : (int?)null,
                            Did = int.TryParse(worksheet.Cells[row, 9].Value?.ToString()?.Trim(), out int did) ? did : (int?)null,
                            Mid = int.TryParse(worksheet.Cells[row, 10].Value?.ToString()?.Trim(), out int mid) ? mid : (int?)null,
                            Usfzh = worksheet.Cells[row, 11].Value?.ToString()?.Trim() ?? string.Empty,
                            Umail = worksheet.Cells[row, 12].Value?.ToString()?.Trim()
                        };

                        _context.Users.Add(user);
                    }

                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Users
        public async Task<IActionResult> Index()
        {
            var webdesignContext = _context.Users.Include(u => u.DidNavigation).Include(u => u.MidNavigation).Include(u => u.PidNavigation);
            return View(await webdesignContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.DidNavigation)
                .Include(u => u.MidNavigation)
                .Include(u => u.PidNavigation)
                .FirstOrDefaultAsync(m => m.Uid == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["Did"] = new SelectList(_context.Depts, "Did", "Did");
            ViewData["Mid"] = new SelectList(_context.Mpays, "Mid", "Mid");
            ViewData["Pid"] = new SelectList(_context.Posts, "Pid", "Pid");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Uid,Uname,Usex,Urzsj,Utel,Ustatus,Uadress,Pid,Did,Mid,Usfzh,Umail")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Did"] = new SelectList(_context.Depts, "Did", "Did", user.Did);
            ViewData["Mid"] = new SelectList(_context.Mpays, "Mid", "Mid", user.Mid);
            ViewData["Pid"] = new SelectList(_context.Posts, "Pid", "Pid", user.Pid);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["Did"] = new SelectList(_context.Depts, "Did", "Did", user.Did);
            ViewData["Mid"] = new SelectList(_context.Mpays, "Mid", "Mid", user.Mid);
            ViewData["Pid"] = new SelectList(_context.Posts, "Pid", "Pid", user.Pid);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Uid,Uname,Usex,Urzsj,Utel,Ustatus,Uadress,Pid,Did,Mid,Usfzh,Umail")] User user)
        {
            if (id != user.Uid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Uid))
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
            ViewData["Did"] = new SelectList(_context.Depts, "Did", "Did", user.Did);
            ViewData["Mid"] = new SelectList(_context.Mpays, "Mid", "Mid", user.Mid);
            ViewData["Pid"] = new SelectList(_context.Posts, "Pid", "Pid", user.Pid);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.DidNavigation)
                .Include(u => u.MidNavigation)
                .Include(u => u.PidNavigation)
                .FirstOrDefaultAsync(m => m.Uid == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Uid == id);
        }
    }
}
