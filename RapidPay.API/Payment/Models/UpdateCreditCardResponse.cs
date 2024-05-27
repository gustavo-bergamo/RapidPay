using RapidPay.API.Shared.Models;
using RapidPay.DependencyInjection;
using RapidPay.Domain.Payment.Constants;
using RapidPay.Domain.Payment.Models;

namespace RapidPay.API.Payment.Models;

public class UpdateCreditCardResponse
{
    public int Id { get; set; }
    public string CreditCardNumber { get; set; }
    public string CreditCardExpiration { get; set; }
    public CreditCardBrand CreditCardBrand { get; set; }
    public ApplicationUserResponse ApplicationUser { get; set; }

    internal static IEnumerable<object> ConfigureMappings(AutomapperProfile profile)
    {
        yield return profile.CreateMap<CreditCard, UpdateCreditCardResponse>();
    }
}
