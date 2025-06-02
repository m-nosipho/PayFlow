using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentPortal.Models
{
    public class PaymentRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string PayeeName { get; set; }

        [Required]
        public string PayeeAccount { get; set; }

        [Required]
        public string SWIFTCode { get; set; }

        [Required]
        public string BankName { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public string SubmittedBy { get; set; }

        public bool IsVerified { get; set; } = false;

        public bool IsSubmittedToSwift { get; set; } = false;
    }
}
