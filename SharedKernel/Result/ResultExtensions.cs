namespace SharedKernel.Result;

public static class ResultExtensions
{
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> map) =>
        result.IsSuccess ? Result<TOut>.Success(map(result.Value)) : Result<TOut>.Failure(result.Error);

    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> map) =>
        result.IsSuccess ? Result<TOut>.Success(await map(result.Value)) : Result<TOut>.Failure(result.Error);

    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> bind) =>
        result.IsSuccess ? bind(result.Value) : Result<TOut>.Failure(result.Error);

    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> bind) =>
        result.IsSuccess ? await bind(result.Value) : Result<TOut>.Failure(result.Error);

    public static Result<T> Tap<T>(this Result<T> result, Action<T> action)
    {
        if (result.IsSuccess)
            action(result.Value);
        return result;
    }

    public static async Task<Result<T>> TapAsync<T>(this Result<T> result, Func<T, Task> action)
    {
        if (result.IsSuccess)
            await action(result.Value);
        return result;
    }

    public static Result<T> TapError<T>(this Result<T> result, Action<Error> action)
    {
        if (result.IsFailure)
            action(result.Error);
        return result;
    }

    public static async Task<Result<T>> TapErrorAsync<T>(this Result<T> result, Func<Error, Task> action)
    {
        if (result.IsFailure)
            await action(result.Error);
        return result;
    }
}