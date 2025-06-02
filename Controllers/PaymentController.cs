using Microsoft.AspNetCore.Mvc;
using PaymentPortal.Models;
using PaymentPortal.Data;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PaymentPortal.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Customer submits payment
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(PaymentRequest payment)
        {
            // Server-side validation
            if (!Regex.IsMatch(payment.PayeeName ?? "", @"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$"))
                ModelState.AddModelError("PayeeName", "Only letters and spaces allowed.");

            if (!Regex.IsMatch(payment.PayeeAccount ?? "", @"^\d{16}$"))
                ModelState.AddModelError("PayeeAccount", "Account Number must be exactly 16 digits.");

            if (!Regex.IsMatch(payment.SWIFTCode ?? "", @"^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$"))
                ModelState.AddModelError("SWIFTCode", "Invalid SWIFT Code format.");

            if (!ModelState.IsValid)
            {
                return View("~/Views/Dashboard/CustomerDashboard.cshtml", payment);
            }

            // Add metadata
            payment.SubmittedBy = HttpContext.Session.GetString("Username");
            payment.DateCreated = DateTime.UtcNow;

            // Save to DB
            _context.PaymentRequests.Add(payment);
            await _context.SaveChangesAsync();

            TempData["PaymentSuccess"] = true;

            return RedirectToAction("CustomerDashboard", "Dashboard");
        }

        // Employee clicks "Verify"
        [HttpPost]
        public async Task<IActionResult> Verify(int id)
        {
            var payment = await _context.PaymentRequests.FindAsync(id);
            if (payment != null)
            {
                payment.IsVerified = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("EmployeeDashboard", "Dashboard");
        }

        // Employee clicks "Submit to SWIFT"
        [HttpPost]
        public async Task<IActionResult> SubmitToSwift(int id)
        {
            var payment = await _context.PaymentRequests.FindAsync(id);
            if (payment != null && payment.IsVerified && !payment.IsSubmittedToSwift)
            {
                payment.IsSubmittedToSwift = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("EmployeeDashboard", "Dashboard");
        }
    }
}
