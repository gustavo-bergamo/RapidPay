using RapidPay.Domain.Infrastructure.Services;

namespace RapidPay.Domain.Store.Models;

internal sealed record BalanceUpdateDomainEvent(int CreditCardId, decimal Amount) : IDomainEvent { }