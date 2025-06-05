using Employees.Api.Contracts.Employee;
using Employees.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employees.Api.Controllers;

[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(GroupName = "Employees / EmployeeController")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost]
    [SwaggerOperation(
        OperationId = "AddNewEmployee",
        Summary = "Добавить нового сотрудника",
        Description = "Создает новую запись о сотруднике на основе переданных данных: идентификатора пользователя, даты приема на работу и, при необходимости, даты увольнения."
    )]
    public async Task<IActionResult> AddNewEmployee([FromBody] AddNewEmployeeRequest request)
    {
        var result = await _employeeService.AddEmployee(request.UserId, request.HireDate, request.FireDate);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpPut]
    [SwaggerOperation(
        OperationId = "UpdateEmployee",
        Summary = "Обновить данные сотрудника",
        Description = "Обновляет существующую запись о сотруднике. Можно изменить идентификатор пользователя, дату приема на работу и дату увольнения."
    )]
    public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeRequest request)
    {
        var result = await _employeeService.UpdateEmployee(request.Id, request.UserId, request.HireDate, request.FireDate);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        OperationId = "GetEmployee",
        Summary = "Получить данные сотрудника по Id",
        Description = "Возвращает информацию о сотруднике по его уникальному идентификатору (GUID)."
    )]
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
    [SwaggerOperation(
        OperationId = "GetAllEmployees",
        Summary = "Получить всех действующих сотрудников",
        Description = "Возвращает список всех сотрудников, которые в настоящее время считаются активными (не уволены)."
    )]
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
    [SwaggerOperation(
        OperationId = "GetIsActiveEmployee",
        Summary = "Проверка активности данных сотрудника",
        Description = "Проверяет, является ли сотрудник с указанным идентификатором активным (не уволенным)."
    )]
    public async Task<ActionResult<bool>> GetIsActiveEmployee([FromRoute] Guid id)
    {
        var result = await _employeeService.GetIsActive(id);
        return result.IsSuccess ? Ok(result.Value) :  NotFound(result.Error);
    }

    [HttpGet("by-user/{userId:guid}")]
    [SwaggerOperation(
        OperationId = "GetEmployeeByUserId",
        Summary = "Получить данные сотрудника по Id пользователя",
        Description = "Возвращает данные сотрудника, связанного с указанным пользователем по идентификатору пользователя (UserId)."
    )]
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
    [SwaggerOperation(
        OperationId = "GetEmployeeRecentHired",
        Summary = "Получить данные новых сотрудников за последние N дней",
        Description = "Возвращает список сотрудников, принятых на работу за последние N дней. Значение по умолчанию — 30 дней."
    )]
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