using RapidPay.DependencyInjection;
using RapidPay.Domain.Store.Models;

namespace RapidPay.API.Payment.Models;

public class GetProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; }

    internal static IEnumerable<object> ConfigureMappings(AutomapperProfile profile)
    {
        yield return profile.CreateMap<Product, GetProductResponse>();
    }
}
