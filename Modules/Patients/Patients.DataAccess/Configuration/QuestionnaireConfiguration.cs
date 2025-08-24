using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patients.DataAccess.Entities;

namespace Patients.DataAccess.Configuration;

public class QuestionnaireConfiguration : IEntityTypeConfiguration<QuestionnaireEntity>
{
    public void Configure(EntityTypeBuilder<QuestionnaireEntity> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Allergies)
            .HasMaxLength(255);

        builder.Property(u => u.CurrentMedications)
            .HasMaxLength(255);

        builder.Property(u => u.HeightCm)
            .HasPrecision(5, 2); 

        builder.Property(u => u.WeightKg)
            .HasPrecision(5, 2); 

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .IsRequired();
        
        builder.Property(i => i.PatientId)
            .IsRequired();
    }
}