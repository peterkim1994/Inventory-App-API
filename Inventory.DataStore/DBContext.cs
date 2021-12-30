using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Daos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryPOS.DataStore
{
    public class DBContext : IdentityDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<SaleInvoice> SaleInvoices { get; set; }
        public DbSet<ProductSale> ProductSales { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<ProductPromotion> ProductPromotions { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<Refund> Refunds { get; set; }
       // public DbSet<ProductToRestock> ProductsToRestock { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
               .Property(b => b.Active)
               .HasDefaultValue(true);

            modelBuilder.Entity<Product>()
              .Property(b => b.Qty)
              .HasDefaultValue(0);

            modelBuilder.Entity<Product>()
              .Property(b => b.Price)
              .HasDefaultValue(0);

            modelBuilder.Entity<Payment>()
                .HasKey(p => new { p.SaleInvoiceId, p.PaymentMethodId });

            //modelBuilder.Entity<ProductSale>()
            //    .HasKey(ps => new { ps.SalesInvoiceId, ps.ProductId });

            modelBuilder.Entity<ProductPromotion>()
               .HasKey(pd => new { pd.PromotionId, pd.ProductId });

            modelBuilder.Entity<SaleInvoice>()
                .Property(s => s.Finalised)
                .HasDefaultValue(false);

            modelBuilder.Entity<SaleInvoice>()
                .Property(s => s.Canceled)
                .HasDefaultValue(false);

            modelBuilder.Entity<Promotion>()
              .Property(b => b.Active)
              .HasDefaultValue(true);

            modelBuilder.Entity<ProductSale>()
                .Property(s => s.Canceled)
                .HasDefaultValue(false);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server = (localdb)\mssqllocaldb;Database=InventoryPOS;Integrated Security= True");

        }
    }
}
