using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RapidPay.Domain.Authentication.Models;
using RapidPay.Domain.Infrastructure.Attributes;

namespace RapidPay.Domain.Authentication.Services;

[AddDependency(Interface = typeof(ApplicationUserManager))]
public class ApplicationUserManager : UserManager<ApplicationUser>
{
    public ApplicationUserManager(IUserStore<ApplicationUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<ApplicationUser> passwordHasher,
        IEnumerable<IUserValidator<ApplicationUser>> userValidators,
        IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string newPassword)
    {
        var user = await FindByIdAsync(userId.ToString());
        user.PasswordHash = PasswordHasher.HashPassword(user, newPassword);
        var updateResponse = await UpdateAsync(user);
        return updateResponse.Succeeded;
    }

    public string Hash(ApplicationUser user, string password)
    {
        return PasswordHasher.HashPassword(user, password);
    }
}
