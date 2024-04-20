using LTWeb_CodeFirst.Data;
using LTWeb_CodeFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LTWeb_CodeFirst.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string query,int pageNumber = 1)
        {
            int pageSize = 5;
            IQueryable<Car> carsQuery;
            if (query != null)
            {
                carsQuery = _context.Cars.Include(p => p.Warranty).Where(b => b.Name.Contains(query) && b.IsDeleted == false);
            }
            else
            {
                carsQuery = _context.Cars.Include(p => p.Warranty).Where(b => b.IsDeleted == false);
            }
            var paginatedCar = await PaginatedList<Car>.CreateAsync(carsQuery, pageNumber, pageSize);
            ViewData["SearchString"] = query;
            ViewData["Companies"] = _context.Cars.Select(f => f.Company).ToList();
            ViewData["CarTypes"] = _context.Cars.Select(f => f.CarType).ToList();
            return View(paginatedCar);
        }


        public async Task<List<string?>> SearchSuggestions(string query)
        {
            return _context.Cars

            .Where(p => p.Name.Contains(query))
            .Select(p => p.Name)
            .ToList();


        }



        public IActionResult dk()
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
