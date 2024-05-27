using RapidPay.DependencyInjection;
using RapidPay.Domain.Payment.Models;

namespace RapidPay.API.Payment.Models;

public class UpdateCreditCardRequest
{
    public int Id { get; set; }
    public string CreditCardNumber { get; set; }
    public string CreditCardExpiration { get; set; }

    internal static IEnumerable<object> ConfigureMappings(AutomapperProfile profile)
    {
        yield return profile.CreateMap<UpdateCreditCardRequest, CreditCard>();
    }
}


