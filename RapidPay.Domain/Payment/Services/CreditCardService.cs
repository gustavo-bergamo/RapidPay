using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RapidPay.CrossProject.Tools;
using RapidPay.Domain.Authentication.Services;
using RapidPay.Domain.Data;
using RapidPay.Domain.Infrastructure.Attributes;
using RapidPay.Domain.Payment.Constants;
using RapidPay.Domain.Payment.Models;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace RapidPay.Domain.Payment.Services;

[AddDependency]
public class CreditCardService : ICreditCardService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IApplicationDbContext _context;
    private readonly IUserSession _userSession;

    public CreditCardService(IMemoryCache memoryCache, IApplicationDbContext context, IUserSession userSession)
    {
        _memoryCache = memoryCache;
        _context = context;
        _userSession = userSession;
    }

    public async Task<CreditCard> CreateCreditCardAsync(CreditCard creditCard)
    {
        creditCard.ApplicationUserId = _userSession.UserId;
        creditCard.CreditCardBrand = GetCreditCardBrand(creditCard.CreditCardNumber);

        await _context.CreditCards.AddAsync(creditCard);
        await _context.SaveChangesAsync();

        creditCard.CreditCardNumber = CreditCardMask.Mask(creditCard.CreditCardNumber);

        return creditCard;
    }

    public async Task<CreditCard> UpdateCreditCardAsync(int id, CreditCard creditCard)
    {
        var existingEntry = await _context.CreditCards.SingleAsync(x => x.Id == id);

        existingEntry.CreditCardNumber = creditCard.CreditCardNumber;
        existingEntry.CreditCardBrand = GetCreditCardBrand(creditCard.CreditCardNumber);
        existingEntry.CreditCardExpiration = creditCard.CreditCardExpiration;

        await _context.SaveChangesAsync();

        existingEntry.CreditCardNumber = CreditCardMask.Mask(existingEntry.CreditCardNumber);

        return existingEntry;
    }

    public async Task<IEnumerable<CreditCard>> GetCreditCardsAsync(Expression<Func<CreditCard, bool>>? expression)
    {
        var query = _context.CreditCards
            .Include(x => x.ApplicationUser)
            .Where(x => x.ApplicationUserId == _userSession.UserId);

        if (expression != null)
        {
            query = query.Where(expression);
        }

        return await query.ToListAsync();
    }

    public async Task DeleteCreditCardByIdAsync(int id)
    {
        var existingEntry = await _context.CreditCards
            .Include(x => x.ApplicationUser)
            .SingleAsync(x => x.Id == id);
        _context.CreditCards.Remove(existingEntry);
        await _context.SaveChangesAsync();
    }

    public async Task<CreditCard> GetCreditCardByIdAsync(int id)
    {
        return await _context.CreditCards
            .Include(x => x.ApplicationUser)
            .SingleAsync(x => x.ApplicationUserId == _userSession.UserId && x.Id == id);
    }

    public decimal GetCreditCardBalance(int id)
    {
        if (!_memoryCache.TryGetValue($"balance_{id}", out decimal balance))
        {
            return 0;
        }

        return balance;
    }

    private CreditCardBrand GetCreditCardBrand(string creditCardNumber)
    {
        return new Regex(@"^4[0-9]{6,}$").IsMatch(creditCardNumber) ? CreditCardBrand.Visa :
               new Regex(@"^5[1-5][0-9]{5,}|222[1-9][0-9]{3,}|22[3-9][0-9]{4,}|2[3-6][0-9]{5,}|27[01][0-9]{4,}|2720[0-9]{3,}$").IsMatch(creditCardNumber) ? CreditCardBrand.MasterCard :
               new Regex(@"^3[47][0-9]{5,}$").IsMatch(creditCardNumber) ? CreditCardBrand.AmericanExpress :
               new Regex(@"^65[4-9][0-9]{13}|64[4-9][0-9]{13}|6011[0-9]{12}|(622(?:12[6-9]|1[3-9][0-9]|[2-8][0-9][0-9]|9[01][0-9]|92[0-5])[0-9]{10})$").IsMatch(creditCardNumber) ? CreditCardBrand.Discover :
               new Regex(@"^3[47][0-9]{13}$").IsMatch(creditCardNumber) ? CreditCardBrand.Amex :
               new Regex(@"^(6541|6556)[0-9]{12}$").IsMatch(creditCardNumber) ? CreditCardBrand.BCGlobal :
               new Regex(@"^389[0-9]{11}$").IsMatch(creditCardNumber) ? CreditCardBrand.CarteBlanch :
               new Regex(@"^3(?:0[0-5]|[68][0-9])[0-9]{11}$").IsMatch(creditCardNumber) ? CreditCardBrand.DinersClub :
               new Regex(@"^63[7-9][0-9]{13}$").IsMatch(creditCardNumber) ? CreditCardBrand.InstaPaymentCard :
               new Regex(@"^(?:2131|1800|35\d{3})\d{11}$").IsMatch(creditCardNumber) ? CreditCardBrand.JCBCard :
               new Regex(@"^9[0-9]{15}$").IsMatch(creditCardNumber) ? CreditCardBrand.KoreanLocal :
               new Regex(@"^(6304|6706|6709|6771)[0-9]{12,15}$").IsMatch(creditCardNumber) ? CreditCardBrand.LaserCard :
               new Regex(@"^(5018|5020|5038|6304|6759|6761|6763)[0-9]{8,15}$").IsMatch(creditCardNumber) ? CreditCardBrand.Maestro :
               new Regex(@"^(6334|6767)[0-9]{12}|(6334|6767)[0-9]{14}|(6334|6767)[0-9]{15}$").IsMatch(creditCardNumber) ? CreditCardBrand.Solo :
               new Regex(@"^(4903|4905|4911|4936|6333|6759)[0-9]{12}|(4903|4905|4911|4936|6333|6759)[0-9]{14}|(4903|4905|4911|4936|6333|6759)[0-9]{15}|564182[0-9]{10}|564182[0-9]{12}|564182[0-9]{13}|633110[0-9]{10}|633110[0-9]{12}|633110[0-9]{13}$").IsMatch(creditCardNumber) ? CreditCardBrand.SwitchCard :
               new Regex(@"^(62[0-9]{14,17})$").IsMatch(creditCardNumber) ? CreditCardBrand.UnionPay :
               creditCardNumber.Where((e) => e >= '0' && e <= '9').Reverse().Select((e, i) => (e - 48) * (i % 2 == 0 ? 1 : 2)).Sum((e) => e / 10 + e % 10) == 0 ? CreditCardBrand.NotFormatted : CreditCardBrand.Unknown;
    }


}
