using CSharpFunctionalExtensions;
using Employees.Core.Models;

namespace Employees.Core.Interfaces.Repositories;

public interface IPositionRepository
{
    Task<Result<List<Position>>> GetAll();
    Task<Result> Add(Position position);
    Task<Result<Position>> GetById(int id);
}