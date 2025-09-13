using Microsoft.EntityFrameworkCore;
using Patients.Core.Interfaces.Repository;
using Patients.Core.Models;
using Patients.DataAccess.Entities;

namespace Patients.DataAccess.Repositories;

public class InsuranceRepository : IInsuranceRepository
{
    private readonly PatientDbContext _context;

    public InsuranceRepository(PatientDbContext context)
    {
        _context = context;
    }

    public async Task Add(Insurance insurance)
    {
        var insuranceEntity = MapToEntity(insurance);

        _context.Insurances.Add(insuranceEntity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Insurance insurance)
    {
        var insuranceEntity = await _context.Insurances.FirstOrDefaultAsync(c => c.Id == insurance.Id);
        if(insuranceEntity == null)
            throw new InvalidOperationException("Insurance not found");

        insuranceEntity.InsuranceCompany = insurance.InsuranceCompany;
        insuranceEntity.PolicyNumber = insurance.PolicyNumber;
        insuranceEntity.ValidFrom = insurance.ValidFrom;
        insuranceEntity.ValidTo = insurance.ValidTo;
        insuranceEntity.PatientId = insurance.PatientId;

        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var insuranceEntity = await _context.Insurances.FirstOrDefaultAsync(c => c.Id == id);
        if(insuranceEntity == null)
            throw new InvalidOperationException("Insurance not found");

        _context.Insurances.Remove(insuranceEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<Insurance?> GetById(Guid id)
    {
        var insuranceEntity = await _context.Insurances
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return insuranceEntity == null ? null : MapToDomain(insuranceEntity);
    }

    public async Task<List<Insurance>> GetByAllPatientId(Guid patientId)
    {
        var insureEntities = await _context.Insurances
            .AsNoTracking()
            .Where(c => c.PatientId == patientId)
            .ToListAsync();

        return insureEntities.Select(MapToDomain).ToList();
    }

    private static InsuranceEntity MapToEntity(Insurance insurance) => new InsuranceEntity()
    {
        Id = insurance.Id,
        InsuranceCompany = insurance.InsuranceCompany,
        PolicyNumber = insurance.PolicyNumber,
        ValidFrom = insurance.ValidFrom,
        ValidTo = insurance.ValidTo,
        PatientId = insurance.PatientId
    };
    
    private static Insurance MapToDomain(InsuranceEntity insuranceEntity) => Insurance.FromPersistence(
        insuranceEntity.Id,
        insuranceEntity.PatientId,
        insuranceEntity.InsuranceCompany,
        insuranceEntity.PolicyNumber,
        insuranceEntity.ValidFrom,
        insuranceEntity.ValidTo);
}