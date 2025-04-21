namespace GolobeTravelApi.Dtos.FavoritesDto
{
    public class UserFavoriteHotelResponse
    {
        public string HotelId { get; set; } = null!;
        public string HotelName { get; set; } = null!;
        public int? HotelStars { get; set; }
        public double? HotelReviewsScore { get; set; }
        public int? HotelReviewsTotal { get; set; }
        public string? HotelReviewsDesc { get; set; }
        public double? HotelLowestPrice { get; set; }
        public List<string>? HotelImageUrls { get; set; }
        public string? HotelNation { get; set; }
        public string? HotelCity { get; set; }
        public string? HotelStreetAddress { get; set; }
        public int? TotalAmenities { get; set; }
    }

}
