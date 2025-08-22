using Employees.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employees.DataAccess.Configuration;

public class AssignmentConfiguration : IEntityTypeConfiguration<AssignmentEntity>
{
    public void Configure(EntityTypeBuilder<AssignmentEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Employee)
            .WithMany(x => x.EmployeeAssignments)
            .HasForeignKey(x => x.EmployeeId)
            .IsRequired();

        builder.HasOne(x => x.Position)
            .WithMany(x => x.Assignments)
            .HasForeignKey(x => x.PositionId)
            .IsRequired();

        builder.Property(x => x.AssignedAt)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.RemovedAt)
            .HasColumnType("date");
    }
}