using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appointments.DataAccess.Entities;

namespace Appointments.DataAccess.Configuration;

public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecordEntity>
{
    public void Configure(EntityTypeBuilder<MedicalRecordEntity> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Diagnosis)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(m => m.DiagnosisCode)
            .HasMaxLength(50);

        builder.Property(m => m.Notes)
            .HasMaxLength(2000);

        builder.Property(m => m.RecordDate)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .IsRequired();
        
        builder.Property(m => m.EmployeeId)
            .IsRequired();
    }
}