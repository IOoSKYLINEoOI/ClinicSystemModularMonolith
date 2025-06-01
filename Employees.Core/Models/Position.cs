using CSharpFunctionalExtensions;

namespace Employees.Core.Models;

public class Position
{
    private Position(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; private set; }  
    public string Name { get; }


    public static Result<Position> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Position>("Name cannot be empty");

        return Result.Success(new Position(0, name)); 
    }
    
    public static Result<Position> Load(int id, string name)
    {
        return Result.Success(new Position(id, name));
    }
}
