﻿// <auto-generated />
using System;
using InventoryPOS.DataStore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InventoryPOS.DataStore.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20210517044112_added-primary-key-for-ProductSales")]
    partial class addedprimarykeyforProductSales
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InventoryPOS.DataStore.Models.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.Colour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("Colours");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.ItemCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("ItemCategories");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.Payment", b =>
                {
                    b.Property<int>("SaleInvoiceId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentMethodId")
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.HasKey("SaleInvoiceId", "PaymentMethodId");

                    b.HasIndex("PaymentMethodId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.PaymentMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Method")
                        .IsUnique();

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<long>("Barcode")
                        .HasColumnType("bigint");

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<int>("ColourId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ItemCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("ManufactureCode")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Price")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("Qty")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("SizeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Barcode")
                        .IsUnique();

                    b.HasIndex("BrandId");

                    b.HasIndex("ColourId");

                    b.HasIndex("ItemCategoryId");

                    b.HasIndex("SizeId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.ProductPromotion", b =>
                {
                    b.Property<int>("PromotionId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("PromotionId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductPromotions");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.ProductSale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PriceSold")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("SaleInvoiceId")
                        .HasColumnType("int");

                    b.Property<int>("SalesInvoiceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SaleInvoiceId");

                    b.ToTable("ProductSales");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.Promotion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime2");

                    b.Property<string>("PromotionName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PromotionPrice")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PromotionName")
                        .IsUnique();

                    b.ToTable("Promotions");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.SaleInvoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Finalised")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("InvoiceDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("SalesInvoices");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.Size", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.Payment", b =>
                {
                    b.HasOne("InventoryPOS.DataStore.Models.PaymentMethod", "PaymentMethod")
                        .WithMany("Payments")
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventoryPOS.DataStore.Models.SaleInvoice", "SaleInvoice")
                        .WithMany("Payments")
                        .HasForeignKey("SaleInvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PaymentMethod");

                    b.Navigation("SaleInvoice");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.Product", b =>
                {
                    b.HasOne("InventoryPOS.DataStore.Models.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventoryPOS.DataStore.Models.Colour", "Colour")
                        .WithMany("Products")
                        .HasForeignKey("ColourId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventoryPOS.DataStore.Models.ItemCategory", "ItemCategory")
                        .WithMany("Products")
                        .HasForeignKey("ItemCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventoryPOS.DataStore.Models.Size", "Size")
                        .WithMany("Products")
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("Colour");

                    b.Navigation("ItemCategory");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.ProductPromotion", b =>
                {
                    b.HasOne("InventoryPOS.DataStore.Models.Product", "Product")
                        .WithMany("Promotions")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventoryPOS.DataStore.Models.Promotion", "Promotion")
                        .WithMany("ProductPromotions")
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Promotion");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.ProductSale", b =>
                {
                    b.HasOne("InventoryPOS.DataStore.Models.Product", "Product")
                        .WithMany("ProductSales")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventoryPOS.DataStore.Models.SaleInvoice", "SaleInvoice")
                        .WithMany("ProductSales")
                        .HasForeignKey("SaleInvoiceId");

                    b.Navigation("Product");

                    b.Navigation("SaleInvoice");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.Brand", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.Colour", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.ItemCategory", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.PaymentMethod", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.Product", b =>
                {
                    b.Navigation("ProductSales");

                    b.Navigation("Promotions");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.Promotion", b =>
                {
                    b.Navigation("ProductPromotions");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.SaleInvoice", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("ProductSales");
                });

            modelBuilder.Entity("InventoryPOS.DataStore.Models.Size", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}