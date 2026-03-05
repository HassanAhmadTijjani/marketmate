using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopTracker.Models;
using ShopTracker.ViewModels;

namespace ShopTracker.Controllers
{
    public class AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;


        // GET: AccountController
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create new User from the model
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email
                };
                // Create User with HashPassword
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Sign in user
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["success"] = "Registration Successful! Welcome";
                    return RedirectToAction("Index", "Home");
                }
                // If failed add errors to modelstate
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // GET: Login
        [HttpGet]
        public ActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Login
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // Find user by email
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Sign in using USERNAME
                    var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        TempData["success"] = "Login Successful";
                        return !string.IsNullOrEmpty(returnUrl) ? Redirect(returnUrl) : RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }
            return View(model);
        }
        
        // GET: Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["success"] = "Logout Successful";
            return RedirectToAction("Index", "Home");
        }
    }
}
