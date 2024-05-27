using RapidPay.Domain.Infrastructure.Constants;

namespace RapidPay.Domain.Infrastructure.Attributes;

public class AddDependencyAttribute : Attribute
{
    public Type Interface { get; set; }
    public DependencyType Type { get; set; } = DependencyType.Scoped;
}
