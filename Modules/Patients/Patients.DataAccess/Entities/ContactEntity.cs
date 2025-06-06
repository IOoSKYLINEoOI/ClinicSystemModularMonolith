namespace Patients.DataAccess.Entities;

public class ContactEntity
{
    public required Guid Id { get; set; }
    public required string ContactName { get; set; }   
    public required string Relationship { get; set; }  
    public required string Phone { get; set; }
    public string? Email { get; set; }
    
    public required Guid PatientId { get; set; }
    public required PatientEntity Patient { get; set; }
}