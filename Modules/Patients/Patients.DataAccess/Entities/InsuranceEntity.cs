namespace Patients.DataAccess.Entities;

public class InsuranceEntity
{
    public required Guid Id { get; set; }
    public required string InsuranceCompany { get; set; }  
    public required string PolicyNumber { get; set; }      
    public required DateOnly ValidFrom { get; set; }       
    public required DateOnly ValidTo { get; set; }        
    
    public required Guid PatientId { get; set; }
    public required PatientEntity Patient { get; set; }

}