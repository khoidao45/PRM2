using EVStation_basedRentalSystem.Services.PaymentAPI.Models;
using Net.payOS;
using Net.payOS.Types;

public class PayOSService
{
    private readonly PayOS _payOS;

    public PayOSService(IConfiguration configuration)
    {
        _payOS = new PayOS(
            configuration["PayOS:ClientId"] ?? throw new Exception("Missing PAYOS_CLIENT_ID"),
            configuration["PayOS:ApiKey"] ?? throw new Exception("Missing PAYOS_API_KEY"),
            configuration["PayOS:ChecksumKey"] ?? throw new Exception("Missing PAYOS_CHECKSUM_KEY"),
            configuration["PayOS:PartnerCode"] ?? string.Empty // optional
        );
    }

    // Generate payment link, return checkout URL, QR code, orderCode
    public async Task<(string checkoutUrl, string qrCode, long orderCode)> GeneratePaymentLink(Payment payment)
    {
        long orderCode = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        var items = new List<ItemData>
        {
            new ItemData("Booking payment", 1, (int)payment.Amount) // cast decimal to int
        };

        var paymentData = new PaymentData(
            orderCode,
            (int)payment.Amount,
            "Booking payment",
            items,
            "https://localhost:3000/payment-cancel",
            "https://localhost:3000/payment-success"
        );

        var result = await _payOS.createPaymentLink(paymentData);

        return (result.checkoutUrl, result.qrCode, orderCode);
    }

    // Verify webhook payload and return WebhookData
    public WebhookData VerifyWebhook(WebhookType body)
    {
        return _payOS.verifyPaymentWebhookData(body);
    }

    // Optional: cancel payment link
    public async Task<PaymentLinkInformation> CancelPaymentLink(long orderCode, string? reason = null)
    {
        return await _payOS.cancelPaymentLink(orderCode, reason);
    }

    // Optional: check payment info
    public async Task<PaymentLinkInformation> GetPaymentLinkInformation(long orderCode)
    {
        return await _payOS.getPaymentLinkInformation(orderCode);
    }
}
