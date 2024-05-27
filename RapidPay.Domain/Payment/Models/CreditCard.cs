using RapidPay.Domain.Audit.Models;
using RapidPay.Domain.Authentication.Models;
using RapidPay.Domain.Payment.Constants;
using RapidPay.Domain.Store.Models;
using System.ComponentModel.DataAnnotations;

namespace RapidPay.Domain.Payment.Models;

public class CreditCard : AuditEntity
{
    public int Id { get; set; }

    [Required]
    public string CreditCardNumber { get; set; }

    [Required]
    public CreditCardBrand CreditCardBrand { get; set; }

    [Required]
    public string CreditCardExpiration { get; set; }

    [Required]
    public int ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public ICollection<Purchase> Purchases { get; set; }
}
