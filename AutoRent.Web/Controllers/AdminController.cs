using AutoRent.Core.DTOs;
using AutoRent.Core.Entities;
using AutoRent.Core.Interfaces;
using AutoRent.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;
using System.Globalization;
using AutoRent.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using AutoRent.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoRent.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICarService _carService;
        private readonly IRentalService _rentalService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public AdminController(ICarService carService, IRentalService rentalService, UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _carService = carService;
            _rentalService = rentalService;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _carService.GetAllCarsAsync();
            var users = await _userManager.Users.ToListAsync();
            var rentals = await _rentalService.GetAllRentalsAsync();

            var unreadMessagesCount = await _context.ContactMessages.CountAsync(m => !m.IsRead);
            var recentMessages = await _context.ContactMessages
                .OrderByDescending(m => m.CreatedAt)
                .Take(5)
                .ToListAsync();

            var viewModel = new AdminDashboardViewModel
            {
                TotalCars = cars.Count(),
                TotalUsers = users.Count,
                TotalRentals = rentals.Count(),
                ActiveRentals = rentals.Count(r => r.Status == "Active" || r.Status == "Pending"),
                UnreadMessagesCount = unreadMessagesCount,
                RecentRentals = rentals.Take(5).ToList(),
                RecentMessages = recentMessages
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Cars()
        {
            var cars = await _carService.GetAllCarsAsync();
            return View(cars);
        }

        public IActionResult CreateCar()
        {
            return View(new CarCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCarManual()
        {
            var brand = Request.Form["Brand"].ToString();
            var carModel = Request.Form["Model"].ToString();
            var bodyType = Request.Form["BodyType"].ToString();
            var transmissionType = Request.Form["TransmissionType"].ToString();
            var imageUrl = Request.Form["ImageUrl"].ToString();
            var description = Request.Form["Description"].ToString();
            var fuelType = Request.Form["FuelType"].ToString();

            var yearRaw = Request.Form["Year"].ToString();
            var seatsRaw = Request.Form["Seats"].ToString();
            var priceRaw = Request.Form["PricePerDay"].ToString();

            var hasPaidVignette = Request.Form["HasPaidVignette"] == "true";
            var hasUnlimitedMileage = Request.Form["HasUnlimitedMileage"] == "true";
            var vatIncluded = Request.Form["VatIncluded"] == "true";
            var isAvailable = Request.Form["IsAvailable"] == "true";

            if (string.IsNullOrWhiteSpace(brand) ||
                string.IsNullOrWhiteSpace(carModel) ||
                string.IsNullOrWhiteSpace(bodyType) ||
                string.IsNullOrWhiteSpace(transmissionType) ||
                string.IsNullOrWhiteSpace(priceRaw))
            {
                TempData["Error"] = "Моля, попълни задължителните полета.";
                return View(new CarCreateViewModel
                {
                    Brand = brand,
                    Model = carModel,
                    BodyType = bodyType,
                    TransmissionType = transmissionType,
                    ImageUrl = imageUrl,
                    Description = description,
                    FuelType = fuelType,
                    HasPaidVignette = hasPaidVignette,
                    HasUnlimitedMileage = hasUnlimitedMileage,
                    VatIncluded = vatIncluded,
                    IsAvailable = isAvailable
                });
            }

            if (!int.TryParse(yearRaw, out var year))
                year = DateTime.Now.Year;

            if (!int.TryParse(seatsRaw, out var seats))
                seats = 5;

            if (!decimal.TryParse(priceRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
            {
                if (!decimal.TryParse(priceRaw, NumberStyles.Any, CultureInfo.CurrentCulture, out price))
                {
                    TempData["Error"] = "Невалидна цена.";
                    return View(new CarCreateViewModel
                    {
                        Brand = brand,
                        Model = carModel,
                        BodyType = bodyType,
                        TransmissionType = transmissionType,
                        ImageUrl = imageUrl,
                        Description = description,
                        Year = year,
                        FuelType = fuelType,
                        Seats = seats,
                        HasPaidVignette = hasPaidVignette,
                        HasUnlimitedMileage = hasUnlimitedMileage,
                        VatIncluded = vatIncluded,
                        IsAvailable = isAvailable
                    });
                }
            }

            var carDto = new CarCreateDto
            {
                Brand = brand,
                Model = carModel,
                BodyType = bodyType,
                TransmissionType = transmissionType,
                ImageUrl = imageUrl,
                PricePerDay = price,
                Description = description,
                Year = year,
                FuelType = fuelType,
                Seats = seats,
                HasPaidVignette = hasPaidVignette,
                HasUnlimitedMileage = hasUnlimitedMileage,
                VatIncluded = vatIncluded
            };

            await _carService.CreateCarAsync(carDto);

            TempData["Success"] = "Car created successfully!";
            return RedirectToAction(nameof(Cars));
        }
      

        public async Task<IActionResult> EditCar(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
                return NotFound();

            var model = new CarEditViewModel
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                BodyType = car.BodyType,
                TransmissionType = car.TransmissionType,
                ImageUrl = car.ImageUrl,
                PricePerDay = car.PricePerDay,
                IsAvailable = car.IsAvailable,
                Description = car.Description,
                Year = car.Year,
                FuelType = car.FuelType,
                Seats = car.Seats,
                HasPaidVignette = car.HasPaidVignette,
                HasUnlimitedMileage = car.HasUnlimitedMileage,
                VatIncluded = car.VatIncluded
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCarManual()
        {
            var idRaw = Request.Form["Id"].ToString();
            var brand = Request.Form["Brand"].ToString();
            var carModel = Request.Form["Model"].ToString();
            var bodyType = Request.Form["BodyType"].ToString();
            var transmissionType = Request.Form["TransmissionType"].ToString();
            var imageUrl = Request.Form["ImageUrl"].ToString();
            var description = Request.Form["Description"].ToString();
            var fuelType = Request.Form["FuelType"].ToString();

            var yearRaw = Request.Form["Year"].ToString();
            var seatsRaw = Request.Form["Seats"].ToString();
            var priceRaw = Request.Form["PricePerDay"].ToString();

            var hasPaidVignette = Request.Form["HasPaidVignette"].ToString().Contains("true");
            var hasUnlimitedMileage = Request.Form["HasUnlimitedMileage"].ToString().Contains("true");
            var vatIncluded = Request.Form["VatIncluded"].ToString().Contains("true");
            var isAvailable = Request.Form["IsAvailable"].ToString().Contains("true");

            if (!int.TryParse(idRaw, out var id))
                return NotFound();

            if (string.IsNullOrWhiteSpace(brand) ||
                string.IsNullOrWhiteSpace(carModel) ||
                string.IsNullOrWhiteSpace(bodyType) ||
                string.IsNullOrWhiteSpace(transmissionType) ||
                string.IsNullOrWhiteSpace(priceRaw))
            {
                TempData["Error"] = "Моля, попълни задължителните полета.";

                return View("EditCar", new CarEditViewModel
                {
                    Id = id,
                    Brand = brand,
                    Model = carModel,
                    BodyType = bodyType,
                    TransmissionType = transmissionType,
                    ImageUrl = imageUrl,
                    Description = description,
                    FuelType = fuelType,
                    HasPaidVignette = hasPaidVignette,
                    HasUnlimitedMileage = hasUnlimitedMileage,
                    VatIncluded = vatIncluded,
                    IsAvailable = isAvailable
                });
            }

            if (!int.TryParse(yearRaw, out var year))
                year = DateTime.Now.Year;

            if (!int.TryParse(seatsRaw, out var seats))
                seats = 5;

            if (!decimal.TryParse(priceRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
            {
                if (!decimal.TryParse(priceRaw, NumberStyles.Any, CultureInfo.CurrentCulture, out price))
                {
                    TempData["Error"] = "Невалидна цена.";

                    return View("EditCar", new CarEditViewModel
                    {
                        Id = id,
                        Brand = brand,
                        Model = carModel,
                        BodyType = bodyType,
                        TransmissionType = transmissionType,
                        ImageUrl = imageUrl,
                        Description = description,
                        Year = year,
                        FuelType = fuelType,
                        Seats = seats,
                        HasPaidVignette = hasPaidVignette,
                        HasUnlimitedMileage = hasUnlimitedMileage,
                        VatIncluded = vatIncluded,
                        IsAvailable = isAvailable
                    });
                }
            }

            var dto = new CarUpdateDto
            {
                Brand = brand,
                Model = carModel,
                BodyType = bodyType,
                TransmissionType = transmissionType,
                ImageUrl = imageUrl,
                PricePerDay = price,
                IsAvailable = isAvailable,
                Description = description,
                Year = year,
                FuelType = fuelType,
                Seats = seats,
                HasPaidVignette = hasPaidVignette,
                HasUnlimitedMileage = hasUnlimitedMileage,
                VatIncluded = vatIncluded
            };

            await _carService.UpdateCarAsync(id, dto);

            TempData["Success"] = "Car updated successfully!";
            return RedirectToAction(nameof(Cars));
        }






        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var hasRentals = await _context.Rentals.AnyAsync(r => r.CarId == id);

            if (hasRentals)
            {
                var car = await _context.Cars.FindAsync(id);

                if (car == null)
                {
                    TempData["Error"] = "Автомобилът не беше намерен.";
                    return RedirectToAction(nameof(Cars));
                }

                car.IsAvailable = false;
                await _context.SaveChangesAsync();

                TempData["Success"] = "Автомобилът има направени наеми и не може да бъде изтрит. Маркиран е като недостъпен.";
                return RedirectToAction(nameof(Cars));
            }

            await _carService.DeleteCarAsync(id);

            TempData["Success"] = "Автомобилът беше изтрит успешно!";
            return RedirectToAction(nameof(Cars));
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.OrderByDescending(u => u.CreatedAt).ToListAsync();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.Role = roles.FirstOrDefault() ?? "Client";
            }
            return View(users);
        }

        public async Task<IActionResult> UserDetails(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var user = await _userManager.Users
                .Include(u => u.Rentals)
                .ThenInclude(r => r.Car)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Client";

            var rentals = user.Rentals
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            var viewModel = new AdminUserDetailsViewModel
            {
                User = user,
                Role = role,
                TotalRentals = rentals.Count,
                ActiveRentals = rentals.Count(r => r.Status == "Active" || r.Status == "Pending"),
                TotalSpent = rentals.Sum(r => r.TotalPrice),
                Rentals = rentals
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
                TempData["Success"] = "User blocked successfully!";
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnblockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
                TempData["Success"] = "User unblocked successfully!";
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                TempData["Success"] = "User deleted successfully!";
            }
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> Rentals()
        {
            var rentals = await _rentalService.GetAllRentalsAsync();
            return View(rentals);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRentalStatus(int id, string status)
        {
            await _rentalService.UpdateRentalStatusAsync(id, status);
            TempData["Success"] = "Rental status updated!";
            return RedirectToAction(nameof(Rentals));
        }

        public async Task<IActionResult> Messages()
        {
            var messages = await _context.ContactMessages
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkMessageAsRead(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            message.IsRead = true;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Съобщението е маркирано като прочетено.";
            return RedirectToAction(nameof(Messages));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.ContactMessages.Remove(message);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Съобщението беше изтрито.";
            return RedirectToAction(nameof(Messages));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveProfileChange(int id)
        {
            var request = await _context.ContactMessages.FindAsync(id);

            if (request == null)
                return NotFound();

            if (request.RequestType != "ProfileChange" || request.RequestStatus != "Pending")
            {
                TempData["Error"] = "Тази заявка не може да бъде одобрена.";
                return RedirectToAction(nameof(Messages));
            }

            var user = await _userManager.FindByIdAsync(request.UserId ?? string.Empty);

            if (user == null)
            {
                TempData["Error"] = "Потребителят не беше намерен.";
                return RedirectToAction(nameof(Messages));
            }

            user.PhoneNumber = request.NewPhoneNumber;
            user.Address = request.NewAddress ?? string.Empty;
            user.City = request.NewCity ?? string.Empty;
            user.IdentityCardNumber = request.NewIdentityCardNumber ?? string.Empty;
            user.DriverLicenseNumber = request.NewDriverLicenseNumber ?? string.Empty;

            if (request.NewDriverLicenseIssueDate.HasValue)
            {
                user.DriverLicenseIssueDate = request.NewDriverLicenseIssueDate.Value;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Възникна грешка при одобряване на заявката.";
                return RedirectToAction(nameof(Messages));
            }

            request.RequestStatus = "Approved";
            request.IsRead = true;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Заявката беше одобрена и профилът беше обновен.";
            return RedirectToAction(nameof(Messages));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectProfileChange(int id)
        {
            var request = await _context.ContactMessages.FindAsync(id);

            if (request == null)
                return NotFound();

            if (request.RequestType != "ProfileChange" || request.RequestStatus != "Pending")
            {
                TempData["Error"] = "Тази заявка не може да бъде отказана.";
                return RedirectToAction(nameof(Messages));
            }

            request.RequestStatus = "Rejected";
            request.IsRead = true;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Заявката беше отказана.";
            return RedirectToAction(nameof(Messages));
        }

        [HttpGet]
        public async Task<IActionResult> ChangeUserPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                TempData["Error"] = "Потребителят не беше намерен.";
                return RedirectToAction(nameof(Users));
            }

            var model = new AdminChangePasswordViewModel
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserPassword(AdminChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                TempData["Error"] = "Потребителят не беше намерен.";
                return RedirectToAction(nameof(Users));
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                model.Email = user.Email ?? string.Empty;
                model.FullName = user.FullName;
                return View(model);
            }

            TempData["Success"] = "Паролата на потребителя беше сменена успешно.";
            return RedirectToAction(nameof(Users));
        }

    }
}
