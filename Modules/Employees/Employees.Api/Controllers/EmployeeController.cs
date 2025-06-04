using Employees.Api.Contracts.Employee;
using Employees.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost]
    public async Task<IActionResult> AddNewEmployee([FromBody] AddNewEmployeeRequest request)
    {
        var result = await _employeeService.AddEmployee(request.UserId, request.HireDate, request.FireDate);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeRequest request)
    {
        var result = await _employeeService.UpdateEmployee(request.Id, request.UserId, request.HireDate, request.FireDate);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EmployeeResponse>> GetEmployee([FromRoute] Guid id)
    {
        var employeeResult = await _employeeService.GetEmployee(id);
        if (employeeResult.IsFailure)
            return NotFound(employeeResult.Error);

        var response = new EmployeeResponse(
            employeeResult.Value.Id,
            employeeResult.Value.UserId,
            employeeResult.Value.HireDate,
            employeeResult.Value.FireDate,
            employeeResult.Value.IsActive);
        
        return Ok(response);
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<List<EmployeeResponse>>> GetAllEmployees()
    {
        var employees = await _employeeService.AllEmployees();
        if (employees.IsFailure)
            return BadRequest(employees.Error);
        
        var result = new List<EmployeeResponse>();
        foreach (var employee in employees.Value)
        {
            var response = new EmployeeResponse(
                employee.Id,
                employee.UserId,
                employee.HireDate,
                employee.FireDate,
                employee.IsActive);
            
            result.Add(response);
        }
        
        return Ok(result);
    }

    [HttpGet("is-active/{id:guid}")]
    public async Task<ActionResult<bool>> GetIsActiveEmployee([FromRoute] Guid id)
    {
        var result = await _employeeService.GetIsActive(id);
        return result.IsSuccess ? Ok(result.Value) :  NotFound(result.Error);
    }

    [HttpGet("by-user/{userId:guid}")]
    public async Task<ActionResult<EmployeeResponse>> GetEmployeeByUserId([FromRoute] Guid userId)
    {
        var employeeResult = await _employeeService.GetEmployeesByUserId(userId);
        if (employeeResult.IsFailure)
            return NotFound(employeeResult.Error);

        var response = new EmployeeResponse(
            employeeResult.Value.Id,
            employeeResult.Value.UserId,
            employeeResult.Value.HireDate,
            employeeResult.Value.FireDate,
            employeeResult.Value.IsActive);
        
        return Ok(response);
    }

    [HttpGet("recent/{days:int}")]
    public async Task<ActionResult<List<EmployeeResponse>>> GetEmployeeRecentHired(int days = 30)
    {
        var employees = await _employeeService.GetEmployeeRecentHired(days);
        if (employees.IsFailure)
            return BadRequest(employees.Error);
        
        var result = new List<EmployeeResponse>();
        foreach (var employee in employees.Value)
        {
            var response = new EmployeeResponse(
                employee.Id,
                employee.UserId,
                employee.HireDate,
                employee.FireDate,
                employee.IsActive);
            
            result.Add(response);
        }
        
        return Ok(result);
    }
}