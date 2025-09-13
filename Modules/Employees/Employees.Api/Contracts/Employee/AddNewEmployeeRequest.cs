using System.ComponentModel.DataAnnotations;

namespace Employees.Api.Contracts.Employee;

public record AddNewEmployeeRequest(
    [Required] Guid UserId,
    [Required] DateOnly HireDate,
    DateOnly FireDate);
