using System.Net;

namespace Library.Domain.Shared;

public class Result<TData> : Result
{
    private readonly TData? _data;

    protected internal Result(
        TData? data,
        bool isSuccess,
        Error error,
        HttpStatusCode statusCode)
        : base(isSuccess, error, statusCode) =>
        _data = data;

    public TData Data => _data is not null
        ? _data
        : throw new InvalidOperationException(
            "Can not get success data in a failure result");

    public static implicit operator Result<TData>(TData data) => Success(data);
}

public class Result
{
    protected internal Result(bool isSuccess, Error error, HttpStatusCode statusCode)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }
        IsSuccess = isSuccess;
        Error = error;
        StatusCode = statusCode;
    }

    public bool IsSuccess { get; }
    public Error Error { get; }
    public HttpStatusCode StatusCode { get; }
    public bool IsFailure => !IsSuccess;

    public static Result<T> Create<T>(T data) => Success(data);

    public static Result Success(HttpStatusCode statusCode = HttpStatusCode.NoContent) =>
        new(true, Error.None, statusCode);

    public static Result<T> Success<T>(
        T data,
        HttpStatusCode statusCode = HttpStatusCode.OK) =>
        new(data, true, Error.None, statusCode);

    public static Result Failure(Error error, HttpStatusCode statusCode) =>
        new(false, error, statusCode);

    public static Result<T> Failure<T>(Error error, HttpStatusCode statusCode) =>
        new(default, false, error, statusCode);
}
