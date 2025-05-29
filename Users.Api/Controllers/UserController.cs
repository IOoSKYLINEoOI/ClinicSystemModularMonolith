using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.Api.Contracts.User;
using Users.Core.Enums;
using Users.Core.Interfaces.Services;
using Users.Core.ValueObjects;

namespace Users.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
    {
        var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber, true, true, false);
        if (phoneNumberResult.IsFailure)
            return BadRequest(phoneNumberResult.Error);

        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return BadRequest(emailResult.Error);


        var result = await _userService.Register(
            request.FirstName,
            request.LastName,
            request.FatherName,
            request.DateOfBirth,
            request.Gender,
            emailResult.Value,
            phoneNumberResult.Value,
            request.Password);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginUserRequest request)
    {
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return BadRequest(new { error = emailResult.Error });

        var token = await _userService.Login(emailResult.Value, request.Password);

        if (token.IsFailure)
        {
            return BadRequest(token.Error);
        }

        Response.Cookies.Append("secretCookie", token.Value, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(2)
        });

        return Ok(new { message = "Login successful" });
    }

    [Authorize]
    [HttpPut("updateProfile")]
    public async Task<ActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber, true, true, false);
        if (phoneNumberResult.IsFailure)
            return BadRequest(phoneNumberResult.Error);

        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return BadRequest(emailResult.Error);

        var result = await _userService.Update(
            userId,
            request.FirstName,
            request.LastName,
            request.FatherName,
            request.DateOfBirth,
            emailResult.Value,
            phoneNumberResult.Value);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpPost("logout")]
    public IActionResult LogoutUser()
    {
        Response.Cookies.Delete("secretCookie");
        return Ok();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserResponse>> GetUserProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var result = await _userService.GetByUserId(userId);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        var user = result.Value;

        var response = new UserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.FatherName,
            user.DateOfBirth,
            user.Gender,
            user.Email.Value,
            user.PhoneNumber.Value);

        return Ok(response);
    }
    
    [Authorize]
    [HttpPut("changePassword")]
    public async Task<ActionResult<UserResponse>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();
        
        var result = await _userService.ChangePassword(userId, request.currentPassword, request.newPassword);
        
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
}
