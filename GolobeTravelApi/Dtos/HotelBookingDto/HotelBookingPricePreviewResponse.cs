namespace GolobeTravelApi.Dtos.HotelBookingDto
{
    public class HotelBookingPricePreviewResponse
    {
        public int Nights { get; set; }
        public decimal BasePrice { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal Taxes { get; set; }
        public decimal Total { get; set; }
    }
}
