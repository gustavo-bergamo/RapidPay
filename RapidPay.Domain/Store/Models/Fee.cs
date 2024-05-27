namespace RapidPay.Domain.Store.Models;

public class Fee
{
    public int Id { get; set; }
    public decimal Past { get; set; }
    public decimal Current { get; set; }
    public DateTime ReferenceDate { get; set; }
}
