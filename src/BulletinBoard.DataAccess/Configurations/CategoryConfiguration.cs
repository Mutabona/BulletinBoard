﻿using BulletinBoard.Domain.Categories.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BulletinBoard.DataAccess.Configurations;

/// <summary>
/// Конфигурация категории.
/// </summary>
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .ToTable("Categories");
        
        builder
            .HasKey(c => c.Id);
        
        builder
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .HasMany(c => c.Bulletins)
            .WithOne(c => c.Category)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasMany(c => c.Subcategories)
            .WithOne(c => c.ParentCategory)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}