using Employees.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employees.DataAccess.Configuration;

public class EmployeeCertificateConfiguration : IEntityTypeConfiguration<EmployeeCertificateEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeCertificateEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Employee)
            .WithMany(x => x.EmployeeCertificates)
            .HasForeignKey(x => x.EmployeeId)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.IssuedBy)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.IssuedAt)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.ValidUntil)
            .HasColumnType("date");
    }
}