using EVStation_basedRentalSystem.Services.PaymentAPI.Data;
using EVStation_basedRentalSystem.Services.PaymentAPI.Models;
using EVStation_basedRendtalSystem.Services.PaymentAPI.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net.payOS.Types;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentDbContext _context;
    private readonly PayOSService _payOSService;
    private readonly IBookingService _bookingService;

    public PaymentController(PaymentDbContext context, PayOSService payOSService, IBookingService bookingService)
    {
        _context = context;
        _payOSService = payOSService;
        _bookingService = bookingService;
    }

    // 1️⃣ Create payment
    [HttpPost("create")]
    public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
    {
        var booking = await _bookingService.GetBookingByIdAsync(request.BookingId);
        if (booking == null)
            return NotFound("Booking not found");

        var existingPayment = await _context.Payments.FirstOrDefaultAsync(p => p.BookingId == booking.Id);
        if (existingPayment != null)
            return BadRequest("Payment already created");

        var payment = new Payment
        {
            BookingId = booking.Id,
            Amount = booking.TotalPrice,
            Status = "Pending"
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        var (checkoutUrl, qrCode, orderCode) = await _payOSService.GeneratePaymentLink(payment);

        payment.OrderCode = orderCode;
        await _context.SaveChangesAsync();

        return Ok(new
        {
            payment.Id,
            payment.BookingId,
            payment.Amount,
            payment.Status,
            CheckoutUrl = checkoutUrl,
            QrCode = qrCode
        });
    }

    // 2️⃣ Webhook
    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook([FromBody] WebhookType body)
    {
        try
        {
            var webhookData = _payOSService.VerifyWebhook(body);
            if (webhookData == null)
                return BadRequest("Webhook verification failed");

            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderCode == webhookData.orderCode);
            if (payment == null) return NotFound("Payment not found");

            if (payment.Status != "Success")
            {
                payment.Status = "Success";
                payment.TransactionId = webhookData.reference;
                payment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await _bookingService.UpdateBookingStatusAsync(payment.BookingId, "Paid");
            }

            return Ok(); // must return 200 to PayOS
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Webhook error: {ex}");
            return BadRequest();
        }
    }

    // 3️⃣ Check status
    [HttpGet("{bookingId}/status")]
    public async Task<IActionResult> GetPaymentStatus(int bookingId)
    {
        var payment = await _context.Payments.FirstOrDefaultAsync(p => p.BookingId == bookingId);
        if (payment == null) return NotFound();

        return Ok(new
        {
            payment.BookingId,
            payment.Status,
            payment.TransactionId,
            payment.UpdatedAt
        });
    }
}

// DTO
public class PaymentRequest
{
    public int BookingId { get; set; }
}
