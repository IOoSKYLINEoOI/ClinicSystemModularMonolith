using Employees.Core.Interfaces.Repositories;
using Employees.Core.Models;
using Employees.Core.Models.Filter;
using Employees.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employees.DataAccess.Repositories;

public class EmployeeLicenseRepository : IEmployeeLicenseRepository
{
    private readonly EmployeeDbContext _context;

    public EmployeeLicenseRepository(EmployeeDbContext context)
    {
        _context = context;
    }
    
        public async Task Add(EmployeeLicense license)
    {
        var licenseEntity = new EmployeeLicenseEntity()
        {
            Id = license.Id,
            EmployeeId = license.EmployeeId,
            LicenseNumber = license.LicenseNumber,
            IssuedBy = license.IssuedBy,
            IssuedAt = license.IssuedAt,
            ValidUntil = license.ValidUntil
        };
        
        await _context.AddAsync(licenseEntity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(EmployeeLicense license)
    {
        var licenseEntity = await _context.EmployeeLicenses
            .FirstOrDefaultAsync(u => u.Id == license.Id)
            ?? throw new InvalidOperationException("License not found or deleted.");
        
        licenseEntity.LicenseNumber = license.LicenseNumber;
        licenseEntity.IssuedBy = license.IssuedBy;
        licenseEntity.IssuedAt = license.IssuedAt;
        licenseEntity.ValidUntil = license.ValidUntil;
        
        await _context.SaveChangesAsync();
    }

    public async Task<EmployeeLicense> GetById(Guid id)
    {
        var employeeLicenseEntity = await _context.EmployeeLicenses
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new InvalidOperationException("License not found or deleted.");

        var employeeLicense =
            EmployeeLicense.Create(
                employeeLicenseEntity.Id, 
                employeeLicenseEntity.EmployeeId,
                employeeLicenseEntity.LicenseNumber,
                employeeLicenseEntity.IssuedBy,
                employeeLicenseEntity.IssuedAt,
                employeeLicenseEntity.ValidUntil);
        
        if(!employeeLicense.IsSuccess)
            throw new InvalidOperationException(employeeLicense.Error);

        return employeeLicense.Value;
    }

    public async Task<List<EmployeeLicense>> GetByEmployeeId(Guid employeeId)
    {
        var employeeLicensesEntities = await _context.EmployeeLicenses
            .AsNoTracking()
            .Where(e => e.EmployeeId == employeeId)
            .ToListAsync()
            ?? throw new InvalidOperationException("License not found or deleted.");
        
        var employeeLicenses = new List<EmployeeLicense>();

        foreach (var employeeLicenseEntity in employeeLicensesEntities)
        {
            var employeeLicense = EmployeeLicense.Create(
                employeeLicenseEntity.Id,
                employeeLicenseEntity.EmployeeId,
                employeeLicenseEntity.LicenseNumber,
                employeeLicenseEntity.IssuedBy,
                employeeLicenseEntity.IssuedAt,
                employeeLicenseEntity.ValidUntil);
            
            if (!employeeLicense.IsSuccess)
                throw new InvalidOperationException(employeeLicense.Error);
            
            employeeLicenses.Add(employeeLicense.Value);
        }
        
        return employeeLicenses;
    }

    public async Task<List<EmployeeLicense>> GetValidLicenses(Guid employeeId, DateOnly asOfDate)
    {
        var employeeLicensesEntities = await _context.EmployeeLicenses
                                               .AsNoTracking()
                                               .Where(e => e.EmployeeId == employeeId && e.ValidUntil >= asOfDate)
                                               .ToListAsync()
                                           ?? throw new InvalidOperationException("Licenses not found or deleted.");
        
        var employeeLicenses = new List<EmployeeLicense>();

        foreach (var employeeLicenseEntity in employeeLicensesEntities)
        {
            var employeeLicense = EmployeeLicense.Create(
                employeeLicenseEntity.Id,
                employeeLicenseEntity.EmployeeId,
                employeeLicenseEntity.LicenseNumber,
                employeeLicenseEntity.IssuedBy,
                employeeLicenseEntity.IssuedAt,
                employeeLicenseEntity.ValidUntil);
            
            if (!employeeLicense.IsSuccess)
                throw new InvalidOperationException(employeeLicense.Error);
            
            employeeLicenses.Add(employeeLicense.Value);
        }
        
        return employeeLicenses;
    }

    public async Task<List<EmployeeLicense>> Filter(EmployeeLicenseFilter filter)
    {
        var query = _context.EmployeeLicenses
            .AsNoTracking()
            .AsQueryable();

        if (filter.EmployeeId.HasValue)
            query = query.Where(e => e.EmployeeId == filter.EmployeeId.Value);

        if (!string.IsNullOrWhiteSpace(filter.LicenseNumber))
            query = query.Where(e => EF.Functions.ILike(e.LicenseNumber, $"%{filter.LicenseNumber}%"));

        if (!string.IsNullOrWhiteSpace(filter.IssuedBy))
            query = query.Where(e => EF.Functions.ILike(e.IssuedBy, $"%{filter.IssuedBy}%"));

        if (filter.IssuedFrom.HasValue)
            query = query.Where(e => e.IssuedAt >= filter.IssuedFrom.Value);

        if (filter.IssuedTo.HasValue)
            query = query.Where(e => e.IssuedAt <= filter.IssuedTo.Value);

        if (filter.ValidUntilFrom.HasValue)
            query = query.Where(e => e.ValidUntil >= filter.ValidUntilFrom.Value);

        if (filter.ValidUntilTo.HasValue)
            query = query.Where(e => e.ValidUntil <= filter.ValidUntilTo.Value);

        var licensesEntities = await query.ToListAsync();

        var result = new List<EmployeeLicense>();

        foreach (var entity in licensesEntities)
        {
            var model = EmployeeLicense.Create(
                entity.Id,
                entity.EmployeeId,
                entity.LicenseNumber,
                entity.IssuedBy,
                entity.IssuedAt,
                entity.ValidUntil);

            if (!model.IsSuccess)
                throw new InvalidOperationException(model.Error);

            result.Add(model.Value);
        }

        return result;
    }
}