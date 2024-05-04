using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> logger;
        private const string _webhooksecret = "";

        public PaymentsController(IPaymentService paymentService,ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            this.logger = logger;
        }

        [HttpPost("{basketId}")] //Post: /api/Payments?id=basketId
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntnet(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null)
                return BadRequest(new ApiErrorResponse(400, "A problem with your Basket"));

            return Ok(basket);
        }
        [HttpPost("webhook")]
        public async Task<IActionResult> stripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Strip-Signature"], _webhooksecret);
            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    await _paymentService.UpdatePaymentIntentToSucceedOrFailed(paymentIntent.Id, true);
                    logger.LogInformation("Payment Succeeded",paymentIntent.Id);
                    break;
                case Events.PaymentIntentPaymentFailed:
                    await _paymentService.UpdatePaymentIntentToSucceedOrFailed(paymentIntent.Id, false);
                    logger.LogInformation("Payment Failed", paymentIntent.Id);
                    break;
            }
            return Ok();
        }
    }
}
