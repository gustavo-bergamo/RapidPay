using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RapidPay.CrossProject.Exceptions;

namespace RapidPay.DependecyInjection.Errors;

internal sealed class UserFriendlyExceptionHandler : IExceptionHandler
{
    private readonly ILogger<UserFriendlyExceptionHandler> _logger;

    public UserFriendlyExceptionHandler(ILogger<UserFriendlyExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not UserFriendlyException userFriendlyException)
        {
            return false;
        }

        _logger.LogError(userFriendlyException, "Exception occurred: {Message}", userFriendlyException.Message);

        var problemDetails = new ProblemDetails
        {
            Title = "Error",
            Status = 0,
            Detail = userFriendlyException.Message
        };

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}