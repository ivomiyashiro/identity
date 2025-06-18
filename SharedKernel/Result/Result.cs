namespace SharedKernel.Result;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T Value { get; }
    public Error Error { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Error = Error.None;
    }

    private Result(Error error)
    {
        IsSuccess = false;
        Value = default!;
        Error = error;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(Error error) => new(error);

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure) =>
        IsSuccess ? onSuccess(Value) : onFailure(Error);

    public async Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFailure) =>
        IsSuccess ? await onSuccess(Value) : await onFailure(Error);
}

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    private Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    public static Result<T> Success<T>(T value) => Result<T>.Success(value);
    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);

    public static implicit operator Result(Error error) => Failure(error);

    public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure) =>
        IsSuccess ? onSuccess() : onFailure(Error);

    public async Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFailure) =>
        IsSuccess ? await onSuccess() : await onFailure(Error);
}