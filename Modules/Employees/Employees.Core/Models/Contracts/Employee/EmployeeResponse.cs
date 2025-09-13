namespace Employees.Api.Contracts.Employee;

public record EmployeeResponse(
    Guid Id,
    Guid UserId,
    DateOnly HireDate,
    DateOnly? FireDate,
    bool IsActive);
