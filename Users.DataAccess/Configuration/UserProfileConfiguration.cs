using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Domain.Configuration;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfileEntity>
{
    public void Configure(EntityTypeBuilder<UserProfileEntity> builder)
    {
        builder.HasKey(u => u.UserId);
        
        builder.HasOne(u => u.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<UserProfileEntity>(u => u.UserId)
            .IsRequired();
        
        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(u => u.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.FatherName)
            .HasMaxLength(100);
        
        builder.Property(u => u.DateOfBirth)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(u => u.Gender)
            .HasConversion<int>();

    }
}