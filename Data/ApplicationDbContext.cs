using LTWeb_CodeFirst.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LTWeb_CodeFirst.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarType> CarsType { get; set; }
        public DbSet<CarTypeDetail> CarTypeDetails { get; set; }
        public DbSet<Company> Companies{ get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Warranty> Warranties { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<FavoriteList> FavoriteLists { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
