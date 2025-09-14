using Microsoft.AspNetCore.Mvc;
using Patients.Api.Contracts.Patient;
using Patients.Api.Mappers;
using Patients.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Patients.Api.Controllers;

[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(GroupName = "Patients / PatientController")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost]
    [SwaggerOperation(
        OperationId = "AddNewPatient",
        Summary = "Добавить нового пациента",
        Description = "Создает новую запись о пациенте на основе переданных данных: идентификатора пользователя"
    )]
    public async Task<IActionResult> AddNewPatient([FromBody] AddNewPatientRequest request)
    {
        var result = await _patientService.AddPatient(request.UserId);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    [HttpPut]
    [SwaggerOperation(
        OperationId = "UpdatePatient",
        Summary = "Обновить данные пациента",
        Description = "Обновляет существующую запись о пациенте. Можно добавить или изменить Профиль крови"
    )]
    public async Task<IActionResult> UpdatePatient([FromBody] UpdatePatientRequest request)
    {
        var result = await _patientService.UpdatePatient(PatientMapper.FromUpdateCommand(request));
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        OperationId = "GetPatient",
        Summary = "Получить данные пациента по Id",
        Description = "Возвращает информацию о пациенте по его уникальному идентификатору (GUID)."
    )]
    public async Task<ActionResult<PatientResponse>> GetPatient([FromRoute] Guid id)
    {
        var patientResult = await _patientService.GetPatient(id);
        if (patientResult.IsFailure)
            return NotFound(patientResult.Error);

        var response = PatientMapper.FromDomain(patientResult.Value);
        return Ok(response);
    }
}