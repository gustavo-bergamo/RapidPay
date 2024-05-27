namespace RapidPay.API.Middlewares;

public class ErrorMiddleware
{
    private readonly ILogger<ErrorMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ErrorMiddleware(ILogger<ErrorMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
