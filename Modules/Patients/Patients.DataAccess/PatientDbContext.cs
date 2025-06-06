using Microsoft.EntityFrameworkCore;
using Patients.DataAccess.Configuration;
using Patients.DataAccess.Entities;

namespace Patients.DataAccess;

public class PatientDbContext(DbContextOptions<PatientDbContext> options) : DbContext(options)
{
    public DbSet<ContactEntity> Contacts { get; set; }
    public DbSet<InsuranceEntity> Insurances { get; set; }
    public DbSet<MedicalRecordEntity> MedicalRecords { get; set; }
    public DbSet<PatientEntity> Patients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ContactConfiguration());
        modelBuilder.ApplyConfiguration(new InsuranceConfiguration());
        modelBuilder.ApplyConfiguration(new MedicalRecordConfiguration());
        modelBuilder.ApplyConfiguration(new PatientConfiguration());
    }
}