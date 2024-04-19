using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LTWeb_CodeFirst.Data;
using LTWeb_CodeFirst.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Authorization;

namespace LTWeb_CodeFirst.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CarTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CarsType.ToListAsync());
        }

        // GET: CarTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carType = await _context.CarsType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carType == null)
            {
                return NotFound();
            }
            var carTypeCompanies = _context.CarTypeDetails
                .Where(c => c.CarTypeId == carType.Id)
                .Select(c => c.Company.Name)
                .ToList().Distinct();
            ViewData["CarTypeCompanies"] = carTypeCompanies;
            return View(carType);
        }

        // GET: CarTypes/Create
        
        public IActionResult Create()
        {
            var companies = _context.Companies.ToList();
            ViewData["Companies"] = new SelectList(companies, "Id", "Name");
            return View();
        }

        // POST: CarTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,IsDeleted")] CarType carType, int[] SelectedCompanies)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carType);
                await _context.SaveChangesAsync();
                if(SelectedCompanies != null)
                {
                    foreach (var item in SelectedCompanies)
                    {
                        var carTypeDetail = new CarTypeDetail()
                        {
                            CarTypeId = carType.Id,
                            CompanyId = item
                        };
                        _context.CarTypeDetails.Add(carTypeDetail);
                        await _context.SaveChangesAsync();
                    }
                }    
                return RedirectToAction(nameof(Index));
            }
            return View(carType);
        }

        // GET: CarTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carType = await _context.CarsType.FindAsync(id);
            if (carType == null)
            {
                return NotFound();
            }
            return View(carType);
        }

        // POST: CarTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,IsDeleted")] CarType carType)
        {
            if (id != carType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarTypeExists(carType.Id))
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
            return View(carType);
        }

        private bool CarTypeExists(int id)
        {
            return _context.CarsType.Any(e => e.Id == id);
        }
    }
}
