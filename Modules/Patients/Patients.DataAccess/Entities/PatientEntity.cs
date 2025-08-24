using Microsoft.EntityFrameworkCore;
using Patients.Core.Enums;

namespace Patients.DataAccess.Entities;

public class PatientEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public BloodProfileEntity? BloodProfileEntity { get; set; }
    
    public ICollection<ContactEntity> Contacts { get; set; } = new List<ContactEntity>(); 
    public InsuranceEntity? Insurance { get; set; }
    public QuestionnaireEntity? Questionnaire { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}

[Owned]
public class BloodProfileEntity
{
    public required BloodGroup Type { get; set; }
    public required RhFactor Rh { get; set; }
    public required KellFactor Kell { get; set; }
}