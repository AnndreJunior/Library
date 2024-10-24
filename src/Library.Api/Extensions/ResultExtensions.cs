using Library.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Extensions;

public static class ResultExtensions
{
    public static async Task<Result> Bind<TIn>(
        this Result<TIn> result,
        Func<TIn, Task<Result>> func) =>
        await func(result.Data);

    public static async Task<Result<TOut>> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Task<Result<TOut>>> func) =>
        await func(result.Data);

    public static async Task<IActionResult> Match(
        this Task<Result> resultTask,
        Func<IActionResult> onSuccess,
        Func<Result, IActionResult> onFailure)
    {
        var result = await resultTask;
        return result.IsSuccess ? onSuccess() : onFailure(result);
    }

    public static async Task<IActionResult> Match<TIn>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, IActionResult> onSuccess,
        Func<Result, IActionResult> onFailure)
    {
        var result = await resultTask;
        return result.IsSuccess ? onSuccess(result.Data) : onFailure(result);
    }
}
