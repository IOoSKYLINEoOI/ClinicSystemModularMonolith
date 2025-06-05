using Employees.Api.Contracts.Certificate;
using Employees.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employees.Api.Controllers;

[ApiController]
[Route("employee-certificates")]
[ApiExplorerSettings(GroupName = "Employees / CertificateController")]
public class CertificateController : ControllerBase
{
    private readonly IEmployeeCertificateService _employeeCertificateService;

    public CertificateController(IEmployeeCertificateService employeeCertificateService)
    {
        _employeeCertificateService = employeeCertificateService;
    }

    [HttpPost]
    [SwaggerOperation(
        OperationId = "AddNewCertificate",
        Summary = "Добавить сертификат сотрудника",
        Description = "Создает новый сертификат для указанного сотрудника с указанием названия, организации, выдавшей сертификат, и срока действия."
    )]
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
    [SwaggerOperation(
        OperationId = "UpdateCertificate",
        Summary = "Обновить сертификат сотрудника",
        Description = "Обновляет данные существующего сертификата сотрудника по его идентификатору. Можно изменить название, выдавшую организацию и срок действия."
    )]
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
    [SwaggerOperation(
        OperationId = "GetCertificate",
        Summary = "Получить сертификат сотрудника по Id",
        Description = "Возвращает подробную информацию о сертификате сотрудника по его уникальному идентификатору."
    )]
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
    [SwaggerOperation(
        OperationId = "GetCertificateByEmployee",
        Summary = "Получить все сертификаты сотрудника по Id сотрудника",
        Description = "Возвращает список всех сертификатов, выданных конкретному сотруднику по его идентификатору."
    )]
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

    [HttpGet("valid-on/{employeeId}")]
    [SwaggerOperation(
        OperationId = "GetCertificateValidOnDate",
        Summary = "Получить все сертификаты сотрудника по Id сотрудника на дату",
        Description = "Возвращает сертификаты сотрудника, действительные на указанную дату. Используется для проверки актуальности сертификатов."
    )]
    public async Task<ActionResult<List<CertificateResponse>>> GetCertificateValidOnDate(Guid employeeId,[FromQuery] DateOnly days)
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
    [SwaggerOperation(
        OperationId = "CertificateFilter",
        Summary = "Получить отфильтрованные сертификаты",
        Description = "Позволяет получить список сертификатов по заданным фильтрам: имя сотрудника, организация, дата выдачи и срок действия."
    )]
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