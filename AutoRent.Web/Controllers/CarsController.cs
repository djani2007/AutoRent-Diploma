using AutoRent.Core.Interfaces;
using AutoRent.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using AutoRent.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;


namespace AutoRent.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarService _carService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CarsController(ICarService carService, UserManager<ApplicationUser> userManager)
        {
            _carService = carService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index( string? brand, string? bodyType, string? transmissionType, string? fuelType, decimal? minPrice, decimal? maxPrice, int? seats)
        {
            var cars = (await _carService.GetAvailableCarsAsync()).ToList();

            var filteredCars = cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(brand))
            {
                var value = brand.Trim().ToLower();
                filteredCars = filteredCars.Where(c => c.Brand != null && c.Brand.ToLower().Contains(value));
            }

            if (!string.IsNullOrWhiteSpace(bodyType))
            {
                var value = bodyType.Trim().ToLower();
                filteredCars = filteredCars.Where(c => c.BodyType != null && c.BodyType.ToLower() == value);
            }

            if (!string.IsNullOrWhiteSpace(transmissionType))
            {
                var value = transmissionType.Trim().ToLower();
                filteredCars = filteredCars.Where(c => c.TransmissionType != null && c.TransmissionType.ToLower() == value);
            }

            if (!string.IsNullOrWhiteSpace(fuelType))
            {
                var value = fuelType.Trim().ToLower();
                filteredCars = filteredCars.Where(c => c.FuelType != null && c.FuelType.ToLower() == value);
            }

            if (minPrice.HasValue)
            {
                filteredCars = filteredCars.Where(c => c.PricePerDay >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                filteredCars = filteredCars.Where(c => c.PricePerDay <= maxPrice.Value);
            }

            if (seats.HasValue)
            {
                filteredCars = filteredCars.Where(c => c.Seats == seats.Value);
            }

            var viewModel = new CarsFilterViewModel
            {
                Cars = filteredCars
                    .OrderBy(c => c.Brand)
                    .ThenBy(c => c.Model)
                    .ToList(),

                Brand = brand,
                BodyType = bodyType,
                TransmissionType = transmissionType,
                FuelType = fuelType,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Seats = seats,

                AvailableBrands = cars
                    .Where(c => !string.IsNullOrWhiteSpace(c.Brand))
                    .Select(c => c.Brand!)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList(),

                AvailableBodyTypes = cars
                    .Where(c => !string.IsNullOrWhiteSpace(c.BodyType))
                    .Select(c => c.BodyType!)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList(),

                AvailableTransmissionTypes = cars
                    .Where(c => !string.IsNullOrWhiteSpace(c.TransmissionType))
                    .Select(c => c.TransmissionType!)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList(),

                AvailableFuelTypes = cars
                    .Where(c => !string.IsNullOrWhiteSpace(c.FuelType))
                    .Select(c => c.FuelType!)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList(),

                AvailableSeats = cars
                    .Select(c => c.Seats)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            return car == null ? NotFound() : View(car);
        }

        [Authorize]
        public async Task<IActionResult> Rent(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var car = await _carService.GetCarByIdAsync(id);
            if (car == null || !car.IsAvailable)
                return NotFound();

            return View(new RentCarViewModel
            {
                CarId = car.Id,
                CarName = car.FullName,
                CarImage = car.ImageUrl,
                PricePerDay = car.PricePerDay,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1)
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rent(RentCarViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var car = await _carService.GetCarByIdAsync(model.CarId);
            if (car == null || !car.IsAvailable)
                return NotFound();

            model.CarName = car.FullName;
            model.CarImage = car.ImageUrl;
            model.PricePerDay = car.PricePerDay;

            if (model.StartDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(model.StartDate), "Началната дата не може да бъде в миналото.");
            }

            if (model.EndDate.Date < model.StartDate.Date)
            {
                ModelState.AddModelError(nameof(model.EndDate), "Крайната дата не може да бъде преди началната дата.");
            }

            if (!ModelState.IsValid)
                return View(model);

            var isAvailable = await _carService.IsCarAvailableAsync(model.CarId, model.StartDate.Date, model.EndDate.Date);
            if (!isAvailable)
            {
                ModelState.AddModelError("", "Автомобилът не е свободен за избрания период.");
                return View(model);
            }

            return RedirectToAction("Create", "Rentals", new
            {
                carId = model.CarId,
                startDate = model.StartDate.ToString("yyyy-MM-dd"),
                endDate = model.EndDate.ToString("yyyy-MM-dd")
            });
        }
    }
}