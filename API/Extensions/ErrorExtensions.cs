using Microsoft.AspNetCore.Mvc;
using SharedKernel.Result;

namespace API.Extensions;

public static class ErrorExtensions
{
    public static IActionResult ToHttpResponse(this Error error)
    {
        if (error == Error.None)
        {
            return new OkResult();
        }

        return error.Type switch
        {
            ErrorType.NotFound => new NotFoundObjectResult(error),
            ErrorType.Validation => new BadRequestObjectResult(error),
            ErrorType.Conflict => new ConflictObjectResult(error),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(error),
            ErrorType.Forbidden => new ObjectResult(error) { StatusCode = StatusCodes.Status403Forbidden },
            ErrorType.Failure => new ObjectResult(error) { StatusCode = StatusCodes.Status500InternalServerError },
            _ => new ObjectResult(error) { StatusCode = StatusCodes.Status500InternalServerError }
        };
    }
}
