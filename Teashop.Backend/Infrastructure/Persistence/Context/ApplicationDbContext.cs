using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Teashop.Backend.Domain.Cart.Entities;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CartEntity> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
