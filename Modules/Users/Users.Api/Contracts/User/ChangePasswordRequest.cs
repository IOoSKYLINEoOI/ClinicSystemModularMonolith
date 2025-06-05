using System.ComponentModel.DataAnnotations;

namespace Users.Api.Contracts.User;

public record ChangePasswordRequest(
    [Required] string CurrentPassword,
    [Required] string NewPassword);
