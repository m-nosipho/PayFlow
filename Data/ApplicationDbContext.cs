using Microsoft.EntityFrameworkCore;
using PaymentPortal.Models;

namespace PaymentPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<PaymentRequest> PaymentRequests { get; set; }


    }
}
