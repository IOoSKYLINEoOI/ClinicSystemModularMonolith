using Employees.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employees.DataAccess.Configuration;

public class EmployeeLicenseConfiguration : IEntityTypeConfiguration<EmployeeLicenseEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeLicenseEntity> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.HasOne(e => e.Employee)
            .WithMany(e => e.EmployeeLicenses)
            .HasForeignKey(e => e.EmployeeId)
            .IsRequired();
        
        builder.Property(e => e.LicenseNumber)
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