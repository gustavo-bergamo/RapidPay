using RapidPay.Domain.Authentication.Models;
using RapidPay.Domain.Store.Models;

namespace RapidPay.Domain.Infrastructure.Seed
{
    public interface ISeedServices
    {
        IEnumerable<Product> SeedProducts();
        IEnumerable<ApplicationUser> SeedUsers();
        IEnumerable<ApplicationUserClaim> SeedClaims();
        IEnumerable<Fee> SeedFees();
    }
}