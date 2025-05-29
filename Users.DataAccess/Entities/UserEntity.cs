using Users.Core.ValueObjects;

namespace Users.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public Email Email { get; set; } = null!;
    public PhoneNumber PhoneNumber { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    
    public UserProfileEntity Profile { get; set; } = null!;
    public ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();
}
