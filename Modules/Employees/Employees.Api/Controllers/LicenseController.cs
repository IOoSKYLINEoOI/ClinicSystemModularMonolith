using AutoMapper;
using Employees.Api.Contracts.License;
using Employees.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employees.Api.Controllers;

[ApiController]
[Route("employee-licenses")]
[ApiExplorerSettings(GroupName = "Employees / LicenseController")]
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
    [SwaggerOperation(
        OperationId = "AddNewLicense",
        Summary = "Добавить лицензию сотрудника",
        Description = "Создает новую лицензия для указанного сотрудника с указанием номера, организации, выдавшей сертификат, и срока действия."
    )]
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
    [SwaggerOperation(
        OperationId = "UpdateLicense",
        Summary = "Обновить лицензию сотрудника",
        Description = "Обновляет данные существующей лицензии сотрудника по ее идентификатору. Можно изменить номер, выдавшую организацию и срок действия."
    )]
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
    [SwaggerOperation(
        OperationId = "GetLicense",
        Summary = "Получить лицензию сотрудника по Id",
        Description = "Возвращает подробную информацию о лицензии сотрудника по ее уникальному идентификатору."
    )]
    public async Task<ActionResult<LicenseResponse>> GetLicense(Guid id)
    {
        var certificateResult = await _employeeLicenseService.GetLicense(id);
        if(certificateResult.IsFailure)
            return NotFound(certificateResult.Error);
        
        var response = _mapper.Map<LicenseResponse>(certificateResult.Value);
        
        return Ok(response);
    }
    
    [HttpGet("by-employee/{employeeId}")]
    [SwaggerOperation(
        OperationId = "GetLicenseByEmployee",
        Summary = "Получить все лицензии сотрудника по Id сотрудника",
        Description = "Возвращает список всех лицензий, выданных конкретному сотруднику по его идентификатору."
    )]
    public async Task<ActionResult<List<LicenseResponse>>> GetLicenseByEmployee(Guid employeeId)
    {
        var certificatesResult = await _employeeLicenseService.GetEmployeeLicenses(employeeId);
        if(certificatesResult.IsFailure)
            return NotFound(certificatesResult.Error);
        
        var response = _mapper.Map<List<LicenseResponse>>(certificatesResult.Value);
        
        return Ok(response);
    }
    
    [HttpGet("valid-on/{employeeId}")]
    [SwaggerOperation(
        OperationId = "GetLicenseValidOnDate",
        Summary = "Получить все лицензии сотрудника по Id сотрудника на дату",
        Description = "Возвращает лицензии сотрудника, действительные на указанную дату. Используется для проверки актуальности сертификатов."
    )]
    public async Task<ActionResult<List<LicenseResponse>>> GetLicenseValidOnDate(Guid employeeId,[FromQuery] DateOnly days)
    {
        var certificatesResult = await _employeeLicenseService.GetEmployeeLicensesValidOnDate(employeeId, days);
        if(certificatesResult.IsFailure)
            return NotFound(certificatesResult.Error);
        
        var response = _mapper.Map<List<LicenseResponse>>(certificatesResult.Value);
        
        return Ok(response);
    }
    
    [HttpPost("filter")]
    [SwaggerOperation(
        OperationId = "LicenseFilter",
        Summary = "Получить отфильтрованные лицензии",
        Description = "Позволяет получить список лицензий по заданным фильтрам: имя сотрудника, организация, дата выдачи и срок действия."
    )]
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