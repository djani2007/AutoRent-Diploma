using AutoRent.Core.Entities;
using AutoRent.Core.Interfaces;
using AutoRent.Infrastructure.Data;
using AutoRent.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AutoRent.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarService _carService;
        private readonly AppDbContext _context;

        public HomeController(ICarService carService, AppDbContext context)
        {
            _carService = carService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _carService.GetAvailableCarsAsync();
            return View(cars.Take(6));
        }

        public IActionResult About() => View();

        [HttpGet]
        public IActionResult Contact()
        {
            var model = new ContactViewModel();

            if (User.Identity?.IsAuthenticated == true)
            {
                model.Name = User.Identity?.Name ?? string.Empty;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (User.IsInRole("Admin"))
            {
                TempData["Error"] = "Администраторът не може да изпраща съобщение от контактната форма.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var message = new ContactMessage
            {
                Name = model.Name,
                Email = model.Email,
                Subject = model.Subject,
                Message = model.Message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Съобщението беше изпратено успешно.";
            return RedirectToAction(nameof(Contact));
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View();
    }
}