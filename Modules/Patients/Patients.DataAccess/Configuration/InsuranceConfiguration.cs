using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patients.DataAccess.Entities;

namespace Patients.DataAccess.Configuration;

public class InsuranceConfiguration : IEntityTypeConfiguration<InsuranceEntity>
{
    public void Configure(EntityTypeBuilder<InsuranceEntity> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.InsuranceCompany)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.PolicyNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.ValidFrom)
            .IsRequired();

        builder.Property(i => i.ValidTo)
            .IsRequired();

        builder.Property(i => i.PatientId)
            .IsRequired();
    }
}