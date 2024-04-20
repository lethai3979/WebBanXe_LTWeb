using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LTWeb_CodeFirst.Data;
using LTWeb_CodeFirst.Models;

namespace LTWeb_CodeFirst.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index(string userId)
        {
            ViewData["Cars"] = _context.Invoices.Select(f => f.Car).ToList();
            ViewData["Promotions"] = _context.Invoices.Select(f => f.Promotion).ToList();
            var invoices = await _context.Invoices.Where(i => i.UserId == userId).ToListAsync();
            return View(invoices);
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Car)
                .Include(i => i.Promotion)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        public IActionResult CalculateTotal(int promotionId, decimal initialTotal)
        {
            var promotion = _context.Promotions.FirstOrDefault(p => p.Id == promotionId);
            if(promotion == null)
            {
                return Json(new { total = initialTotal });
            }    
            else
            {
                if (promotion.DiscountValue <= 1)
                {
                    return Json(new { total = initialTotal - (initialTotal * promotion.DiscountValue) });
                }
                else
                {
                    return Json(new { total = initialTotal - promotion.DiscountValue });
                }
            }    

          
        }


    // GET: Invoices/Create
    public async Task<IActionResult> Create(int id, string userId)
        {
            var car = await _context.Cars.FindAsync(id);
            var invoice = new Invoice()
            {
                CarId = id,
                UserId = userId,
                CreateOn = DateTime.Now,
                Total = car.Price
            };
            ViewData["PromotionId"] = new SelectList(_context.Promotions, "Id", "Content");
            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Total,CreateOn,CarId,PromotionId,UserId,IsDeleted")] Invoice invoice)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Invoices", new { userId = invoice.UserId });
            }
            ViewData["PromotionId"] = new SelectList(_context.Promotions, "Id", "Content");
            return View(invoice);
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
