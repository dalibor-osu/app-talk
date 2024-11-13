using System.Diagnostics;
using AppTalk.Core.Enums;
using AppTalk.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppTalk.Core.Base;

public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    protected IActionResult CreateActionResultResponse<T>(OptionalResponse<T> optionalResponse)
    {
        if (optionalResponse.IsSuccess)
        {
            return Ok(optionalResponse.Value);
        }

        if (optionalResponse.IsValidationError)
        {
            return BadRequest(optionalResponse.ValidationResult);
        }

        switch (optionalResponse.Type)
        {
            case OptionalErrorType.NotFound:
                return NotFound(optionalResponse.GetErrorMessageWrapper());
            case OptionalErrorType.AlreadyExists:
                return Conflict(optionalResponse.GetErrorMessageWrapper());
            case OptionalErrorType.BadRequest:
                return BadRequest(optionalResponse.GetErrorMessageWrapper());
            case OptionalErrorType.ServiceError:
#if DEBUG
                return StatusCode(500, optionalResponse.GetErrorMessageWrapper());
#else
                return StatusCode(500);
#endif
            case OptionalErrorType.Invalid:
            case OptionalErrorType.ValidationError:
            default:
                throw new UnreachableException("CreateActionResult reached unreachable code.");
        }
    }
}