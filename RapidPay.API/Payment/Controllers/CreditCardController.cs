using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidPay.API.Payment.Models;
using RapidPay.Domain.Payment.Models;
using RapidPay.Domain.Payment.Services;

namespace RapidPay.API.Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Client")]
    public class CreditCardController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICreditCardService _creditCardService;

        public CreditCardController(IMapper mapper, ICreditCardService creditCardService)
        {
            _mapper = mapper;
            _creditCardService = creditCardService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var creditCard = await _creditCardService.GetCreditCardByIdAsync(id);
            return Ok(_mapper.Map<GetCreditCardResponse>(creditCard));
        }

        [HttpGet("{id}/getbalance")]
        [Authorize(Policy = "Manager")]
        public IActionResult GetBalance(int id)
        {
            var creditCardBalance = _creditCardService.GetCreditCardBalance(id);
            return Ok(new GetCreditCardBalanceResponse(creditCardBalance));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCreditCardRequest createCreditCardRequest)
        {
            var creditCard = _mapper.Map<CreditCard>(createCreditCardRequest);
            var createdCreditCard = await _creditCardService.CreateCreditCardAsync(creditCard);
            return Ok(_mapper.Map<CreateCreditCardResponse>(createdCreditCard));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateCreditCardRequest updateCreditCardRequest)
        {
            var creditCard = _mapper.Map<CreditCard>(updateCreditCardRequest);
            var updatedCreditCard = await _creditCardService.UpdateCreditCardAsync(id, creditCard);
            return Ok(_mapper.Map<CreateCreditCardResponse>(updatedCreditCard));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _creditCardService.DeleteCreditCardByIdAsync(id);
            return Ok();
        }
    }
}
