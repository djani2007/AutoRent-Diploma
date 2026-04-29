using AutoRent.Core.Entities;
using AutoRent.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoRent.Infrastructure.Data;

namespace AutoRent.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Невалиден имейл или парола.");
                return View(model);
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "Вашият акаунт е блокиран.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Невалиден имейл или парола.");
                return View(model);
            }

            await SetSessionAsync(user);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existing = await _userManager.FindByEmailAsync(model.Email);
            if (existing != null)
            {
                ModelState.AddModelError("Email", "Вече съществува потребител с този email.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                City = model.City,
                DriverLicenseNumber = model.DriverLicenseNumber,
                EGN = model.EGN,
                BirthDate = model.BirthDate,
                IdentityCardNumber = model.IdentityCardNumber,
                DriverLicenseIssueDate = model.DriverLicenseIssueDate,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            await _userManager.AddToRoleAsync(user, "Client");

            await _signInManager.SignInAsync(user, false);
            await SetSessionAsync(user);

            TempData["Success"] = "Регистрацията е успешна. Добре дошли в AutoRent.";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            var viewModel = new ProfileViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Address = user.Address,
                City = user.City,
                DriverLicenseNumber = user.DriverLicenseNumber,
                EGN = user.EGN,
                BirthDate = user.BirthDate,
                IdentityCardNumber = user.IdentityCardNumber,
                DriverLicenseIssueDate = user.DriverLicenseIssueDate,
                CreatedAt = user.CreatedAt
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            model.Id = user.Id;
            model.Email = user.Email ?? string.Empty;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.EGN = user.EGN;
            model.BirthDate = user.BirthDate;
            model.CreatedAt = user.CreatedAt;

            if (!ModelState.IsValid)
                return View(model);

            var hasChanges =
                user.PhoneNumber != model.PhoneNumber ||
                user.Address != model.Address ||
                user.City != model.City ||
                user.IdentityCardNumber != model.IdentityCardNumber ||
                user.DriverLicenseNumber != model.DriverLicenseNumber ||
                user.DriverLicenseIssueDate != model.DriverLicenseIssueDate;

            if (!hasChanges)
            {
                TempData["Success"] = "Няма направени промени.";
                return RedirectToAction("Profile");
            }

            var messageText =
                $"Потребителят {user.FirstName} {user.LastName} поиска промяна на профилни данни.\n\n" +
                $"Телефон: {user.PhoneNumber} → {model.PhoneNumber}\n" +
                $"Адрес: {user.Address} → {model.Address}\n" +
                $"Град: {user.City} → {model.City}\n" +
                $"Лична карта: {user.IdentityCardNumber} → {model.IdentityCardNumber}\n" +
                $"Шофьорска книжка: {user.DriverLicenseNumber} → {model.DriverLicenseNumber}\n" +
                $"Дата на издаване на книжка: {user.DriverLicenseIssueDate:dd.MM.yyyy} → {model.DriverLicenseIssueDate:dd.MM.yyyy}";

            var request = new ContactMessage
            {
                Name = user.FullName,
                Email = user.Email ?? string.Empty,
                Subject = "Заявка за промяна на профил",
                Message = messageText,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,

                RequestType = "ProfileChange",
                RequestStatus = "Pending",
                UserId = user.Id,

                NewPhoneNumber = model.PhoneNumber,
                NewAddress = model.Address,
                NewCity = model.City,
                NewIdentityCardNumber = model.IdentityCardNumber,
                NewDriverLicenseNumber = model.DriverLicenseNumber,
                NewDriverLicenseIssueDate = model.DriverLicenseIssueDate
            };

            _context.ContactMessages.Add(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Заявката за промяна беше изпратена към администратор за одобрение.";
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToAction("Login");

            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToAction("Login");

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login");

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword
            );

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);

            TempData["Success"] = "Паролата беше сменена успешно.";
            return RedirectToAction("Profile");
        }


        private async Task SetSessionAsync(ApplicationUser user)
        {
            HttpContext.Session.SetString("UserId", user.Id);
            HttpContext.Session.SetString("UserEmail", user.Email ?? string.Empty);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserRole", await _userManager.IsInRoleAsync(user, "Admin") ? "Admin" : "Client");
        }
    }
}