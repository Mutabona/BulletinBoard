using BulletinBoard.Domain.Files.Images.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BulletinBoard.DataAccess.Configurations;

/// <summary>
/// Конфигурация изображения.
/// </summary>
public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder
            .ToTable("Images");
        
        builder
            .HasKey(x => x.Id);
    }
}