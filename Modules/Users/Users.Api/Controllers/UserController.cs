using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Users.Api.Contracts.User;
using Users.Core.Interfaces.Services;
using Users.Core.ValueObjects;

namespace Users.Api.Controllers;

[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(GroupName = "Users / UserController")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [SwaggerOperation(
        OperationId = "RegisterUser",
        Summary = "Регистрация нового пользователя",
        Description = "Создает нового пользователя с указанием личных данных, адреса электронной почты, номера телефона и пароля. Выполняет валидацию введённых данных и сохраняет пользователя в системе."
    )]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
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
    [SwaggerOperation(
        OperationId = "LoginUser",
        Summary = "Авторизация пользователя",
        Description = "Проверяет логин и пароль пользователя. В случае успеха устанавливает HTTP-only cookie с JWT токеном для последующих запросов."
    )]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request)
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
    [SwaggerOperation(
        OperationId = "UpdateUser",
        Summary = "Обновить данные пользователя",
        Description = "Позволяет текущему авторизованному пользователю обновить свои личные данные, включая имя, email и номер телефона. Выполняется валидация данных."
    )]
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
    [SwaggerOperation(
        OperationId = "LogoutUser",
        Summary = "Выход из системы",
        Description = "Удаляет cookie с JWT токеном и завершает сеанс текущего пользователя."
    )]
    public IActionResult LogoutUser()
    {
        Response.Cookies.Delete("secretCookie");
        return Ok();
    }

    [Authorize]
    [HttpGet("me")]
    [SwaggerOperation(
        OperationId = "GetUserProfile",
        Summary = "Получить данные текущего пользователя",
        Description = "Возвращает профиль текущего авторизованного пользователя по ID, извлечённому из JWT токена."
    )]
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
    [SwaggerOperation(
        OperationId = "ChangePassword",
        Summary = "Смена пароля",
        Description = "Позволяет авторизованному пользователю изменить свой пароль. Для изменения требуется указать текущий и новый пароль."
    )]
    public async Task<ActionResult<UserResponse>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();
        
        var result = await _userService.ChangePassword(userId, request.CurrentPassword, request.NewPassword);
        
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
}
