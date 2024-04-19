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
            int pageSize = 1;
            IQueryable<Car> productsQuery;
            if (query != null)
            {
                productsQuery = _context.Cars.Include(p => p.Warranty).Where(b => b.Name.Contains(query));
            }
            else
            {
                productsQuery = _context.Cars.Include(p => p.Warranty);
            }
            var paginatedCar = await PaginatedList<Car>.CreateAsync(productsQuery, pageNumber, pageSize);
            ViewData["SearchString"] = query;
            return View(paginatedCar);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.CarType)
                .Include(c => c.Company)
                .Include(c => c.Warranty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
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
