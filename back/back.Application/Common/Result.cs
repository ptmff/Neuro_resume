namespace back.Application.Common;

public class Result
{
    public string Error { get; }
    public bool IsSuccess { get; }

    protected Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true, null);
    public static Result Failure(string error) => new Result(false, error);

    /// <summary>
    /// Обрабатывает результат, вызывая success или failure-функцию в зависимости от состояния.
    /// </summary>
    public TResult Match<TResult>(Func<TResult> success, Func<string, TResult> failure)
        => IsSuccess ? success() : failure(Error);
}

public class Result<T> : Result
{
    public T Value { get; }

    private Result(T value) : base(true, null)
    {
        Value = value;
    }

    private Result(string error) : base(false, error)
    {
    }

    public static Result<T> Success(T value) => new Result<T>(value);
    public static new Result<T> Failure(string error) => new Result<T>(error);

    /// <summary>
    /// Обрабатывает результат, вызывая success или failure-функцию в зависимости от состояния.
    /// </summary>
    public TResult Match<TResult>(Func<T, TResult> success, Func<string, TResult> failure)
        => IsSuccess ? success(Value) : failure(Error);
}