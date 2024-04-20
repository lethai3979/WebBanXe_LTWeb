using LTWeb_CodeFirst.Data;
using LTWeb_CodeFirst.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LTWeb_CodeFirst.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FavoriteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> AddToFavorite(int id, string userId)
        {
            //Check xem item co ton tai hay khong
            var item = await _context.Cars.FindAsync(id);
            if (item == null)
            {
                return BadRequest("Item not found");
            }
            //Check xem xe da duoc yeu thich hay chua
            var exisitingFavorite = await _context.FavoriteLists.FirstOrDefaultAsync(f => f.UserId == userId && f.CarId == id);
            if (exisitingFavorite == null)
            {
                var Favorite = new FavoriteList
                {
                    UserId = userId,
                    CarId = id,
                };
                _context.FavoriteLists.Add(Favorite);
                _context.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            exisitingFavorite.IsDeleted = false;
            _context.FavoriteLists.Update(exisitingFavorite);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Favorite(string userId)
        {
            ViewData["Cars"] = _context.FavoriteLists.Select(f => f.Car).ToList();
            var cars = _context.FavoriteLists.Where(p => p.UserId == userId && p.IsDeleted == false).ToList();
            return View(cars);
        }

        public IActionResult Details(int id) 
        {

            return RedirectToAction("Details","Cars",new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id, string userId)
        {
            if (id == null || userId == null)
            {
                return NotFound();
            }
            var car = await _context.FavoriteLists.Where(p => p.CarId == id && p.UserId == userId).FirstOrDefaultAsync();
            if (car == null)
            {
                return NotFound();
            }
            car.IsDeleted = true;
            _context.FavoriteLists.Update(car);
            _context.SaveChanges();
            return RedirectToAction("Favorite", new {userId = userId});
        }
    } 
}
