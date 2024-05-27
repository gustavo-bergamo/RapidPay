using RapidPay.Domain.Payment.Models;
using System.Linq.Expressions;

namespace RapidPay.Domain.Payment.Services
{
    public interface ICreditCardService
    {
        Task<CreditCard> CreateCreditCardAsync(CreditCard creditCard);
        Task<CreditCard> GetCreditCardByIdAsync(int id);
        decimal GetCreditCardBalance(int id);
        Task<IEnumerable<CreditCard>> GetCreditCardsAsync(Expression<Func<CreditCard, bool>>? expression);
        Task<CreditCard> UpdateCreditCardAsync(int id, CreditCard creditCard);
        Task DeleteCreditCardByIdAsync(int id);
    }
}