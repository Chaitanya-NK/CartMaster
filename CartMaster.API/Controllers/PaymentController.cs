using CartMaster.Business.IServices;
using CartMaster.Data.Models;
using CartMaster.Static;
using CartMaster.TokenGeneration.TokenInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable 1591
#pragma warning disable 8625

namespace CartMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IToken _token;
        public PaymentController(IPaymentService paymentService, IToken token)
        {
            _paymentService = paymentService;
            _token = token;
        }

        [HttpPost("HandlePayment")]
        public IActionResult HandlePayment([FromQuery] string action, [FromBody] PaymentModel paymentModel = null, int orderId = 0)
        {
            try
            {
                switch(action.ToLower())
                {
                    case "getbyorderid":
                        var payment = _paymentService.GetPaymentDetailsByOrderId(orderId);
                        return Ok(payment);

                    case "add":
                        if (paymentModel == null) return BadRequest("Payment data is required.");
                        var addedPayment = _paymentService.InsertPaymentDetails(paymentModel);
                        return Ok(new { success = true, message = StaticPayment.CreatePaymentaSuccess });

                    default:
                        return BadRequest("Invalid action specified");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
