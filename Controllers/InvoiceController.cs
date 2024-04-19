using LTWeb_CodeFirst.Data;
using LTWeb_CodeFirst.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LTWeb_CodeFirst.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Invoice(int id,string userId)
        {
            var item = await _context.Cars.FindAsync(id);
            if (item == null)
            {
                return BadRequest("Item not found");
            }
            var invoice = new Invoice
            {
               CreateOn = DateTime.Now,
               UserId = userId,
               Total = item.Price
            };
            return View(item);
        }
    }
}
