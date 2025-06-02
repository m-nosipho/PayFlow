using Microsoft.AspNetCore.Mvc;
using PaymentPortal.Models;
using PaymentPortal.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PaymentPortal.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
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
        public async Task<IActionResult> Register(string fullName, string idNumber, string accountNumber, string username, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }

            var success = await _authService.Register(fullName, idNumber, accountNumber, username, password, "Customer");


            if (!success)
            {
                ViewBag.Error = "Username, ID number or Account number already exists.";
                return View();
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string identifier, string password)
        {
            var user = await _authService.Login(identifier, password);

            if (user == null)
            {
                ViewBag.Error = "Invalid login credentials.";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);

            if (user.Role == "Employee")
                return RedirectToAction("EmployeeDashboard", "Dashboard");

            else
                return RedirectToAction("CustomerDashboard", "Dashboard");

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
