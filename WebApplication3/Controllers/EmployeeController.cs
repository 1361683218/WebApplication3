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
    public IActionResult Index(string idCard, string name)
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
    public IActionResult TransferInfo(string idCard, string name)
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

        var result = query.FirstOrDefault();
        return View(result);
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

        user.Ustatus = "就职";
        _context.SaveChanges();
        return RedirectToAction("TransferInfo", new { idCard = user.Usfzh, name = user.Uname });
    }
}