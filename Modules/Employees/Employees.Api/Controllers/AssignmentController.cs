using AutoMapper;
using Employees.Api.Contracts.Assignment;
using Employees.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employees.Api.Controllers;

[ApiController]
[Route("assignment")]
[ApiExplorerSettings(GroupName = "Employees / AssignmentController")]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;
    private readonly IMapper _mapper;

    public AssignmentController(IAssignmentService assignmentService, IMapper mapper)
    {
        _assignmentService = assignmentService;
        _mapper = mapper;
    }

    [HttpPost]
    [SwaggerOperation(
        OperationId = "AddNewAssignment",
        Summary = "Добавить новое назначение для сотрудника",
        Description = "Создает новое назначение сотрудника с указанными параметрами: ID сотрудника, должность, департамент и даты назначения."
    )]
    public async Task<IActionResult> AddNewAssignment([FromBody] AddNewAssignmentRequest request)
    {
        var result = await _assignmentService.AddAssignment(
            request.EmployeeId,
            request.PositionId,
            request.DepartmentId,
            request.AssignedAt,
            request.RemovedAt);
        
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpPut]
    [SwaggerOperation(
        OperationId = "UpdateAssignment",
        Summary = "Редактировать назначение сотрудника",
        Description = "Обновляет существующее назначение сотрудника с новыми данными, такими как должность, департамент и даты назначения."
    )]
    public async Task<IActionResult> UpdateAssignment([FromBody] UpdateAssignmentRequest request)
    {
        var result = await _assignmentService.UpdateAssignment(
            request.Id,
            request.EmployeeId,
            request.PositionId,
            request.DepartmentId,
            request.AssignedAt,
            request.RemovedAt);
        
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        OperationId = "GetAssignment",
        Summary = "Получить назначение сотрудника по Id",
        Description = "Возвращает информацию о назначении сотрудника по уникальному идентификатору назначения."
    )]
    public async Task<ActionResult<AssignmentResponse>> GetAssignment(Guid id)
    {
        var assignmentResult = await _assignmentService.GetAssignment(id);
        if(assignmentResult.IsFailure)
            return NotFound(assignmentResult.Error);
        
        var response = _mapper.Map<AssignmentResponse>(assignmentResult.Value);
        
        return Ok(response);
    }

    [HttpGet("by-employee/{employeeId:guid}")]
    [SwaggerOperation(
        OperationId = "GetAssignmentByEmployee",
        Summary = "Получить все назначения сотрудника по Id сотрудника",
        Description = "Возвращает список всех назначений, связанных с указанным сотрудником по его уникальному идентификатору."
    )]
    public async Task<ActionResult<List<AssignmentResponse>>> GetAssignmentByEmployee(Guid employeeId)
    {
        var assignmentResult = await _assignmentService.GetEmployeeAssignments(employeeId);
        if(assignmentResult.IsFailure)
            return NotFound(assignmentResult.Error);
        
        var response = _mapper.Map<List<AssignmentResponse>>(assignmentResult.Value);
        
        return Ok(response);
    }
    
    [HttpGet("by-employee/{employeeId:guid}/by-date")]
    [SwaggerOperation(
        OperationId = "GetAssignmentByDepartmentOnDate",
        Summary = "Получить все назначения для данного департамента на указанную дату",
        Description = "Возвращает назначения сотрудника по ID и фильтрует их по указанной дате, относящиеся к определённому департаменту."
    )]
    public async Task<ActionResult<List<AssignmentResponse>>> GetAssignmentByDepartmentOnDate(Guid employeeId,[FromQuery] DateOnly onDate)
    {
        var assignmentResult = await _assignmentService.GetEmployeeByDepartmentAssignments(employeeId, onDate);
        if(assignmentResult.IsFailure)
            return NotFound(assignmentResult.Error);
        
        var response = _mapper.Map<List<AssignmentResponse>>(assignmentResult.Value);
        
        return Ok(response);
    }
    
    [HttpGet("all")]
    [SwaggerOperation(
        OperationId = "GetAssignmentsAll",
        Summary = "Получить все действующие назначения",
        Description = "Возвращает список всех активных назначений сотрудников без фильтров."
    )]
    public async Task<ActionResult<List<AssignmentResponse>>> GetAssignmentAll()
    {
        var assignmentResult = await _assignmentService.GetAllEmployeeAssignment();
        if(assignmentResult.IsFailure)
            return NotFound(assignmentResult.Error);
        
        var response = _mapper.Map<List<AssignmentResponse>>(assignmentResult.Value);
        
        return Ok(response);
    }
}