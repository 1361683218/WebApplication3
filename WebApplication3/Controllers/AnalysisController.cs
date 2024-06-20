using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class AnalysisController : Controller
    {
        private readonly WebdesignContext _context;

        public AnalysisController(WebdesignContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 获取所有部门名称
            var departments = _context.Depts.Select(d => d.Dname).Distinct().ToList();
            ViewBag.Departments = departments;
            return View();
        }
        public IActionResult EntryStatistics(string department)
        {
            var query = _context.Users
                                .Include(u => u.DidNavigation) // Include Department navigation property
                                .Where(u => u.Ustatus == "Active"); // Filter by Active status

            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(u => u.DidNavigation.Dname == department);
            }

            var entries = query.AsEnumerable() // Move to client-side evaluation
                               .GroupBy(u => new
                               {
                                   Department = u.DidNavigation.Dname,
                                   EntryDate = DateTime.ParseExact(u.Urzsj, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                               })
                               .Select(g => new AnalysisModel.EmployeeEntry
                               {
                                   Department = g.Key.Department,
                                   Month = g.Key.EntryDate.Month,
                                   Year = g.Key.EntryDate.Year,
                                   EntryCount = g.Count()
                               })
                               .ToList();

            // Calculate the total entry count for the selected department
            int totalEntryCount = entries.Sum(e => e.EntryCount);

            // Pass the total entry count to the view
            ViewBag.TotalEntryCount = totalEntryCount;

            // Pass the departments list to the view
            var departments = _context.Depts.Select(d => d.Dname).Distinct().ToList();
            ViewBag.Departments = departments;

            return View(entries);
        }
        public IActionResult ExitStatistics(string department)
        {
            var query = _context.Users
                                .Include(u => u.DidNavigation) // Include Department navigation property
                                .Where(u => u.Ustatus == "Inactive"); // Filter by Inactive status

            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(u => u.DidNavigation.Dname == department);
            }

            var exits = query.AsEnumerable() // Move to client-side evaluation
                             .GroupBy(u => new
                             {
                                 Department = u.DidNavigation.Dname,
                                 EntryDate = DateTime.ParseExact(u.Urzsj, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                             })
                             .Select(g => new AnalysisModel.EmployeeExit
                             {
                                 Department = g.Key.Department,
                                 Month = g.Key.EntryDate.Month,
                                 Year = g.Key.EntryDate.Year,
                                 ExitCount = g.Count()
                             })
                             .ToList();

            // Calculate the total exit count for the selected department
            int totalExitCount = exits.Sum(e => e.ExitCount);

            // Pass the total exit count to the view
            ViewBag.TotalExitCount = totalExitCount;

            // Pass the departments list to the view
            var departments = _context.Depts.Select(d => d.Dname).Distinct().ToList();
            ViewBag.Departments = departments;

            return View(exits);
        }
        public IActionResult SalaryStatistics(string department)
        {
            var salaries = _context.Users
                                   .Include(u => u.DidNavigation) // Include Department navigation property
                                   .Include(u => u.MidNavigation) // Include Salary navigation property
                                   .Where(u => u.DidNavigation.Dname == department)
                                   .GroupBy(u => u.DidNavigation.Dname)
                                   .Select(g => new AnalysisModel.EmployeeSalary
                                   {
                                       Department = g.Key,
                                       AverageSalary = (decimal)g.Average(u => u.MidNavigation.Mpay1)
                                   })
                                   .ToList();
            return View(salaries);
        }
    }
}