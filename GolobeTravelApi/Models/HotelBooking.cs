namespace GolobeTravelApi.Models
{
    public class HotelBooking
   {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int Nights { get; set; }
        public string HotelId { get; set; } = null!;
        public HotelData? Hotel { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int Rooms { get; set; }
        public decimal BasePrice { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal Taxes { get; set; }
        public decimal NightlyRate { get; set; }
        public decimal CalculatedBasePrice => NumberOfNights * NightlyRate * Rooms;
        public int NumberOfNights => (CheckoutDate - CheckinDate).Days;
        public decimal TotalPrice => CalculatedBasePrice + ServiceFee + Taxes;
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
