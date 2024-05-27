using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidPay.API.Payment.Models;
using RapidPay.Domain.Store.Services;

namespace RapidPay.API.Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Client")]
    public class PaymentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public PaymentController(IMapper mapper, IPaymentService paymentService)
        {
            _mapper = mapper;
            _paymentService = paymentService;
        }

        [HttpPost("makeapayment")]
        public async Task<IActionResult> MakePaymentAsync([FromBody] MakePaymentRequest makePaymentRequest)
        {
            var purchase = await _paymentService.MakePayment(makePaymentRequest.ProductId, makePaymentRequest.Quantity, makePaymentRequest.CreditCardId);
            return Ok(_mapper.Map<MakePaymentResponse>(purchase));
        }
    }
}
