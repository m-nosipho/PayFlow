using PaymentPortal.Data;
using PaymentPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace PaymentPortal.Services
{
    public class DataSeeder
    {
        public static async Task SeedUsersAsync(ApplicationDbContext context)
        {
            if (!await context.Users.AnyAsync(u => u.Username == "admin"))
            {
                CreatePasswordHash("Admin123!", out byte[] hash, out byte[] salt);
                context.Users.Add(new User
                {
                    FullName = "System Admin",
                    IDNumber = "EMP0001",
                    AccountNumber = "EMPACC01",
                    Username = "admin",
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    Role = "Employee"
                });
            }

            if (!await context.Users.AnyAsync(u => u.Username == "client"))
            {
                CreatePasswordHash("Client123!", out byte[] hash, out byte[] salt);
                context.Users.Add(new User
                {
                    FullName = "Client One",
                    IDNumber = "CLI0001",
                    AccountNumber = "CLIACC01",
                    Username = "client",
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    Role = "Customer"
                });
            }

            await context.SaveChangesAsync();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
