using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WarehouseAPI.Models;

namespace WarehouseAPI.Data
{
    public class WarehouseDbContext : DbContext
    {
        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options){ }

        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var hasher = new PasswordHasher<User>();

            var admin = new User { Id = 1, Username = "admin", Role = "admin" };
            admin.PasswordHash = hasher.HashPassword(admin, "admin123"); 

            var staff = new User { Id = 2, Username = "stationery", Role = "stationery" };
            staff.PasswordHash = hasher.HashPassword(staff, "pass123"); 

            modelBuilder.Entity<User>().HasData(admin, staff);
        }
    }
}