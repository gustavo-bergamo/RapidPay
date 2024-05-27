using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using RapidPay.Domain.Payment.Models;
using RapidPay.Domain.Payment.Services;

namespace RapidPay.Test
{
    internal class CreditCardServiceTest : BaseTest
    {
        CreditCardService _creditCardService;

        [SetUp]
        public void Setup()
        {
            var mockedMemoryCache = Substitute.For<IMemoryCache>();

            _creditCardService = new CreditCardService(mockedMemoryCache, applicationDbContext, userSesion);
        }

        [Test]
        public async Task OnCreateCreditCardAsync_When_HappyPath_Then_Success()
        {
            var creditCart = new CreditCard
            {
                CreditCardNumber = "4242424242424242",
                CreditCardExpiration = "12/30"
            };

            var createdCreditCard = await _creditCardService.CreateCreditCardAsync(creditCart);

            Assert.IsNotNull(createdCreditCard);
            Assert.IsTrue(createdCreditCard.Id > 0);
            Assert.IsTrue(createdCreditCard.CreditCardNumber == creditCart.CreditCardNumber);
        }

        [Test]
        public void OnCreateCreditCardAsync_When_NoData_Then_ThrowException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _creditCardService.CreateCreditCardAsync(new CreditCard()));
        }

        [Test]
        public async Task OnGetCreditCardByIdAsync_When_HappyPath_Then_Success()
        {
            var creditCart = new CreditCard
            {
                CreditCardNumber = "4242424242424242",
                CreditCardExpiration = "12/30"
            };

            var createdCreditCard = await _creditCardService.CreateCreditCardAsync(creditCart);

            var getCreditCard = await _creditCardService.GetCreditCardByIdAsync(createdCreditCard.Id);

            Assert.IsNotNull(getCreditCard);
            Assert.That(createdCreditCard.Id, Is.EqualTo(getCreditCard.Id));
        }

        [Test]
        public void OnGetCreditCardByIdAsync_When_InvalidId_Then_ThrowException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => _creditCardService.GetCreditCardByIdAsync(100));
        }

        [Test]
        public async Task OnGetCreditCardsAsync_When_HappyPath_Then_Success()
        {
            var creditCart = new CreditCard
            {
                CreditCardNumber = "4242424242424242",
                CreditCardExpiration = "12/30"
            };

            var createdCreditCard = await _creditCardService.CreateCreditCardAsync(creditCart);

            var creditCards = await _creditCardService.GetCreditCardsAsync(x => x.Id == createdCreditCard.Id);

            Assert.IsNotNull(creditCards);
            Assert.IsTrue(creditCards.Any(x => x.Id == createdCreditCard.Id));
        }

        [Test]
        public async Task OnGetCreditCardsAsync_When_GetUserCreditCard_Then_Success()
        {
            var creditCart1 = new CreditCard
            {
                CreditCardNumber = "4242424242424242",
                CreditCardExpiration = "12/30"
            };

            var creditCart2 = new CreditCard
            {
                CreditCardNumber = "5454545454545454",
                CreditCardExpiration = "12/30"
            };

            await _creditCardService.CreateCreditCardAsync(creditCart1);
            await _creditCardService.CreateCreditCardAsync(creditCart2);

            var creditCards = await _creditCardService.GetCreditCardsAsync(null);

            Assert.IsNotNull(creditCards);
            Assert.IsTrue(creditCards.Count() == 2);
        }

        [Test]
        public async Task OnUpdateCreditCardAsync_When_HappyPath_Then_Success()
        {
            var createdNumber = "4242424242424242";
            var updatedNumber = "5454545454545454";

            var creditCart = new CreditCard
            {
                CreditCardNumber = createdNumber,
                CreditCardExpiration = "12/30"
            };

            var createdCreditCard = await _creditCardService.CreateCreditCardAsync(creditCart);

            Assert.IsNotNull(createdCreditCard);

            var getCreditCard = await _creditCardService.GetCreditCardByIdAsync(createdCreditCard.Id);

            createdCreditCard.CreditCardNumber = updatedNumber;
            createdCreditCard.CreditCardExpiration = "12/31";

            var updatedCreditCard = await _creditCardService.UpdateCreditCardAsync(getCreditCard.Id, getCreditCard);

            Assert.IsNotNull(updatedCreditCard);
            Assert.That(getCreditCard.Id, Is.EqualTo(updatedCreditCard.Id));
            Assert.That(createdNumber, Is.Not.EqualTo(updatedCreditCard.CreditCardNumber));
        }
    }
}
