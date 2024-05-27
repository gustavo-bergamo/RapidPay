using RapidPay.Domain.Authentication.Services;
using System.Security.Claims;

namespace RapidPay.API.Middlewares;

public class UserMiddleware
{
    private readonly RequestDelegate _next;

    public UserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext.User.Identity is not null and { IsAuthenticated: true })
        {
            var userSession = (IUserSession)httpContext.RequestServices.GetService(typeof(IUserSession))!;

            var userIdContext = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)!;
            userSession.SetUserId(int.Parse(userIdContext.Value));
            userSession.SetIPAddress(httpContext.Request.HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString());
        }

        await _next(httpContext);
    }
}