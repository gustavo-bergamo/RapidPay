
using RapidPay.Domain.Authentication.Models;

namespace RapidPay.Domain.Authentication.Services
{
    public interface IAuthenticationService
    {
        Task<(bool authenticated, ApplicationUser user)> TryAuthenticateAsync(string username, string password);
    }
}