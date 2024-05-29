using Microsoft.AspNetCore.Identity;
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
        public DbSet<OptionPackage> OptionPackages { get; set; }    
        public DbSet<OptionPackageHistory> OptionPackageHistories { get; set; } 
        public DbSet<Configuration> Configurations { get; set; }    
        public DbSet<Affiliate> Affiliates { get; set; }    
        public DbSet<ShopPackage> ShopPackages { get; set; }    
        public DbSet<PaymentResponse> PaymentResponses { get; set; }        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "6a32e12a-60b5-4d93-8306-82231e1232d7",
                Name = "ADMIN",
                ConcurrencyStamp = "6a32e12a-60b5-4d93-8306-82231e1232d7",
                NormalizedName = "admin"
            });
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "85b6791c-49d8-4a61-ad0b-8274ec27e764",
                Name = "STAFF",
                ConcurrencyStamp = "85b6791c-49d8-4a61-ad0b-8274ec27e764",
                NormalizedName = "staff"
            });
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "814f9270-78f5-4503-b7d3-0c567e5812ba",
                Name = "SHOP",
                ConcurrencyStamp = "814f9270-78f5-4503-b7d3-0c567e5812ba",
                NormalizedName = "shop"
            });
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "02962efa-1273-46c0-b103-7167b1742ef3",
                Name = "CUSTOMER",
                ConcurrencyStamp = "02962efa-1273-46c0-b103-7167b1742ef3",
                NormalizedName = "customer"
            });
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
            //   "server=.;database=Moda;uid=sa;pwd=12345;TrustServerCertificate=True;MultipleActiveResultSets=True;");
        }
    }
}
