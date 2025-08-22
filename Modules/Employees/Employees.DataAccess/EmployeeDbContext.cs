using Employees.DataAccess.Configuration;
using Employees.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employees.DataAccess;

public class EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : DbContext(options)
{
    public DbSet<AssignmentEntity> EmployeeAssignments { get; set; }
    public DbSet<CertificateEntity> EmployeeCertificates { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }
    public DbSet<LicenseEntity> EmployeeLicenses { get; set; }
    public DbSet<PositionEntity> Positions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AssignmentConfiguration());
        modelBuilder.ApplyConfiguration(new CertificateConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new LicenseConfiguration());
        modelBuilder.ApplyConfiguration(new PositionConfiguration());
    }
}