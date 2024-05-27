using MediatR;
using Microsoft.EntityFrameworkCore;
using RapidPay.Domain.Data;
using RapidPay.Domain.Infrastructure.Attributes;
using RapidPay.Domain.Store.Models;

namespace RapidPay.Domain.Store.Services;

[AddDependency]
public class PaymentService : IPaymentService
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly IFeeService _feeService;

    public PaymentService(IApplicationDbContext context, IMediator mediator, IFeeService feeService)
    {
        _context = context;
        _mediator = mediator;
        _feeService = feeService;
    }

    public async Task<Purchase> MakePayment(int productId, int quantity, int creditCardId)
    {
        var product = await _context.Products.SingleAsync(x => x.Id == productId && x.Active);
        var creditCard = await _context.CreditCards.SingleAsync(x => x.Id == creditCardId);

        var feeAmount = _feeService.GetFee();

        var purchase = new Purchase
        {
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = product.Price,
            TotalPrice = product.Price * quantity + feeAmount,
            Fee = feeAmount,
            CreditCardId = creditCardId
        };

        await _context.Purchases.AddAsync(purchase);
        await _context.SaveChangesAsync();

        await _mediator.Publish(new BalanceUpdateDomainEvent(creditCard.Id, purchase.TotalPrice));

        return purchase;
    }
}
