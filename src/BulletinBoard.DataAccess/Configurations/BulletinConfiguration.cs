using BulletinBoard.Domain.Bulletins.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BulletinBoard.DataAccess.Configurations;

public class BulletinConfiguration : IEntityTypeConfiguration<Bulletin>
{
    public void Configure(EntityTypeBuilder<Bulletin> builder)
    {
        builder
            .ToTable("Bulletins");
        
        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(50);
        
        builder
            .HasOne(x => x.Owner)
            .WithMany(c => c.Bulletins)
            .HasForeignKey(x => x.OwnerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(c => c.Images)
            .WithOne(x => x.Bulletin)
            .HasForeignKey(x => x.BulletinId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}