using AutoMapper;
using Employees.Api.Contracts.License;
using Employees.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

[ApiController]
[Route("employee-licenses")]
public class LicenseController: ControllerBase
{
    private readonly IEmployeeLicenseService _employeeLicenseService;
    private readonly IMapper _mapper;

    public LicenseController(IEmployeeLicenseService employeeLicenseService, IMapper mapper)
    {
        _employeeLicenseService = employeeLicenseService;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddNewLicense([FromBody] AddNewLicenseRequest request)
    {
        var result = await _employeeLicenseService.AddLicense(
            request.EmployeeId, 
            request.LicenseNumber,
            request.IssuedBy,
            request.IssuedAt,
            request.ValidUntil);
        
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateLicense([FromBody] UpdateLicenseRequest request)
    {
        var result = await _employeeLicenseService.UpdateLicense(
            request.Id,
            request.LicenseNumber,
            request.IssuedBy,
            request.ValidUntil);
            
        return result.IsSuccess ? Ok() : BadRequest(result.Error); 
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LicenseResponse>> GetLicense(Guid id)
    {
        var certificateResult = await _employeeLicenseService.GetLicense(id);
        if(certificateResult.IsFailure)
            return NotFound(certificateResult.Error);
        
        var response = _mapper.Map<LicenseResponse>(certificateResult.Value);
        
        return Ok(response);
    }
    
    [HttpGet("by-employee/{employeeId}")]
    public async Task<ActionResult<List<LicenseResponse>>> GetLicenseByEmployee(Guid employeeId)
    {
        var certificatesResult = await _employeeLicenseService.GetEmployeeLicenses(employeeId);
        if(certificatesResult.IsFailure)
            return NotFound(certificatesResult.Error);
        
        var response = _mapper.Map<List<LicenseResponse>>(certificatesResult.Value);
        
        return Ok(response);
    }
    
    [HttpGet("valid-on/{employeeId}/{asOfDate}")]
    public async Task<ActionResult<List<LicenseResponse>>> GetLicenseValidOnDate(Guid employeeId, DateOnly days)
    {
        var certificatesResult = await _employeeLicenseService.GetEmployeeLicensesValidOnDate(employeeId, days);
        if(certificatesResult.IsFailure)
            return NotFound(certificatesResult.Error);
        
        var response = _mapper.Map<List<LicenseResponse>>(certificatesResult.Value);
        
        return Ok(response);
    }
    
    [HttpPost("filter")]
    public async Task<ActionResult<List<LicenseResponse>>> LicenseFilter(
        [FromBody] LicenseFilterRequest request)
    {
        var certificatesResult = await _employeeLicenseService.Filter(
            request.EmployeeId,
            request.LicenseNumber,
            request.IssuedBy,
            request.IssuedFrom,
            request.IssuedTo,
            request.ValidUntilFrom,
            request.ValidUntilTo);
        if(certificatesResult.IsFailure)
            return NotFound(certificatesResult.Error);
        
        var response = _mapper.Map<List<LicenseResponse>>(certificatesResult.Value);
        
        return Ok(response);
    }
}