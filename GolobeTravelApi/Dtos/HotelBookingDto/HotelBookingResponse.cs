namespace GolobeTravelApi.Dtos.HotelBookingDto
{
    public class HotelBookingResponse
    {
        public int BookingId { get; set; }
        public string HotelName { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
