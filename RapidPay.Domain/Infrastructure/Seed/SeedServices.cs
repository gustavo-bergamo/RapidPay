using Microsoft.AspNetCore.Identity;
using RapidPay.Domain.Audit.Models;
using RapidPay.Domain.Authentication.Models;
using RapidPay.Domain.Infrastructure.Attributes;
using RapidPay.Domain.Infrastructure.Constants;
using RapidPay.Domain.Store.Models;
using System.Security.Claims;
using System.Text;

namespace RapidPay.Domain.Infrastructure.Seed;

[AddDependency]
public class SeedServices : ISeedServices
{
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

    public SeedServices(IPasswordHasher<ApplicationUser> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public IEnumerable<ApplicationUser> SeedUsers()
    {
        var admin = new ApplicationUser
        {
            Id = StaticValues.SEED_USER_ID,
            UserName = "demo@rapidpay.com",
            NormalizedUserName = "DEMO@RAPIDPAY.COM",
            Email = "demo@rapidpay.com",
            NormalizedEmail = "DEMO@RAPIDPAY.COM",
            EmailConfirmed = true,
            SecurityStamp = GetSecurityStamp(),
        };

        admin.PasswordHash = _passwordHasher.HashPassword(admin, "demo");

        return [admin];
    }

    public IEnumerable<Product> SeedProducts()
    {
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Lenovo Laptop", Description = "Levono Laptop", Price = 1799, Active = true },
            new Product { Id = 2, Name = "Macbook", Description = "Macbook", Price = 3199, Active = true },
            new Product { Id = 3, Name = "iPhone", Description = "iPhone", Price = 999, Active = true },
            new Product { Id = 4, Name = "iPhone charger", Description = "iPhone charger", Price = 49, Active = true },
        };

        FillAuditFields(products);

        return products;
    }

    public IEnumerable<ApplicationUserClaim> SeedClaims()
    {
        var name_identifier = new ApplicationUserClaim
        {
            Id = 1,
            ClaimType = ClaimTypes.NameIdentifier,
            ClaimValue = $"{StaticValues.SEED_USER_ID}",
            ApplicationUserId = StaticValues.SEED_USER_ID,
            UserId = StaticValues.SEED_USER_ID,
        };

        var role_manager = new ApplicationUserClaim
        {
            Id = 2,
            ClaimType = ClaimTypes.Role,
            ClaimValue = StaticValues.ROLE_MANAGER,
            ApplicationUserId = StaticValues.SEED_USER_ID,
            UserId = StaticValues.SEED_USER_ID,
        };

        var role_client = new ApplicationUserClaim
        {
            Id = 3,
            ClaimType = ClaimTypes.Role,
            ClaimValue = StaticValues.ROLE_CLIENT,
            ApplicationUserId = StaticValues.SEED_USER_ID,
            UserId = StaticValues.SEED_USER_ID,
        };

        return [name_identifier, role_manager, role_client];
    }

    public IEnumerable<Fee> SeedFees()
    {
        var initialFee = new Fee
        {
            Id = 1,
            Past = 1,
            Current = 1,
            ReferenceDate = DateTime.UtcNow
        };

        return [initialFee];
    }

    private void FillAuditFields<T>(List<T> auditEntities) where T : AuditEntity
    {
        var utcDate = DateTime.UtcNow;
        foreach (var entity in auditEntities)
        {
            entity.CreatedUser = StaticValues.SEED_USER_ID;
            entity.CreatedOnUtc = utcDate;
            entity.CreatedIPAddress = "0.0.0.1";
            entity.ModifiedUser = StaticValues.SEED_USER_ID;
            entity.ModifiedOnUtc = utcDate;
            entity.ModifiedIPAddress = "0.0.0.1";
        }
    }

    private string GetSecurityStamp()
    {
        var salt = DateTime.UtcNow.Ticks.ToString();
        var hashed = Convert.ToBase64String(Encoding.ASCII.GetBytes(salt));
        return hashed;
    }
}
