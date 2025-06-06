using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patients.DataAccess.Entities;

namespace Patients.DataAccess.Configuration;

public class ContactConfiguration: IEntityTypeConfiguration<ContactEntity>
{
    public void Configure(EntityTypeBuilder<ContactEntity> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.ContactName)
            .HasMaxLength(60)
            .IsRequired();
        
        builder.Property(u => u.Relationship)
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(u => u.Phone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(60);
    }
}