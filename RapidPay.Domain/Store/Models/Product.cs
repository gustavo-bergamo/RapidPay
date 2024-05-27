using RapidPay.Domain.Audit.Models;

namespace RapidPay.Domain.Store.Models;

public class Product : AuditEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool Active { get; set; } = true;

    public ICollection<Purchase> Purchases { get; set; }
}
