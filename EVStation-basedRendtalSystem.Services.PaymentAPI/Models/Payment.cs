namespace EVStation_basedRentalSystem.Services.PaymentAPI.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending";
        public string TransactionId { get; set; } = "";
        public long OrderCode { get; set; } // for PayOS link
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
