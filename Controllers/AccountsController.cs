using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TweetishApp.Data;
using TweetishApp.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TweetishApp.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountsController(
            ILogger<AccountsController> logger,
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager
        )
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] UserCreationVM vm)
        {
            if (!ModelState.IsValid) {
                TempData["error"] = "Invalid input for creation";
                _logger.LogInformation("Invalid input for creation");

                return Redirect("/accounts/register");
            }

            if (vm.Password != vm.ConfirmPassword) {
                TempData["error"] = "Password dont match";
                _logger.LogInformation("Passwords input dont match");

                return Redirect("/accounts/register");
            }

            AppUser user = new AppUser {
                Nickname = vm.Nickname,
                UserName = vm.Nickname
            };

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded) {
                TempData["error"] = "An error ocurred, try again";
                _logger.LogError("Error while registering an user");

                return Redirect("/accounts/register");
            }

            return Redirect("/accounts/login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] UserCreationVM vm)
        {
            if (!ModelState.IsValid) {
                TempData["error"] = "Invalid input for creation";
                _logger.LogInformation("Invalid input for creation");

                return Redirect("/accounts/login");
            }

            AppUser user = await _userManager.FindByNameAsync(vm.Nickname);

            if (user == null) {
                TempData["error"] = "Nickname or password invalid";
                _logger.LogInformation("Nickname not found");

                return Redirect("/accounts/login");
            }

            bool match = await _signInManager.UserManager.CheckPasswordAsync(user, vm.Password);

            if (!match) {
                TempData["error"] = "Nickname or password invalid";
                _logger.LogInformation("Password no match");

                return Redirect("/accounts/login");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Redirect("/users/dashboard");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/");
        }
    }
}