﻿using System;
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
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var cars = _context.Cars.Include(c => c.CarType).Include(c => c.Company).Include(c => c.Warranty);
            return View(await cars.ToListAsync());
        }
        // GET: Cars/Details/5
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

        public IActionResult GetCompanyCarType(int carTypeId)
        {
            var companies = _context.CarTypeDetails
                            .Where(c => c.CarTypeId == carTypeId)
                            .Select(c => c.Company)
                            .ToList().Distinct();
            // Tạo danh sách SelectListItem từ dữ liệu companies
            var companyItems = companies.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            // Thêm một option mặc định vào đầu danh sách
            companyItems.Insert(0, new SelectListItem { Value = "", Text = "-- Chọn hãng xe --" });

            // Đặt danh sách vào ViewBag
            ViewBag.CompanyId = companyItems;

            return Json(companies);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["CarTypeId"] = new SelectList(_context.CarsType, "Id", "Name");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["WarrantyId"] = new SelectList(_context.Warranties, "Id", "Content");
            return View();
        }
        public async Task<string> SaveImage(IFormFile file)
        {
            var savePath = "./wwwroot/images/";
            var filePath = Path.Combine(savePath + file?.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file!.CopyToAsync(fileStream);
            }
            return file.FileName;
        }
        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Seat,Price,CarTypeId,CompanyId,WarrantyId,Id,IsDeleted")] Car car, string Gear, IFormFile? CarImages)
        {
            if(Gear == "Số tự động")
            {
                car.Gear = true;
            }    
            else
            {
                car.Gear = false;
            }
            if (ModelState.IsValid)
            {
                if (CarImages != null)
                {
                    car.CarImages = await SaveImage(CarImages);
                }
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarTypeId"] = new SelectList(_context.CarsType, "Id", "Name", car.CarTypeId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", car.CompanyId);
            ViewData["WarrantyId"] = new SelectList(_context.Warranties, "Id", "Content", car.WarrantyId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["CarTypeId"] = new SelectList(_context.CarsType, "Id", "Name", car.CarTypeId);
            ViewData["WarrantyId"] = new SelectList(_context.Warranties, "Id", "Content", car.WarrantyId);
            var carCompanies = _context.CarTypeDetails
                            .Where(c => c.CarTypeId == car.CarTypeId)
                            .Select(c => c.Company)
                            .ToList().Distinct();
            ViewData["CarCompanies"] = new SelectList(carCompanies, "Id", "Name", car.CompanyId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.

        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.[A[
        [Authorize(Roles = "Admin")]        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Seat,Price,CarTypeId,CompanyId,WarrantyId,Id,IsDeleted")] Car car, string gear,IFormFile? carImages)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCar = await _context.Cars.FindAsync(id);

                    if (existingCar == null)
                    {
                        return NotFound();
                    }

                    if (carImages != null)
                    {
                        // Đã tải lên hình ảnh mới
                        existingCar.CarImages = await SaveImage(carImages);
                    }
                    // Người dùng không tải lên hình ảnh mới, giữ lại hình ảnh cũ
                    // Không cần thêm else vì mặc định existingCar.CarImages đã được giữ lại

                    // Cập nhật các thuộc tính của existingCar từ đối tượng car được chỉnh sửa
                    existingCar.Name = car.Name;
                    existingCar.Seat = car.Seat;
                    existingCar.Price = car.Price;
                    existingCar.CarTypeId = car.CarTypeId;
                    existingCar.CompanyId = car.CompanyId;
                    existingCar.WarrantyId = car.WarrantyId;
                    existingCar.IsDeleted = car.IsDeleted;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _context.Update(existingCar);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
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
            ViewData["CarTypeId"] = new SelectList(_context.CarsType, "Id", "Name", car.CarTypeId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", car.CompanyId);
            ViewData["WarrantyId"] = new SelectList(_context.Warranties, "Id", "Content", car.WarrantyId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
