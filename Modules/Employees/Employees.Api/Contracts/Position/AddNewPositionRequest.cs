using System.ComponentModel.DataAnnotations;

namespace Employees.Api.Contracts.Position;

public record AddNewPositionRequest(
    [Required] string Name);
