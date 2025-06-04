using CSharpFunctionalExtensions;

namespace Employees.Core.Models;

public class Employee
{
    private static readonly DateOnly MaxDeltaDate = DateOnly.FromDateTime(DateTime.Now.AddDays(90));
    private static readonly DateOnly MinDeltaDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-90));

    private Employee(
        Guid id, 
        Guid userId, 
        DateOnly hireDate, 
        DateOnly? fireDate, 
        bool isActive)
    {
        Id = id;
        UserId = userId;
        HireDate = hireDate;
        FireDate = fireDate;
        IsActive = isActive;
    }

    public Guid Id { get; }
    public Guid UserId { get; }
    public DateOnly HireDate { get; }
    public DateOnly? FireDate { get; }
    public bool IsActive { get; }

    public static Result<Employee> Create(
        Guid id,
        Guid userId,
        DateOnly hireDate,
        DateOnly? fireDate,
        bool isActive)
    {
        if (userId == Guid.Empty)
        {
            return Result.Failure<Employee>(
                $"Invalid references: all foreign keys must be non-empty GUIDs.");
        }
        
        if (fireDate.HasValue && fireDate.Value < hireDate)
        {
            return Result.Failure<Employee>(
                $"'RemovedAt' cannot be earlier than 'AssignedAt'.");
        }
        
        if (hireDate.CompareTo(MaxDeltaDate) > 0 || hireDate.CompareTo(MinDeltaDate) < 0)
        {
            return Result.Failure<Employee>(
                $"'{nameof(hireDate)}' is out of range. Must be between {MinDeltaDate.ToString("yyyy-MM-dd")} and {MaxDeltaDate.ToString("yyyy-MM-dd")}.");
        }

        if (fireDate.HasValue)
        {
            if (fireDate.Value.CompareTo(MaxDeltaDate) > 0 || fireDate.Value.CompareTo(MinDeltaDate) < 0)
            {
                return Result.Failure<Employee>(
                    $"'{nameof(fireDate)}' is out of range. Must be between {MinDeltaDate:yyyy-MM-dd} and {MaxDeltaDate:yyyy-MM-dd}.");
            }
        }

        var employee = new Employee(
            id,
            userId,
            hireDate,
            fireDate,
            isActive);

        return Result.Success(employee);
    }
}
        
    

    
    
