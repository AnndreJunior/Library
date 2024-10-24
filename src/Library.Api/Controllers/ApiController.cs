using Library.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected readonly ISender _sender;

    protected ApiController(ISender sender)
    {
        _sender = sender;
    }

    protected IActionResult HandleFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error) =>
        new()
        {
            Title = title,
            Type = error.Key,
            Detail = error.Message,
            Status = status
        };
}
