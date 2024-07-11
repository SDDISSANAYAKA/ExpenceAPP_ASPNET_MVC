using Microsoft.AspNetCore.Mvc;
using SpendSmart.Models;
using System.Diagnostics;

namespace SpendSmart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SpendSmartDbContext _context;


        public HomeController(ILogger<HomeController> logger, SpendSmartDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Expenses()
        {
            var expenses = _context.Expenses.ToList();
            var totalExpenses = expenses.Sum(e => e.Value);
            ViewBag.Expenses = totalExpenses;
            return View(expenses);
        }

        public IActionResult CreateEditExpenses(int? id)
        {
            if(id != null)
            {
                //editing -> load an expense by id
                var expenseInDb = _context.Expenses.SingleOrDefault(e => e.Id == id);
                return View(expenseInDb);
            }
           return View();
        }

        public IActionResult DeleteExpenses(int id)
        {
            var expenseInDb = _context.Expenses.SingleOrDefault(e => e.Id == id);
            _context.Expenses.Remove(expenseInDb);
            _context.SaveChanges();
            return RedirectToAction("Expenses");
        }

        public IActionResult CreateEditExpensesForm(Expense model)
        {
            if(model.Id == 0)
            {
                //Create
                _context.Expenses.Add(model);
            }
            else
            {
                //Edit
                _context.Expenses.Update(model);
            }
            _context.SaveChanges();
            return RedirectToAction("Expenses");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
