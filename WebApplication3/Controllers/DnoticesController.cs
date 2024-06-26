﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace WebApplication3.Controllers
{
    public class DnoticesController : Controller
    {
        private readonly WebdesignContext _context;
        private readonly ILogger<DnoticesController> _logger;

        public DnoticesController(WebdesignContext context, ILogger<DnoticesController> logger)
        {
            _context = context;
            _logger = logger;
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
        public async Task<IActionResult> Create(int? uid)
        {
            if (uid.HasValue)
            {
                var user = await _context.Users.FindAsync(uid.Value);
                if (user != null)
                {
                    ViewBag.Npost = new SelectList(_context.Posts, "Pid", "Pid", user.Pid);
                    ViewBag.Nposto = new SelectList(_context.Posts.Where(p => p.Pid != user.Pid), "Pid", "Pid");
                    ViewBag.Nuid = new SelectList(_context.Users, "Uid", "Uid", uid.Value);
                    return View(new Dnotice { Nuid = uid.Value, Npost = user.Pid.Value });
                }
            }
            ViewBag.Npost = new SelectList(_context.Posts, "Pid", "Pid");
            ViewBag.Nposto = new SelectList(_context.Posts, "Pid", "Pid");
            ViewBag.Nuid = new SelectList(_context.Users, "Uid", "Uid");
            return View();
        }

        // POST: Dnotices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nuid,Npost,Nposto,Naddtime,Ncontime")] Dnotice dnotice)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 检查 Nuid 是否已经存在
                    var existingDnotice = await _context.Dnotices.FindAsync(dnotice.Nuid);
                    if (existingDnotice != null)
                    {
                        ModelState.AddModelError("Nuid", "The Nuid already exists.");
                        ViewData["Npost"] = new SelectList(_context.Posts, "Pid", "Pname", dnotice.Npost);
                        ViewData["Nposto"] = new SelectList(_context.Posts.Where(p => p.Pid != dnotice.Npost), "Pid", "Pname", dnotice.Nposto);
                        ViewData["Nuid"] = new SelectList(_context.Users, "Uid", "Uid", dnotice.Nuid);
                        return View(dnotice);
                    }

                    // 获取用户并记录原本岗位ID
                    var user = await _context.Users.FindAsync(dnotice.Nuid);
                    if (user != null)
                    {
                        dnotice.Npost = user.Pid.Value; // 记录原本岗位ID
                        user.Pid = dnotice.Nposto; // 更新员工的岗位信息
                        user.Ustatus = "Inactive"; // 更新员工状态
                    }

                    // 检查是否已经存在该用户的调岗记录，并且只处理最新的那一条
                    var existingRecords = await _context.Dnotices.Where(d => d.Nuid == dnotice.Nuid).ToListAsync();
                    if (existingRecords.Any())
                    {
                        var latestRecord = existingRecords.OrderByDescending(d => d.Naddtime).First();
                        _context.Dnotices.Remove(latestRecord);
                    }

                    _context.Add(dnotice);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Error saving new Dnotice record.");
                    ModelState.AddModelError("", "Unable to save changes. Please try again.");
                }
            }
            ViewData["Npost"] = new SelectList(_context.Posts, "Pid", "Pname", dnotice.Npost);
            ViewData["Nposto"] = new SelectList(_context.Posts.Where(p => p.Pid != dnotice.Npost), "Pid", "Pname", dnotice.Nposto);
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

            var user = await _context.Users.FindAsync(dnotice.Nuid);
            if (user != null)
            {
                ViewData["Npost"] = new SelectList(_context.Posts, "Pid", "Pid", user.Pid);
            }
            ViewData["Nposto"] = new SelectList(_context.Posts.Where(p => p.Pid != dnotice.Npost), "Pid", "Pid", dnotice.Nposto);
            ViewData["Nuid"] = new SelectList(_context.Users, "Uid", "Uid", dnotice.Nuid);
            return View(dnotice);
        }

        // POST: Dnotices/Edit/5
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
                    // 获取用户并更新其状态为 Inactive
                    var user = await _context.Users.FindAsync(dnotice.Nuid);
                    if (user != null)
                    {
                        user.Ustatus = "Inactive";
                        user.Pid = dnotice.Nposto; // 更新员工的岗位信息
                    }

                    // 检查是否已经存在该用户的调岗记录，并且只处理最新的那一条
                    var existingRecords = await _context.Dnotices.Where(d => d.Nuid == dnotice.Nuid).ToListAsync();
                    if (existingRecords.Any())
                    {
                        var latestRecord = existingRecords.OrderByDescending(d => d.Naddtime).First();
                        _context.Dnotices.Remove(latestRecord);
                    }

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
                        _logger.LogError("Concurrency error updating Dnotice record.");
                        ModelState.AddModelError("", "Unable to save changes. Another user has updated this record.");
                    }
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Error updating Dnotice record.");
                    ModelState.AddModelError("", "Unable to save changes. Please try again.");
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Npost"] = new SelectList(_context.Posts, "Pid", "Pname", dnotice.Npost);
            ViewData["Nposto"] = new SelectList(_context.Posts.Where(p => p.Pid != dnotice.Npost), "Pid", "Pname", dnotice.Nposto);
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

        [HttpGet]
        public async Task<IActionResult> GetUserPost(int uid)
        {
            var user = await _context.Users.FindAsync(uid);
            if (user != null)
            {
                return Json(new { postId = user.Pid });
            }
            return Json(new { postId = 0 });
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailablePosts(int excludePostId)
        {
            var posts = await _context.Posts
                .Where(p => p.Pid != excludePostId)
                .Select(p => new { p.Pid, p.Pname })
                .ToListAsync();
            return Json(posts);
        }
    }

}