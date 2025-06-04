namespace Employees.Api.Contracts.Assignment;

public record AssignmentResponse(
    Guid Id,
    Guid EmployeeId,
    int PositionId,
    Guid DepartmentId,
    DateOnly AssignedAt,
    DateOnly? RemovedAt);