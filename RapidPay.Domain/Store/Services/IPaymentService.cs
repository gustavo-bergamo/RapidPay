using RapidPay.Domain.Store.Models;

namespace RapidPay.Domain.Store.Services
{
    public interface IPaymentService
    {
        Task<Purchase> MakePayment(int productId, int quantity, int creditCardId);
    }
}