using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Domain.Configuration;

public class UserConfiguration: IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.IsActive).IsRequired();

        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(60);
        });

        builder.OwnsOne(u => u.PhoneNumber, phone =>
        {
            phone.Property(p => p.Value)
                .HasColumnName("PhoneNumber")
                .IsRequired()
                .HasMaxLength(20);

            phone.Property(p => p.IsValid)
                .HasColumnName("PhoneNumberIsValid")
                .IsRequired();
        
            phone.Property(p => p.IsActual)
                .HasColumnName("PhoneNumberIsActual")
                .IsRequired();

            phone.Property(p => p.IsComfirmed)
                .HasColumnName("PhoneNumberIsComfirmed")
                .IsRequired();
        });

        builder.HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfileEntity>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}