using Employees.Core.Models;
using Employees.DataAccess.Configuration;
using Employees.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employees.DataAccess;

public class EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : DbContext(options)
{
    public DbSet<EmployeeAssignmentEntity> EmployeeAssignments { get; set; }
    public DbSet<EmployeeCertificateEntity> EmployeeCertificates { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }
    public DbSet<EmployeeLicenseEntity> EmployeeLicenses { get; set; }
    public DbSet<PositionEntity> Positions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmployeeAssignmentConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeCertificateConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeLicenseConfiguration());
        modelBuilder.ApplyConfiguration(new PositionConfiguration());
    }
}