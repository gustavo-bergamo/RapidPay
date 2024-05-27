using RapidPay.DependencyInjection;
using RapidPay.Domain.Store.Models;

namespace RapidPay.API.Payment.Models;

public class MakePaymentResponse
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal Fee { get; set; }
    public GetCreditCardResponse CreditCard { get; set; }
    public GetProductResponse Product { get; set; }

    internal static IEnumerable<object> ConfigureMappings(AutomapperProfile profile)
    {
        yield return profile.CreateMap<Purchase, MakePaymentResponse>();
    }
}