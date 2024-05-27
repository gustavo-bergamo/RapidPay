using Microsoft.Extensions.DependencyInjection;

namespace RapidPay.DependecyInjection.Errors;

internal static class Errors
{
    public static void AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<UserFriendlyExceptionHandler>();

        services.AddProblemDetails();
    }
}
