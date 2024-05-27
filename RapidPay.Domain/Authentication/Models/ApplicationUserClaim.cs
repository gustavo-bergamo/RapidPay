using Microsoft.AspNetCore.Identity;

namespace RapidPay.Domain.Authentication.Models;

public class ApplicationUserClaim : IdentityUserClaim<int>
{
    public int ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}
