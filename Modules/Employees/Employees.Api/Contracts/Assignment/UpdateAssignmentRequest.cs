using System.ComponentModel.DataAnnotations;

namespace Employees.Api.Contracts.Assignment;

public record UpdateAssignmentRequest(
    [Required] Guid Id,
    [Required] Guid EmployeeId,
    [Required] int PositionId,
    [Required] Guid DepartmentId,
    [Required] DateOnly AssignedAt,
    DateOnly? RemovedAt);
