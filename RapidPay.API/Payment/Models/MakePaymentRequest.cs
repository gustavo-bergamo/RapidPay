using System.ComponentModel.DataAnnotations;

namespace RapidPay.API.Payment.Models;

public class MakePaymentRequest
{

    [Required]
    public int ProductId { get; set; }


    [Required]
    public int Quantity { get; set; }


    [Required]
    public int CreditCardId { get; set; }
}
