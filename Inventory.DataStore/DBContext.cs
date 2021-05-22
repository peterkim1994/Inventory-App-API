using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Daos;
using InventoryPOS.DataStore.Daos;
using Microsoft.EntityFrameworkCore;

namespace InventoryPOS.DataStore
{
    public class DBContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<SaleInvoice> SalesInvoices { get; set; }
        public DbSet<ProductSale> ProductSales { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<ProductPromotion> ProductPromotions { get; set; }
        public DbSet<Store> Store { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
               .Property(b => b.Active)
               .HasDefaultValue(true);

            modelBuilder.Entity<Product>()
             .HasIndex(p => new { p.Barcode, p.StoreId }).IsUnique();

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

            modelBuilder.Entity<Colour>()
                 .HasIndex(p => new { p.Value, p.StoreId }).IsUnique();
            modelBuilder.Entity<Brand>()
                .HasIndex(p => new { p.Value, p.StoreId }).IsUnique();
            modelBuilder.Entity<ItemCategory>()  
                .HasIndex(p => new { p.Value, p.StoreId }).IsUnique();
            modelBuilder.Entity<Size>()
                .HasIndex(p => new { p.Value, p.StoreId }).IsUnique();

            modelBuilder.Entity<Promotion>()
              .Property(b => b.Active)
              .HasDefaultValue(true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server = (localdb)\mssqllocaldb;Database=InventoryPOS-V2;Integrated Security= True");
        }
    }
}
