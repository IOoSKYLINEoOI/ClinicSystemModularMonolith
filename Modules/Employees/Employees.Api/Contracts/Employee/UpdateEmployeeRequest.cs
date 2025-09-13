using System.ComponentModel.DataAnnotations;

namespace Employees.Api.Contracts.Employee;

public record UpdateEmployeeRequest(
    [Required] Guid Id,
    [Required] Guid UserId,
    [Required] DateOnly HireDate,
    DateOnly FireDate);