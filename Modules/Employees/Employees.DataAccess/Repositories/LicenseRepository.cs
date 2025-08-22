using CSharpFunctionalExtensions;
using Employees.Core.Interfaces.Repositories;
using Employees.Core.Models;
using Employees.Core.Models.Filter;
using Employees.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employees.DataAccess.Repositories;

public class LicenseRepository : ILicenseRepository
{
    private readonly EmployeeDbContext _context;

    public LicenseRepository(EmployeeDbContext context)
    {
        _context = context;
    }
    
        public async Task Add(EmployeeLicense license)
    {
        var licenseEntity = new LicenseEntity()
        {
            Id = license.Id,
            EmployeeId = license.EmployeeId,
            Number = license.LicenseNumber,
            IssuedBy = license.IssuedBy,
            IssuedAt = license.IssuedAt,
            ValidUntil = license.ValidUntil
        };
        
        await _context.AddAsync(licenseEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<Result> Update(EmployeeLicense license)
    {
        var licenseEntity = await _context.EmployeeLicenses
            .FirstOrDefaultAsync(u => u.Id == license.Id);
        
        if (licenseEntity is null)
            return Result.Failure<EmployeeLicense>("License not found or deleted.");
        
        licenseEntity.Number = license.LicenseNumber;
        licenseEntity.IssuedBy = license.IssuedBy;
        licenseEntity.IssuedAt = license.IssuedAt;
        licenseEntity.ValidUntil = license.ValidUntil;
        
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result<EmployeeLicense>> GetById(Guid id)
    {
        var employeeLicenseEntity = await _context.EmployeeLicenses
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (employeeLicenseEntity is null)
            return Result.Failure<EmployeeLicense>("License not found or deleted.");
        
        var employeeLicenseResult =
            EmployeeLicense.Create(
                employeeLicenseEntity.Id, 
                employeeLicenseEntity.EmployeeId,
                employeeLicenseEntity.Number,
                employeeLicenseEntity.IssuedBy,
                employeeLicenseEntity.IssuedAt,
                employeeLicenseEntity.ValidUntil);
        
        if (employeeLicenseResult.IsFailure)
            return Result.Failure<EmployeeLicense>(employeeLicenseResult.Error);

        return employeeLicenseResult;
    }

    public async Task<Result<List<EmployeeLicense>>> GetByEmployeeId(Guid employeeId)
    {
        var employeeLicensesEntities = await _context.EmployeeLicenses
            .AsNoTracking()
            .Where(e => e.EmployeeId == employeeId)
            .ToListAsync();
            
        if (employeeLicensesEntities.Count == 0)
            return Result.Failure<List<EmployeeLicense>>("License not found or deleted.");
        
        var employeeLicenses = new List<EmployeeLicense>();

        foreach (var employeeLicenseEntity in employeeLicensesEntities)
        {
            var employeeLicenseResult = EmployeeLicense.Create(
                employeeLicenseEntity.Id,
                employeeLicenseEntity.EmployeeId,
                employeeLicenseEntity.Number,
                employeeLicenseEntity.IssuedBy,
                employeeLicenseEntity.IssuedAt,
                employeeLicenseEntity.ValidUntil);
            
            if (employeeLicenseResult.IsFailure)
                return Result.Failure<List<EmployeeLicense>>(employeeLicenseResult.Error);
            
            employeeLicenses.Add(employeeLicenseResult.Value);
        }
        
        return employeeLicenses;
    }

    public async Task<Result<List<EmployeeLicense>>> GetValidLicenses(Guid employeeId, DateOnly asOfDate)
    {
        var employeeLicensesEntities = await _context.EmployeeLicenses
            .AsNoTracking()
            .Where(e => e.EmployeeId == employeeId && e.ValidUntil >= asOfDate)
            .ToListAsync();
        
        if (employeeLicensesEntities.Count == 0)
            return Result.Failure<List<EmployeeLicense>>("License not found or deleted.");
        
        var employeeLicenses = new List<EmployeeLicense>();

        foreach (var employeeLicenseEntity in employeeLicensesEntities)
        {
            var employeeLicenseResult = EmployeeLicense.Create(
                employeeLicenseEntity.Id,
                employeeLicenseEntity.EmployeeId,
                employeeLicenseEntity.Number,
                employeeLicenseEntity.IssuedBy,
                employeeLicenseEntity.IssuedAt,
                employeeLicenseEntity.ValidUntil);
            
            if (employeeLicenseResult.IsFailure)
                return Result.Failure<List<EmployeeLicense>>(employeeLicenseResult.Error);
            
            employeeLicenses.Add(employeeLicenseResult.Value);
        }
        
        return employeeLicenses;
    }

    public async Task<Result<List<EmployeeLicense>>> Filter(EmployeeLicenseFilter filter)
    {
        var query = _context.EmployeeLicenses
            .AsNoTracking()
            .AsQueryable();

        if (filter.EmployeeId.HasValue)
            query = query.Where(e => e.EmployeeId == filter.EmployeeId.Value);

        if (!string.IsNullOrWhiteSpace(filter.LicenseNumber))
            query = query.Where(e => EF.Functions.ILike(e.Number, $"%{filter.LicenseNumber}%"));

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
                entity.Number,
                entity.IssuedBy,
                entity.IssuedAt,
                entity.ValidUntil);

            if (!model.IsSuccess)
                return Result.Failure<List<EmployeeLicense>>(model.Error);

            result.Add(model.Value);
        }

        return result;
    }
}