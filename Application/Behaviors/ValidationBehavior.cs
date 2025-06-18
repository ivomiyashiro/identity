using FluentValidation;
using MediatR;
using SharedKernel.Result;

namespace Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Count > 0)
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Count > 0)
        {
            var errorMessage = string.Join("; ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));
            var validationError = Error.Validation("Validation.Failed", errorMessage);

            // If TResponse is a Result type, return a failure result
            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = typeof(TResponse).GetGenericArguments()[0];
                var failureMethod = typeof(Result<>).MakeGenericType(resultType).GetMethod("Failure", new[] { typeof(Error) });
                return (TResponse)failureMethod!.Invoke(null, new object[] { validationError })!;
            }

            if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Failure(validationError);
            }

            throw new ValidationException(failures);
        }

        return await next();
    }
}