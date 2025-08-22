namespace Patients.DataAccess.Entities;

public class ContactEntity
{
    public required Guid Id { get; set; }
    public required string ContactName { get; set; }   
    public required string Relationship { get; set; }  
    public required string Phone { get; set; }
    public string? Email { get; set; }
    
    public required Guid PatientId { get; set; }
    public PatientEntity Patient { get; set; } = null!;
}