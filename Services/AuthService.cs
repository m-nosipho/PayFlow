using System.Security.Cryptography;
using System.Text;
using PaymentPortal.Models;
using PaymentPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace PaymentPortal.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Register(string fullName, string idNumber, string accountNumber, string username, string password, string role)
        {
            // Check uniqueness across all identifiers
            bool userExists = await _context.Users.AnyAsync(u =>
                u.Username == username || u.IDNumber == idNumber || u.AccountNumber == accountNumber);

            if (userExists)
                return false;

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                FullName = fullName,
                IDNumber = idNumber,
                AccountNumber = accountNumber,
                Username = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> Login(string identifier, string password)
        {
            // Try matching identifier to username, ID number, or account number
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Username == identifier || u.IDNumber == identifier || u.AccountNumber == identifier);

            if (user == null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
}
