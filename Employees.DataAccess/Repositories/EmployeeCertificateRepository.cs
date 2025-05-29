using CSharpFunctionalExtensions;
using Employees.Core.Interfaces.Repositories;
using Employees.Core.Models;
using Employees.Core.Models.Filter;
using Employees.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employees.DataAccess.Repositories;

public class EmployeeCertificateRepository : IEmployeeCertificateRepository
{
    private readonly EmployeeDbContext _context;

    public EmployeeCertificateRepository(EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task Add(EmployeeCertificate certificate)
    {
        var certificateEntity = new EmployeeCertificateEntity()
        {
            Id = certificate.Id,
            EmployeeId = certificate.EmployeeId,
            Name = certificate.Name,
            IssuedBy = certificate.IssuedBy,
            IssuedAt = certificate.IssuedAt,
            ValidUntil = certificate.ValidUntil
        };
        
        await _context.AddAsync(certificateEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<Result> Update(EmployeeCertificate certificate)
    {
        var certificateEntity = await _context.EmployeeCertificates
            .FirstOrDefaultAsync(u => u.Id == certificate.Id);
        
        if (certificateEntity is null)
            return Result.Failure<EmployeeCertificate>("Certificate not found or deleted");
        
        certificateEntity.Name = certificate.Name;
        certificateEntity.IssuedBy = certificate.IssuedBy;
        certificateEntity.IssuedAt = certificate.IssuedAt;
        certificateEntity.ValidUntil = certificate.ValidUntil;
        
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result<EmployeeCertificate>> GetById(Guid id)
    {
        var employeeCertificateEntity = await _context.EmployeeCertificates
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (employeeCertificateEntity is null)
            return Result.Failure<EmployeeCertificate>("Certificate not found or deleted");

        var employeeCertificateResult =
            EmployeeCertificate.Create(
                employeeCertificateEntity.Id, 
                employeeCertificateEntity.EmployeeId,
                employeeCertificateEntity.Name,
                employeeCertificateEntity.IssuedBy,
                employeeCertificateEntity.IssuedAt,
                employeeCertificateEntity.ValidUntil);
        
        if (employeeCertificateResult.IsFailure)
            return Result.Failure<EmployeeCertificate>(employeeCertificateResult.Error);

        return employeeCertificateResult;
    }

    public async Task<List<EmployeeCertificate>> GetByEmployeeId(Guid employeeId)
    {
        var employeeCertificatesEntities = await _context.EmployeeCertificates
            .AsNoTracking()
            .Where(e => e.EmployeeId == employeeId)
            .ToListAsync()
            ?? throw new InvalidOperationException("Certificates not found or deleted.");
        
        var employeeCertificates = new List<EmployeeCertificate>();

        foreach (var employeeCertificateEntity in employeeCertificatesEntities)
        {
            var employeeCertificate = EmployeeCertificate.Create(
                employeeCertificateEntity.Id,
                employeeCertificateEntity.EmployeeId,
                employeeCertificateEntity.Name,
                employeeCertificateEntity.IssuedBy,
                employeeCertificateEntity.IssuedAt,
                employeeCertificateEntity.ValidUntil);
            
            if (!employeeCertificate.IsSuccess)
                throw new InvalidOperationException(employeeCertificate.Error);
            
            employeeCertificates.Add(employeeCertificate.Value);
        }
        
        return employeeCertificates;
    }

    public async Task<List<EmployeeCertificate>> GetValidCertificates(Guid employeeId, DateOnly asOfDate)
    {
        var employeeCertificatesEntities = await _context.EmployeeCertificates
                                               .AsNoTracking()
                                               .Where(e => e.EmployeeId == employeeId && e.ValidUntil >= asOfDate)
                                               .ToListAsync()
                                           ?? throw new InvalidOperationException("Certificates not found or deleted.");
        
        var employeeCertificates = new List<EmployeeCertificate>();

        foreach (var employeeCertificateEntity in employeeCertificatesEntities)
        {
            var employeeCertificate = EmployeeCertificate.Create(
                employeeCertificateEntity.Id,
                employeeCertificateEntity.EmployeeId,
                employeeCertificateEntity.Name,
                employeeCertificateEntity.IssuedBy,
                employeeCertificateEntity.IssuedAt,
                employeeCertificateEntity.ValidUntil);
            
            if (!employeeCertificate.IsSuccess)
                throw new InvalidOperationException(employeeCertificate.Error);
            
            employeeCertificates.Add(employeeCertificate.Value);
        }
        
        return employeeCertificates;
    }

    public async Task<List<EmployeeCertificate>> Filter(EmployeeCertificateFilter filter)
    {
        var query = _context.EmployeeCertificates
            .AsNoTracking()
            .AsQueryable();

        if (filter.EmployeeId.HasValue)
            query = query.Where(e => e.EmployeeId == filter.EmployeeId.Value);

        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(e => EF.Functions.ILike(e.Name, $"%{filter.Name}%"));

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

        var certificateEntities = await query.ToListAsync();

        var result = new List<EmployeeCertificate>();

        foreach (var entity in certificateEntities)
        {
            var model = EmployeeCertificate.Create(
                entity.Id,
                entity.EmployeeId,
                entity.Name,
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