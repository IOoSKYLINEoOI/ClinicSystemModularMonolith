using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Domain.Configuration;

public class ProfileConfiguration : IEntityTypeConfiguration<ProfileEntity>
{
    public void Configure(EntityTypeBuilder<ProfileEntity> builder)
    {
        builder.HasKey(u => u.UserId);
        
        builder.HasOne(u => u.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<ProfileEntity>(u => u.UserId)
            .IsRequired();
        
        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(u => u.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.MiddleName)
            .HasMaxLength(100);
        
        builder.Property(u => u.DateOfBirth)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(u => u.Gender)
            .HasConversion<int>();

    }
}