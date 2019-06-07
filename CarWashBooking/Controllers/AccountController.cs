using System.Linq;
using System.Threading.Tasks;
using CarWashBooking.Constants;
using CarWashBooking.Models;
using CarWashBooking.Models.DataModels;
using CarWashBooking.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarWashBooking.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register() => View();

        //
        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
           return  RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded )
                {
                  
                    var user = _userManager.Users.Where(w => w.Email == model.Email ).FirstOrDefault();
        
                    Request.HttpContext.Session.SetString(SessionConstants.SessionActiceUserID, user.Id.ToString ());
                    Request.HttpContext.Session.SetString(SessionConstants.SessionActiceUser, user.UserName.ToString());

                    return RedirectToAction("Index", "Booking");
                }
                else
                {

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Fejl ved login");
            }
            return View(model);
        }

        //[HttpPost]
        public async Task<IActionResult> Logout()

        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

    }

}
