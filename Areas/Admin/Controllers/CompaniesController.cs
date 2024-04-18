using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LTWeb_CodeFirst.Data;
using LTWeb_CodeFirst.Models;

namespace LTWeb_CodeFirst.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Companies.ToListAsync());
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            var companyCarTypes = _context.CarTypeDetails
                .Where(c => c.CompanyId == company.Id)
                .Select(c => c.CarType.Name)
                .ToList();
            ViewData["companyCarTypes"] = companyCarTypes;
            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            var carTypes = _context.CarsType.ToList();
            ViewData["CarTypes"] = new SelectList(carTypes, "Id", "Name");
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,IsDeleted")] Company company, int[] SelectedCarTypes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                await GetSelectedCarTypes(company, SelectedCarTypes);
                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        private async Task GetSelectedCarTypes(Company company, int[] SelectedCarTypes)
        {
            if (SelectedCarTypes != null)
            {
                foreach (var item in SelectedCarTypes)
                {
                    var carTypeDetail = new CarTypeDetail()
                    {
                        CarTypeId = item,
                        CompanyId = company.Id
                    };
                    _context.CarTypeDetails.Add(carTypeDetail);
                    await _context.SaveChangesAsync();
                }
            }
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            var companyCarTypes = _context.CarTypeDetails
                .Where(c => c.CompanyId == company.Id)
                .Select(c => c.CarType.Name)
                .ToList().Distinct();
            ViewData["companyCarTypes"] = companyCarTypes;
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,IsDeleted")] Company company, int[] SelectedCarTypes)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
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
            return View(company);
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
