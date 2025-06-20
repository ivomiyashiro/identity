using SharedKernel.Result;

namespace API.Extensions;

public static class ResultExtensions
{
    public static IResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        var extensions = new Dictionary<string, object?>();
        var errors = ParseErrors(result.Error.Type, result.Error.Message);

        if (errors != null)
        {
            extensions.Add("errors", errors);
        }

        return Results.Problem(
            type: GetType(result.Error.Type),
            title: GetTitle(result.Error.Type),
            statusCode: GetStatusCode(result.Error.Type),
            detail: GetDetail(result.Error.Type, result.Error.Message),
            extensions: extensions.Count != 0 ? extensions : null
        );
    }

    public static IResult ToProblemDetails<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        var extensions = new Dictionary<string, object?>();
        var errors = ParseErrors(result.Error.Type, result.Error.Message);

        if (errors != null)
        {
            extensions.Add("errors", errors);
        }

        return Results.Problem(
            type: GetType(result.Error.Type),
            title: GetTitle(result.Error.Type),
            statusCode: GetStatusCode(result.Error.Type),
            detail: GetDetail(result.Error.Type, result.Error.Message),
            extensions: extensions.Count != 0 ? extensions : null
        );
    }

    private static int GetStatusCode(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static string GetDetail(ErrorType errorType, string message)
    {
        return errorType switch
        {
            ErrorType.Validation => "Your request parameters didn't validate",
            _ => message
        };
    }

    private static string GetTitle(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => "Validation Error",
            ErrorType.NotFound => "Resource Not Found",
            ErrorType.Unauthorized => "Unauthorized Access",
            ErrorType.Forbidden => "Forbidden Access",
            ErrorType.Conflict => "Conflict Detected",
            _ => "Internal Server Error"
        };
    }

    private static string GetType(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
            ErrorType.Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };
    }

    public static Dictionary<string, List<string>>? ParseErrors(ErrorType errorType, string errorString)
    {
        // Only return errors for validation error types
        if (errorType != ErrorType.Validation)
            return null;

        var errors = new Dictionary<string, List<string>>();
        var parts = errorString.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            var trimmed = part.Trim();
            var colonIndex = trimmed.IndexOf(':');
            if (colonIndex > 0)
            {
                var field = trimmed.Substring(0, colonIndex).Trim().ToLowerInvariant();
                var errorMessage = trimmed.Substring(colonIndex + 1).Trim();

                if (!errors.ContainsKey(field))
                    errors[field] = [];

                errors[field].Add(errorMessage);
            }
        }

        return errors.Count != 0 ? errors : null;
    }
}