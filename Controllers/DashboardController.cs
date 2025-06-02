using Microsoft.AspNetCore.Mvc;
using PaymentPortal.Models;
using PaymentPortal.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentPortal.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult CustomerDashboard(bool paymentSuccess = false)
        {
            ViewBag.PaymentSuccess = paymentSuccess;
            return View();
        }

        public async Task<IActionResult> EmployeeDashboard()
        {
            var payments = await _context.PaymentRequests
                .OrderByDescending(p => p.DateCreated)
                .ToListAsync();

            return View(payments);
        }

        [HttpPost]
        public async Task<IActionResult> Verify(int id)
        {
            var payment = await _context.PaymentRequests.FindAsync(id);
            if (payment != null)
            {
                payment.IsVerified = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("EmployeeDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitToSwift(int id)
        {
            var payment = await _context.PaymentRequests.FindAsync(id);
            if (payment != null && payment.IsVerified && !payment.IsSubmittedToSwift)
            {
                payment.IsSubmittedToSwift = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("EmployeeDashboard");
        }
    }
}
