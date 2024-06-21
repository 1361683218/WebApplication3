using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication3.Models;

public class EmployeeController : Controller
{
    private readonly WebdesignContext _context;

    public EmployeeController(WebdesignContext context)
    {
        _context = context;
    }

    // 员工信息查询
    public IActionResult Index(string idCard, string name, int? uid)
    {
        var query = _context.Users.Include(u => u.DidNavigation).Include(u => u.PidNavigation).AsQueryable();

        if (!string.IsNullOrEmpty(idCard))
        {
            query = query.Where(e => e.Usfzh == idCard);
        }

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(e => e.Uname == name);
        }

        if (uid.HasValue)
        {
            query = query.Where(e => e.Uid == uid.Value);
        }

        var result = query.FirstOrDefault();
        return View(result);
    }

    // 员工信息修改
    [HttpPost]
    public IActionResult Update(User user)
    {
        var existingUser = _context.Users.Find(user.Uid);
        if (existingUser == null)
        {
            return NotFound();
        }

        // 更新用户信息
        existingUser.Uname = user.Uname;
        existingUser.Umail = user.Umail;
        existingUser.Uadress = user.Uadress;
        existingUser.Utel = user.Utel;
        existingUser.Usex = user.Usex;
        existingUser.Ustatus = user.Ustatus;

        _context.SaveChanges();
        return RedirectToAction("Index", new { idCard = existingUser.Usfzh, name = existingUser.Uname });
    }

    // 调岗信息查询
    public IActionResult TransferInfo(string idCard, string name, int? uid)
    {
        var query = _context.Users.Include(u => u.DidNavigation).Include(u => u.PidNavigation).AsQueryable();

        if (!string.IsNullOrEmpty(idCard))
        {
            query = query.Where(e => e.Usfzh == idCard);
        }

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(e => e.Uname == name);
        }

        if (uid.HasValue)
        {
            query = query.Where(e => e.Uid == uid.Value);
        }

        var user = query.FirstOrDefault();
        if (user != null)
        {
            // 检查是否有调岗记录
            var transferRecord = _context.Dnotices.FirstOrDefault(d => d.Nuid == user.Uid);
            ViewBag.HasTransferRecord = transferRecord != null;
        }
        else
        {
            ViewBag.HasTransferRecord = false;
        }

        return View(user);
    }

    // 调岗确认
    [HttpPost]
    public IActionResult ConfirmTransfer(int uid)
    {
        var user = _context.Users.Find(uid);
        if (user == null)
        {
            return NotFound();
        }

        user.Ustatus = "Active"; // 更新状态为Active

        // 删除对应的调岗记录
        var transferRecord = _context.Dnotices.FirstOrDefault(d => d.Nuid == user.Uid);
        if (transferRecord != null)
        {
            _context.Dnotices.Remove(transferRecord);
        }

        _context.SaveChanges();
        return RedirectToAction("TransferInfo", new { idCard = user.Usfzh, name = user.Uname });
    }
}