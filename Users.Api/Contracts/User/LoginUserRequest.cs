using System.ComponentModel.DataAnnotations;

namespace Users.Api.Contracts.User;

public record LoginUserRequest(
    [Required] string Email,
    [Required] string Password);