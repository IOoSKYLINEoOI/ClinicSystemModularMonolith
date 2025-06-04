using Employees.Api.Contracts.Certificate;
using Employees.Api.Contracts.Position;
using Employees.Core.Interfaces.Services;
using Employees.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

[ApiController]
[Route("position")]
public class PositionController : ControllerBase
{
    private readonly IPositionService _positionService;

    public PositionController(IPositionService positionService)
    {
        _positionService = positionService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PositionResponse>> GetPositionById(int id)
    {
        var positionResult = await _positionService.GetPositionById(id);
        if(positionResult.IsFailure)
            return NotFound(positionResult.Error);
        
        var response = new PositionResponse(positionResult.Value.Name);
        
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddNewPosition([FromBody] AddNewPositionRequest request)
    {
        var result = await _positionService.AddPosition(request.Name);
        
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<PositionResponse>>> GetAllPositions()
    {
        var positionResult = await _positionService.GetAllPositions();
        if(positionResult.IsFailure)
            return NotFound(positionResult.Error);

        var response = new List<PositionResponse>();

        foreach (var position in positionResult.Value)
        {
            var res = new PositionResponse(position.Name);
            
            response.Add(res);
        }
        return Ok(response);
    }
}