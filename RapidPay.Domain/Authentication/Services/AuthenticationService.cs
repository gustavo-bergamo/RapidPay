using RapidPay.CrossProject.Exceptions;
using RapidPay.Domain.Authentication.Models;
using RapidPay.Domain.Data;
using RapidPay.Domain.Infrastructure.Attributes;

namespace RapidPay.Domain.Authentication.Services;

[AddDependency]
public class AuthenticationService : IAuthenticationService
{
    private readonly IApplicationDbContext _context;
    private readonly ApplicationUserManager _applicationUserManager;

    public AuthenticationService(IApplicationDbContext context, ApplicationUserManager applicationUserManager)
    {
        _context = context;
        _applicationUserManager = applicationUserManager;
    }

    public async Task<(bool authenticated, ApplicationUser user)> TryAuthenticateAsync(string username, string password)
    {
        var user = _context.Users.SingleOrDefault(x => x.UserName == username);
        if (user == null)
        {
            throw new UserFriendlyException("Username not found");
        }

        var authenticated = await _applicationUserManager.CheckPasswordAsync(user, password);

        return (authenticated, user);
    }
}
