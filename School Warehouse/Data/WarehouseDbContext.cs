using Microsoft.EntityFrameworkCore;
using WarehouseAPI.Models; // Make sure this matches your project's namespace

namespace WarehouseAPI.Data
{
    public class WarehouseDbContext : DbContext
    {
        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
    }
}