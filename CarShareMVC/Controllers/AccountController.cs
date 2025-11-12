using Microsoft.AspNetCore.Mvc;
using CarShareBLL.Services;
using CarShareBLL.DTOs;

namespace CarShareMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.RegisterAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.LoginAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", result.User.Id);
            HttpContext.Session.SetString("UserRole", result.User.Role);
            HttpContext.Session.SetString("UserName", result.User.FullName);

            TempData["SuccessMessage"] = $"Welcome, {result.User.FullName}!";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return result.User.Role switch
            {
                "Admin" => RedirectToAction("Index", "Admin"),
                "CarOwner" => RedirectToAction("Dashboard", "CarOwner"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                await _authService.LogoutAsync(userId.Value);
            }

            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Logged out successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
}