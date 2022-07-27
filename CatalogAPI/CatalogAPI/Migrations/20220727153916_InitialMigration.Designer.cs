﻿// <auto-generated />
using System;
using CatalogAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CatalogAPI.Migrations
{
    [DbContext(typeof(CatalogContext))]
    [Migration("20220727153916_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CatalogAPI.Models.Catalog", b =>
                {
                    b.Property<long>("CatalogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Catalog_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CatalogId"), 1L, 1);

                    b.Property<string>("CatalogName")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Catalog_Name");

                    b.HasKey("CatalogId");

                    b.ToTable("Catalog");
                });

            modelBuilder.Entity("CatalogAPI.Models.Product", b =>
                {
                    b.Property<long>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Product_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ProductId"), 1L, 1);

                    b.Property<long>("CatalogId")
                        .HasColumnType("bigint")
                        .HasColumnName("Catalog_Id_FK");

                    b.Property<long>("Price")
                        .HasColumnType("bigint")
                        .HasColumnName("Price");

                    b.Property<int>("ProductType")
                        .HasColumnType("int")
                        .HasColumnName("Product_Type");

                    b.HasKey("ProductId");

                    b.HasIndex("CatalogId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("CatalogAPI.Models.Product", b =>
                {
                    b.HasOne("CatalogAPI.Models.Catalog", "Catalog")
                        .WithMany()
                        .HasForeignKey("CatalogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("CatalogAPI.Models.ProductDescription", "ProductDescription", b1 =>
                        {
                            b1.Property<long>("ProductId")
                                .HasColumnType("bigint");

                            b1.Property<DateTime>("DOE")
                                .HasColumnType("datetime2")
                                .HasColumnName("DOE");

                            b1.Property<string>("Summary")
                                .HasColumnType("varchar(200)")
                                .HasColumnName("Summary");

                            b1.Property<string>("UOM")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("UOM");

                            b1.HasKey("ProductId");

                            b1.ToTable("Product");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.Navigation("Catalog");

                    b.Navigation("ProductDescription");
                });
#pragma warning restore 612, 618
        }
    }
}
