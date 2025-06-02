using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace PaymentPortal.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Portal()
        {
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(username) || role != "Employee")
                return RedirectToAction("Login", "Auth");

            return RedirectToAction("EmployeeDashboard", "Payment");
        }
    }
}
