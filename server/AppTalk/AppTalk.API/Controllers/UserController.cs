using AppTalk.API.Managers;
using AppTalk.Core.Enums;
using AppTalk.Core.Extensions;
using AppTalk.Models.DataTransferObjects.Login;
using AppTalk.Models.DataTransferObjects.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Validator = AppTalk.Core.Validation.Validator;

namespace AppTalk.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(UserManager userManager) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResultDto>> Register(
        [FromServices] Validator validator,
        [FromBody] NewUserDto user)
    {
        var validationResult = validator.Validate(user);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult);
        }

        var result = await userManager.RegisterAsync(user);

        if (result == null)
        {
            return Conflict();
        }

        return Ok(result);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResultDto>> Login([FromBody] LoginDto loginDto)
    {
        var result = await userManager.LoginAsync(loginDto);

        if (result.ResultType == LoginResultType.InvalidPassword)
        {
            return Unauthorized();
        }

        if (result.ResultType == LoginResultType.UserNotFound)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("current")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<FullUserDto>> GetCurrentAsync()
    {
        var userId = User.GetUserId();

        if (userId == Guid.Empty)
        {
            return Forbid();
        }

        var result = await userManager.GetFullAsync(userId);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<UserDto>> GetAsync([FromRoute] Guid id)
    {
        var result = await userManager.GetAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}