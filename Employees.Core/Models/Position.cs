using CSharpFunctionalExtensions;

namespace Employees.Core.Models;

public class Position
{
    private Position(
        int id,
        string name)
    {
        Id = id;
        Name = name;
    }
    
    public int Id { get; }
    public string Name { get; }

    public static Result<Position> Create(
        int id,
        string name)
    {
        var position = new Position(id, name);
        
        return Result.Success(position);
    }
}