using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moda.Backend.Domain.Models;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Domain.Data
{
    public class ModaDbContext : IdentityDbContext<Account>, IDbContext
    {

        public ModaDbContext()
        {
        }
        public ModaDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderCoupon> OrderCoupons { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<StaticFile> StaticFiles { get; set; }
        public DbSet<StockHistory> StockHistories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        private static void SeedEnumTable<TEntity, TEnum>(ModelBuilder modelBuilder)
        where TEntity : class
        where TEnum : System.Enum
        {
            var enumType = typeof(TEnum);
            var entityType = typeof(TEntity);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("TEnum must be an enum type.");
            }

            var enumValues = System.Enum.GetValues(enumType).Cast<TEnum>();

            foreach (var enumValue in enumValues)
            {
                var entityInstance = Activator.CreateInstance(entityType);
                entityType.GetProperty("Id")!.SetValue(entityInstance, enumValue);
                entityType.GetProperty("Name")!.SetValue(entityInstance, enumValue.ToString());
                modelBuilder.Entity<TEntity>().HasData(entityInstance!);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration config = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json", true, true)
                           .Build();
            string cs = config["ConnectionStrings:DB"];
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(cs);
            }
            //optionsBuilder.UseSqlServer(
            //   "server=.;database=TravelCapstone;uid=sa;pwd=12345;TrustServerCertificate=True;MultipleActiveResultSets=True;");
        }

    }
}
