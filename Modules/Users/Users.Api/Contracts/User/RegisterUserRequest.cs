using System.ComponentModel.DataAnnotations;
using Users.Core.Enums;

namespace Users.Api.Contracts.User;

public record RegisterUserRequest(
    [Required] string FirstName,
    [Required] string LastName,
    string? FatherName,
    [Required] DateOnly DateOfBirth,
    [Required] Gender Gender,
    [Required][EmailAddress] string Email,
    [Required][Phone] string PhoneNumber,
    [Required] string Password);

    
