using CSharpFunctionalExtensions;

namespace Patient.Core.Models;

public class Insurance
{
    private const int MaxLengthInsuranceCompany = 100;
    private const int MaxLengthPolicyNumber = 50;
    
    public Insurance(
        Guid id, 
        Guid patientId, 
        string insuranceCompany, 
        string policyNumber, 
        DateOnly validFrom, 
        DateOnly validTo)
    {
        Id = id;
        PatientId = patientId;
        InsuranceCompany = insuranceCompany;
        PolicyNumber = policyNumber;
        ValidFrom = validFrom;
        ValidTo = validTo;
    }

    public Guid Id { get; }
    public Guid PatientId { get; }
    public string InsuranceCompany { get; }  
    public string PolicyNumber { get; }      
    public DateOnly ValidFrom { get; }       
    public DateOnly ValidTo { get; }

    public static Result<Insurance> Create(
        Guid id,
        Guid patientId,
        string insuranceCompany,
        string policyNumber,
        DateOnly validFrom,
        DateOnly validTo)
    {
        if (patientId == Guid.Empty)
            return Result.Failure<Insurance>("PatientId не может быть пустым GUID.");
        
        if (insuranceCompany.Length > MaxLengthInsuranceCompany)
            return Result.Failure<Insurance>($"'{nameof(insuranceCompany)}' more than {MaxLengthInsuranceCompany} characters.");
        
        if (policyNumber.Length > MaxLengthPolicyNumber)
            return Result.Failure<Insurance>($"'{nameof(policyNumber)}' more than {MaxLengthPolicyNumber} characters.");
        
        if (validFrom < validTo)
            return Result.Failure<Insurance>($"'{nameof(validTo)}' more than '{nameof(validFrom)}'.");
        
        var insurance = new Insurance(
            id,
            patientId,
            insuranceCompany,
            policyNumber,
            validFrom,
            validTo);
        
        return Result.Success(insurance);
    }
}