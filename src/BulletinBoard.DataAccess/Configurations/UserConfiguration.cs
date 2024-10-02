using BulletinBoard.Domain.Users.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BulletinBoard.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable("Users");
        
        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Password)
            .IsRequired()
            .HasMaxLength(50);
        
        builder
            .Property(x => x.Email)
            .HasMaxLength(50)
            .IsRequired();
        
        builder
            .Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}