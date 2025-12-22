using Application_Layer;
using Application_Layer.DTO;
using Application_Layer.Interface;
using Domain_Layer.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost("process/{bookingId}")]
        public async Task<ActionResult<PaymentResult>> ProcessPayment(
           int bookingId,
           [FromBody] PaymentDTO dto)
        {
            try
            {
                var request = new PaymentRequest
                {
                    CardNumber = dto.CardNumber,
                    CardHolderName = dto.CardHolderName,
                    ExpiryMonth = dto.ExpiryMonth,
                    ExpiryYear = dto.ExpiryYear,
                    CVV = dto.CVV,
                    BankName = dto.BankName,
                    AccountNumber = dto.AccountNumber
                };

                var result = await paymentService.ProcessBookingPaymentAsync(bookingId, request);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("validate/{transactionId}")]
        public async Task<ActionResult> ValidatePayment(string transactionId)
        {
            try
            {
                var isValid = await paymentService.ValidatePaymentAsync(transactionId);
                return Ok(new { transactionId, isValid });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("methods")]
        public ActionResult<IEnumerable<string>> GetPaymentMethods()
        {
            var methods = paymentService.GetAvailablePaymentMethods();
            return Ok(methods);
        }
    }
}
