using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LTWeb_CodeFirst.Data;
using LTWeb_CodeFirst.Models;
using Microsoft.AspNetCore.Authorization;

namespace LTWeb_CodeFirst.Controllers
{
    [Authorize(Roles = "Admin")]
    public class WarrantiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WarrantiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Warranties
        public async Task<IActionResult> Index()
        {
            return View(await _context.Warranties.ToListAsync());
        }

        // GET: Warranties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warranty = await _context.Warranties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warranty == null)
            {
                return NotFound();
            }

            return View(warranty);
        }

        // GET: Warranties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Warranties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,Id,IsDeleted")] Warranty warranty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(warranty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(warranty);
        }

        // GET: Warranties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warranty = await _context.Warranties.FindAsync(id);
            if (warranty == null)
            {
                return NotFound();
            }
            return View(warranty);
        }

        // POST: Warranties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Content,Id,IsDeleted")] Warranty warranty)
        {
            if (id != warranty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(warranty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarrantyExists(warranty.Id))
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
            return View(warranty);
        }

        // GET: Warranties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warranty = await _context.Warranties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warranty == null)
            {
                return NotFound();
            }

            return View(warranty);
        }

        // POST: Warranties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warranty = await _context.Warranties.FindAsync(id);
            if (warranty != null)
            {
                _context.Warranties.Remove(warranty);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WarrantyExists(int id)
        {
            return _context.Warranties.Any(e => e.Id == id);
        }
    }
}
