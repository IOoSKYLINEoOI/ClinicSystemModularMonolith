using Employees.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employees.DataAccess.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.UserId).IsUnique();
        
        builder.Property(x => x.HireDate)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.FireDate)
            .HasColumnType("date");
        
        builder.Property(x => x.IsActive)
            .IsRequired();
    }
}