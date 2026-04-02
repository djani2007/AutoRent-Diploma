using AutoRent.Core.DTOs;
using AutoRent.Core.Entities;
using AutoRent.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoRent.Web.Controllers
{
    [Authorize]
    public class RentalsController : Controller
    {
        private readonly IRentalService _rentalService;
        private readonly ICarService _carService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RentalsController(
            IRentalService rentalService,
            ICarService carService,
            UserManager<ApplicationUser> userManager)
        {
            _rentalService = rentalService;
            _carService = carService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var rentals = await _rentalService.GetRentalsByUserIdAsync(userId);
            return View(rentals);
        }

        public async Task<IActionResult> Create(int carId, string startDate, string endDate)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var car = await _carService.GetCarByIdAsync(carId);
            if (car == null) return NotFound();

            var start = DateTime.Parse(startDate);
            var end = DateTime.Parse(endDate);
            var totalPrice = await _rentalService.CalculateTotalPriceAsync(carId, start, end);

            ViewBag.Car = car;
            ViewBag.StartDate = start;
            ViewBag.EndDate = end;
            ViewBag.TotalPrice = totalPrice;
            ViewBag.TotalDays = (end - start).Days;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int carId, DateTime startDate, DateTime endDate, string? notes)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            try
            {
                var rental = await _rentalService.CreateRentalAsync(new RentalCreateDto
                {
                    UserId = userId,
                    CarId = carId,
                    StartDate = startDate,
                    EndDate = endDate,
                    Notes = notes
                });

                TempData["Success"] = "Rental created successfully!";
                return RedirectToAction(nameof(Details), new { id = rental.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Rent", "Cars", new { id = carId });
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var rental = await _rentalService.GetRentalByIdAsync(id);
            if (rental == null) return NotFound();

            if (rental.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            return View(rental);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var rental = await _rentalService.GetRentalByIdAsync(id);
            if (rental == null) return NotFound();

            if (rental.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            await _rentalService.CancelRentalAsync(id);
            TempData["Success"] = "Rental cancelled successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}