using APIsMainProject.ResponseModule;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace APIsMainProject.Controllers
{
    public class PaymentsController : BaseController
    {
        private readonly IPaymentService paymentService;
        private readonly ILogger<PaymentsController> logger;
        private const string WebHookSecret = "whsec_74de6a645bde4158438d88e6e83b9f763b63f45992d304b8f3def4daf56e5d2d";

        public PaymentsController
                                (
                         IPaymentService paymentService,
                         ILogger<PaymentsController> logger
                                )
        {
            this.paymentService = paymentService;
            this.logger = logger;
        }

        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await paymentService.CreatOrUpdatPaymentIntent(basketId);
            if (basket == null)
                return BadRequest(new ApiResponse(400,"Problem with Your Basket "));
            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebHook()
        {
            var Json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var StripeEvent = EventUtility.ConstructEvent(Json,Request.Headers["Stripe-Signature"], WebHookSecret);
            PaymentIntent intent;
            ProductOrder order;
            switch (StripeEvent.Type)
            {
                case Events.PaymentIntentPaymentFailed:
                    intent = (PaymentIntent)StripeEvent.Data.Object;
                    logger.LogInformation("Payment Faild" ,intent.Id);
                    order = await paymentService.UpdateOrderPaymentFaild(intent.Id);
                    logger.LogInformation("Payment Faild", order.Id);
                    break;

                case Events.PaymentIntentSucceeded:
                    intent = (PaymentIntent)StripeEvent.Data.Object;
                    logger.LogInformation("Payment Succeeded", intent.Id);
                    order = await paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                    logger.LogInformation("Order Updated To PaymentReceived", order.Id);
                    break;
            }
            return new EmptyResult();
        }
        }
}
