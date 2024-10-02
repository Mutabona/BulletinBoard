﻿// <auto-generated />
using System;
using BulletinBoard.DbMigrator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BulletinBoard.DbMigrator.Migrations
{
    [DbContext(typeof(MigrationDbContext))]
    [Migration("20241001132123_Add_comments")]
    partial class Add_comments
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BulletinBoard.Domain.Bulletins.Entity.Bulletin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(1500)
                        .HasColumnType("character varying(1500)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Bulletins", (string)null);
                });

            modelBuilder.Entity("BulletinBoard.Domain.Categories.Entity.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid?>("ParentCategoryId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("BulletinBoard.Domain.Comments.Entity.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("BulletinId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("character varying(1500)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BulletinId");

                    b.ToTable("Comments", (string)null);
                });

            modelBuilder.Entity("BulletinBoard.Domain.Files.Images.Entity.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BulletinId")
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Length")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BulletinId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("BulletinBoard.Domain.Users.Entity.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("BulletinBoard.Domain.Bulletins.Entity.Bulletin", b =>
                {
                    b.HasOne("BulletinBoard.Domain.Categories.Entity.Category", "Category")
                        .WithMany("Bulletins")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("BulletinBoard.Domain.Users.Entity.User", "Owner")
                        .WithMany("Bulletins")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("BulletinBoard.Domain.Categories.Entity.Category", b =>
                {
                    b.HasOne("BulletinBoard.Domain.Categories.Entity.Category", "ParentCategory")
                        .WithMany("Subcategories")
                        .HasForeignKey("ParentCategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("BulletinBoard.Domain.Comments.Entity.Comment", b =>
                {
                    b.HasOne("BulletinBoard.Domain.Users.Entity.User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BulletinBoard.Domain.Bulletins.Entity.Bulletin", "Bulletin")
                        .WithMany("Comments")
                        .HasForeignKey("BulletinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Bulletin");
                });

            modelBuilder.Entity("BulletinBoard.Domain.Files.Images.Entity.Image", b =>
                {
                    b.HasOne("BulletinBoard.Domain.Bulletins.Entity.Bulletin", "Bulletin")
                        .WithMany("Images")
                        .HasForeignKey("BulletinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bulletin");
                });

            modelBuilder.Entity("BulletinBoard.Domain.Bulletins.Entity.Bulletin", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("BulletinBoard.Domain.Categories.Entity.Category", b =>
                {
                    b.Navigation("Bulletins");

                    b.Navigation("Subcategories");
                });

            modelBuilder.Entity("BulletinBoard.Domain.Users.Entity.User", b =>
                {
                    b.Navigation("Bulletins");

                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
