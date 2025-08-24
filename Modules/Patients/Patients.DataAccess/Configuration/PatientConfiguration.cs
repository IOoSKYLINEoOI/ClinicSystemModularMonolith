using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patients.DataAccess.Entities;

namespace Patients.DataAccess.Configuration;

public class PatientConfiguration: IEntityTypeConfiguration<PatientEntity>
{
    public void Configure(EntityTypeBuilder<PatientEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.UserId)
            .IsRequired();
        
        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .IsRequired();
        
        
        builder.OwnsOne(u => u.BloodProfileEntity, bp =>
        {
            bp.Property(b => b.Type).IsRequired();
            bp.Property(b => b.Rh).IsRequired();
            bp.Property(b => b.Kell).IsRequired();
        });

        builder.HasMany(u => u.Contacts)
            .WithOne(c => c.Patient)
            .HasForeignKey(c => c.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Insurance)
            .WithOne(i => i.Patient)
            .HasForeignKey<InsuranceEntity>(i => i.PatientId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(u => u.Questionnaire)
            .WithOne(i => i.Patient)
            .HasForeignKey<QuestionnaireEntity>(i => i.PatientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}