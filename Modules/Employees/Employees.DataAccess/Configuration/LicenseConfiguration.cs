using Employees.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employees.DataAccess.Configuration;

public class LicenseConfiguration : IEntityTypeConfiguration<LicenseEntity>
{
    public void Configure(EntityTypeBuilder<LicenseEntity> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.HasOne(e => e.Employee)
            .WithMany(e => e.EmployeeLicenses)
            .HasForeignKey(e => e.EmployeeId)
            .IsRequired();
        
        builder.Property(e => e.Number)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(e => e.IssuedBy)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(e => e.IssuedAt)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(e => e.ValidUntil)
            .HasColumnType("date");
    }
}