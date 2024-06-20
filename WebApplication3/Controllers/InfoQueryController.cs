using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication3.Models;
namespace WebApplication3.Controllers
{
    public class InfoQueryController : Controller
    {
        private readonly WebdesignContext _context;

        public InfoQueryController(WebdesignContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.Include(u => u.DidNavigation).Include(u => u.PidNavigation).ToList();
            return View(users);
        }

        public IActionResult Search(string idCard, string name, string email, string department)
        {
            var query = _context.Users.Include(u => u.DidNavigation).Include(u => u.PidNavigation).AsQueryable();

            if (!string.IsNullOrEmpty(idCard))
            {
                query = query.Where(e => e.Usfzh.Contains(idCard));
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Uname.Contains(name));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(e => e.Umail.Contains(email));
            }

            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(e => e.DidNavigation.Dname.Contains(department));
            }

            var result = query.ToList();
            return View("Index", result);
        }
    }
}
