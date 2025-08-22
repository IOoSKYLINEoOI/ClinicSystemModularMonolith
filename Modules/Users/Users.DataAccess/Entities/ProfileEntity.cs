using Users.Core.Enums;

namespace Users.Domain.Entities;

public class ProfileEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
}