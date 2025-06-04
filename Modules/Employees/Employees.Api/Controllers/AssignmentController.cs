using AutoMapper;
using Employees.Api.Contracts.Assignment;
using Employees.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

[ApiController]
[Route("assignment")]
public class AssignmentController : ControllerBase
{
    private readonly IEmployeeAssignmentService _employeeAssignmentService;
    private readonly IMapper _mapper;

    public AssignmentController(IEmployeeAssignmentService employeeAssignmentService, IMapper mapper)
    {
        _employeeAssignmentService = employeeAssignmentService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AddNewAssignment([FromBody] AddNewAssignmentRequest request)
    {
        var result = await _employeeAssignmentService.AddAssignment(
            request.EmployeeId,
            request.PositionId,
            request.DepartmentId,
            request.AssignedAt,
            request.RemovedAt);
        
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAssignment([FromBody] UpdateAssignmentRequest request)
    {
        var result = await _employeeAssignmentService.UpdateAssignment(
            request.Id,
            request.EmployeeId,
            request.PositionId,
            request.DepartmentId,
            request.AssignedAt,
            request.RemovedAt);
        
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AssignmentResponse>> GetAssignment(Guid id)
    {
        var assignmentResult = await _employeeAssignmentService.GetAssignment(id);
        if(assignmentResult.IsFailure)
            return NotFound(assignmentResult.Error);
        
        var response = _mapper.Map<AssignmentResponse>(assignmentResult.Value);
        
        return Ok(response);
    }

    [HttpGet("by-employee/{employeeId:guid}")]
    public async Task<ActionResult<List<AssignmentResponse>>> GetAssignmentByEmployee(Guid employeeId)
    {
        var assignmentResult = await _employeeAssignmentService.GetEmployeeAssignments(employeeId);
        if(assignmentResult.IsFailure)
            return NotFound(assignmentResult.Error);
        
        var response = _mapper.Map<List<AssignmentResponse>>(assignmentResult.Value);
        
        return Ok(response);
    }
    
    [HttpGet("by-employee/{employeeId:guid}/{onDate:dateonly}")]
    public async Task<ActionResult<List<AssignmentResponse>>> GetAssignmentByDepartmentOnDate(Guid employeeId, DateOnly onDate)
    {
        var assignmentResult = await _employeeAssignmentService.GetEmployeeByDepartmentAssignments(employeeId, onDate);
        if(assignmentResult.IsFailure)
            return NotFound(assignmentResult.Error);
        
        var response = _mapper.Map<List<AssignmentResponse>>(assignmentResult.Value);
        
        return Ok(response);
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<List<AssignmentResponse>>> GetAssignmentAll()
    {
        var assignmentResult = await _employeeAssignmentService.GetAllEmployeeAssignment();
        if(assignmentResult.IsFailure)
            return NotFound(assignmentResult.Error);
        
        var response = _mapper.Map<List<AssignmentResponse>>(assignmentResult.Value);
        
        return Ok(response);
    }
}