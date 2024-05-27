namespace RapidPay.API.Payment.Models
{
    public class GetCreditCardBalanceResponse
    {
        public GetCreditCardBalanceResponse(decimal balance)
        {
            Balance = balance;
        }
        public decimal Balance { get; private set; }
    }
}
