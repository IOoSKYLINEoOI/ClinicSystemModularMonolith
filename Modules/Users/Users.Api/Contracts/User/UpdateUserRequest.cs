using System.ComponentModel.DataAnnotations;

namespace Users.Api.Contracts.User;
public record UpdateUserRequest(
        [Required] string FirstName,
        [Required] string LastName,
        string? FatherName,
        [Required] DateOnly DateOfBirth,
        [Required][EmailAddress] string Email,
        [Required][Phone] string PhoneNumber);
