
using Application_Layer;
using Application_Layer.DTO;
using Application_Layer.Interface;
using Domain_Layer.Interface;
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
        private readonly IBookingRepository bookingRepository; 
        private readonly IPaymentRepository paymentRepository; 
        private readonly IBookingSubject bookingSubject; 
        private readonly ILogger<PaymentController> logger; 

        public PaymentController(
            IPaymentService paymentService,
            IBookingRepository bookingRepository, 
            IPaymentRepository paymentRepository, 
            IBookingSubject bookingSubject, 
            ILogger<PaymentController> logger) 
        {
            this.paymentService = paymentService;
            this.bookingRepository = bookingRepository; 
            this.paymentRepository = paymentRepository;
            this.bookingSubject = bookingSubject; 
            this.logger = logger; 
        }

        [HttpPost("process/{bookingId}")]
        public async Task<ActionResult<PaymentResult>> ProcessPayment(
           int bookingId,
           [FromBody] PaymentDTO dto)
        {
            try
            {
                logger.LogInformation($" Processing payment for booking {bookingId}");

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
                    logger.LogInformation($" Payment successful: {result.TransactionId}");

                    // Get booking and payment details for email
                    var bookingWithDetails = await bookingRepository.GetByIdBookingAsync(bookingId);
                    var payments = await paymentRepository.GetAllPaymentsByBookingIdAsync(bookingId);
                    var latestPayment = payments.OrderByDescending(p => p.PaymentDate).FirstOrDefault();

                    if (latestPayment != null)
                    {
                        // OBSERVER PATTERN - Notify observers (Sends Payment Email!)
                        logger.LogInformation($" Notifying observers: Payment completed - {result.TransactionId}");
                        await bookingSubject.NotifyPaymentCompletedAsync(bookingWithDetails, latestPayment);
                        logger.LogInformation($" Payment notification sent successfully");
                    }

                    return Ok(result);
                }

                logger.LogWarning($" Payment failed: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                logger.LogError($" Payment processing error: {ex.Message}");
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

     
        [HttpPost("refund/{bookingId}")]
        public async Task<ActionResult<PaymentResult>> RefundPayment(
            int bookingId,
            [FromBody] RefundRequest request)
        {
            try
            {
                logger.LogInformation($"💰 Processing refund for booking {bookingId}");

                var result = await paymentService.RefundBookingPaymentAsync(bookingId, request.Reason);

                if (result.IsSuccess)
                {
                    logger.LogInformation($"✅ Refund successful: {result.TransactionId}");

                    // Optional: Add refund notification
                    // var booking = await bookingRepository.GetBookingByIdAsync(bookingId);
                    // await bookingSubject.NotifyRefundCompletedAsync(booking);

                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                logger.LogError($" Refund processing error: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class RefundRequest
    {
        public string Reason { get; set; }
    }
}