using BulletinBoard.Domain.Comments.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BulletinBoard.DataAccess.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");
        
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Bulletin)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.BulletinId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(x => x.Author)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.AuthorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .Property(x => x.Text)
            .HasMaxLength(1500)
            .IsRequired();
    }
}