using Employees.Api.Contracts.Certificate;
using Employees.Api.Contracts.EmployeeCertificate;
using Employees.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

[ApiController]
[Route("employee-certificates")]
public class CertificateController : ControllerBase
{
    private readonly IEmployeeCertificateService _employeeCertificateService;

    public CertificateController(IEmployeeCertificateService employeeCertificateService)
    {
        _employeeCertificateService = employeeCertificateService;
    }

    [HttpPost]
    public async Task<IActionResult> AddNewCertificate([FromBody] AddNewCertificateRequest request)
    {
        var result = await _employeeCertificateService.AddCertificate(
            request.EmployeeId, 
            request.Name,
            request.IssuedBy,
            request.IssuedAt,
            request.ValidUntil);
        
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCertificate([FromBody] UpdateCertificateRequest request)
    {
        var result = await _employeeCertificateService.UpdateCertificate(
            request.Id,
            request.Name,
            request.IssuedBy,
            request.ValidUntil);
            
        return result.IsSuccess ? Ok() : BadRequest(result.Error); 
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CertificateResponse>> GetCertificate(Guid id)
    {
        var certificateResult = await _employeeCertificateService.GetCertificate(id);
        if(certificateResult.IsFailure)
            return NotFound(certificateResult.Error);
        
        var response = new CertificateResponse(
            certificateResult.Value.Id,
            certificateResult.Value.EmployeeId,
            certificateResult.Value.Name,
            certificateResult.Value.IssuedBy,
            certificateResult.Value.IssuedAt,
            certificateResult.Value.ValidUntil);
        
        return Ok(response);
    }

    [HttpGet("by-employee/{employeeId}")]
    public async Task<ActionResult<List<CertificateResponse>>> GetCertificateByEmployee(Guid employeeId)
    {
        var certificatesResult = await _employeeCertificateService.GetEmployeeCertificates(employeeId);
        if(certificatesResult.IsFailure)
            return NotFound(certificatesResult.Error);
        
        var response = new List<CertificateResponse>();

        foreach (var certificate in certificatesResult.Value)
        {
            var res = new CertificateResponse(
                certificate.Id,
                certificate.EmployeeId,
                certificate.Name,
                certificate.IssuedBy,
                certificate.IssuedAt,
                certificate.ValidUntil);
            
            response.Add(res);
        }
        
        return Ok(response);
    }

    [HttpGet("valid-on/{employeeId}/{asOfDate}")]
    public async Task<ActionResult<List<CertificateResponse>>> GetCertificateValidOnDate(Guid employeeId, DateOnly days)
    {
        var certificatesResult = await _employeeCertificateService.GetEmployeeCertificatesValidOnDate(employeeId, days);
        if(certificatesResult.IsFailure)
            return NotFound(certificatesResult.Error);
        
        var response = new List<CertificateResponse>();

        foreach (var certificate in certificatesResult.Value)
        {
            var res = new CertificateResponse(
                certificate.Id,
                certificate.EmployeeId,
                certificate.Name,
                certificate.IssuedBy,
                certificate.IssuedAt,
                certificate.ValidUntil);
            
            response.Add(res);
        }
        
        return Ok(response);
    }

    [HttpPost("filter")]
    public async Task<ActionResult<List<CertificateResponse>>> CertificateFilter(
        [FromBody] CertificateFilterRequest request)
    {
        var certificatesResult = await _employeeCertificateService.Filter(
            request.EmployeeId,
            request.Name,
            request.IssuedBy,
            request.IssuedFrom,
            request.IssuedTo,
            request.ValidUntilFrom,
            request.ValidUntilTo);
        if(certificatesResult.IsFailure)
            return NotFound(certificatesResult.Error);
        
        var response = new List<CertificateResponse>();

        foreach (var certificate in certificatesResult.Value)
        {
            var res = new CertificateResponse(
                certificate.Id,
                certificate.EmployeeId,
                certificate.Name,
                certificate.IssuedBy,
                certificate.IssuedAt,
                certificate.ValidUntil);
            
            response.Add(res);
        }
        
        return Ok(response);
    }
}