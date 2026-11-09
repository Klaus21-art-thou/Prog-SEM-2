using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;
using System.Text.Json.Serialization;

namespace RaceDay.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly RaceDayApiClient _api;

        public AccountController(RaceDayApiClient api)
        {
            _api = api;
        }

        private bool IsLoggedIn => HttpContext.Session.GetInt32(SessionKeys.UserId) != null;

        private IActionResult RedirectToLanding()
        {
            return HttpContext.Session.GetString(SessionKeys.UserRole) == RoleNames.Organiser
                ? RedirectToAction("Dashboard", "Organiser")
                : RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (IsLoggedIn) return RedirectToLanding();
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Matches API RegisterDto property expectations exactly
            var body = new
            {
                username = model.FullName,
                userEmail = model.Email,
                password = model.Password,
                userRole = model.Role,
                userBio = model.Gender
            };

            var result = await _api.PostAsync<UserProfileDto>("api/users/register", body);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Registration failed.");
                return View(model);
            }

            TempData["Success"] = "Account created successfully. Please log in.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (IsLoggedIn) return RedirectToLanding();
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Matches API LoginDto property expectations exactly
            var body = new
            {
                userEmail = model.Email,
                password = model.Password
            };

            var result = await _api.PostAsync<ApiLoginResponseModel>("api/users/login", body);

            if (!result.Success || result.Data == null)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Invalid login credentials.");
                return View(model);
            }

            HttpContext.Session.SetInt32(SessionKeys.UserId, result.Data.UserID);
            HttpContext.Session.SetString(SessionKeys.UserName, result.Data.Username);
            HttpContext.Session.SetString(SessionKeys.UserRole, result.Data.UserRole);

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToLanding();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Info"] = "You have been logged out.";
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();
    }

    public class ApiLoginResponseModel
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("userID")]
        public int UserID { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("userRole")]
        public string UserRole { get; set; } = string.Empty;
    }

    public class UserProfileDto
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public string? UserBio { get; set; }
    }
}