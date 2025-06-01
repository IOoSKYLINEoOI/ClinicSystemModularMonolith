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

    public async Task<IActionResult> AddNewEmployee([FromBody] AddNewEmployeeRequest request)
    {
        var result = await _employeeService.AddEmployee(request.UserId, request.HireDate, request.FireDate);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeRequest request)
    {
        var result = await _employeeService.UpdateEmployee(request.Id, request.UserId, request.HireDate, request.FireDate);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    [HttpGet("{employeeId:guid}")]
    public async Task<ActionResult<EmployeeResponse>> GetEmployee([FromRoute] Guid employeeId)
    {
        var employeeResult = await _employeeService.GetEmployee(employeeId);
        if (employeeResult.IsFailure)
            return NotFound(employeeResult.Error)

        var response = new EmployeeResponse(
            employeeResult.Value.Id,
            employeeResult.Value.UserId,
            employeeResult.Value.HireDate,
            employeeResult.Value.FireDate,
            employeeResult.Value.IsActive);
        
        return Ok(response);
    }
    
    [HttpGet("allEmployees")]
    public async Task<ActionResult<List<EmployeeResponse>>> GetAllEmployees()
    {
        var result = await _employeeService.AllEmployees();
        return result.IsSuccess ? Ok(result) :  NotFound(result.Error);
    }
    
    public async Task
}