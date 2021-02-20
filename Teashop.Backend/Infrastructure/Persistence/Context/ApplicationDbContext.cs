using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Teashop.Backend.Domain.Cart.Entities;
using Teashop.Backend.Domain.Order.Entities;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<CartEntity> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PaymentCard> PaymentCards { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("ProductNumbers", schema: "dbo")
                .StartsAt(100000)
                .IncrementsBy(1);
            modelBuilder.HasSequence<int>("OrderNumbers", schema: "dbo")
                .StartsAt(100000)
                .IncrementsBy(1);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
