using System.ComponentModel.DataAnnotations;

namespace PaymentPortal.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "ID Number must be 13 digits.")]
        public string IDNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{10,20}$", ErrorMessage = "Account Number must be 10 to 20 digits.")]
        public string AccountNumber { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{4,20}$", ErrorMessage = "Username must be 4-20 letters or numbers.")]
        public string Username { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        [RegularExpression(@"^(Customer|Employee)$", ErrorMessage = "Role must be Customer or Employee.")]
        public string Role { get; set; } = "Customer";
    }
}
