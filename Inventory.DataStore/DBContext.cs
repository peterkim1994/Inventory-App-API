using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Models;
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


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server = (localdb)\mssqllocaldb;Database=InventoryPOS;Integrated Security= True");
        }
    }
}
