using CSharpFunctionalExtensions;
using Employees.Core.Enums;
using Employees.Core.Models;

namespace Employees.Core.Interfaces.Services;

public interface IPositionService
{
    Task<Result<List<Position>>> GetAllPositions();
    Task<Result> AddPosition(string name);
    Task<Result<Position>> GetPositionById(int id);
}