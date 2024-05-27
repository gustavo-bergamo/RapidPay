using RapidPay.Domain.Audit.Models;
using RapidPay.Domain.Payment.Models;
using System.ComponentModel.DataAnnotations;

namespace RapidPay.Domain.Store.Models;

public class Purchase : AuditEntity
{
    public int Id { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    [Required]
    public decimal Fee { get; set; }

    [Required]
    public int CreditCardId { get; set; }
    public CreditCard CreditCard { get; set; }

    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
