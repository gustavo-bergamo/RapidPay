using Microsoft.AspNetCore.Identity;
using RapidPay.Domain.Payment.Models;

namespace RapidPay.Domain.Authentication.Models;

public class ApplicationUser : IdentityUser<int>
{
    public ICollection<CreditCard> CreditCards { get; set; }
    public ICollection<ApplicationUserClaim> UserClaims { get; set; }
}
