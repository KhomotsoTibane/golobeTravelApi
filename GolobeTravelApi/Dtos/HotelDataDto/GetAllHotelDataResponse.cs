using GolobeTravelApi.Models;

namespace GolobeTravelApi.Dtos.HotelDataDto
{
    public class GetAllHotelDataResponse
    {
        public string? HotelId { get; set; }
        public string? HotelName { get; set; }
        public int? HotelStars { get; set; }
        public double? HotelLongitude { get; set; }
        public double? HotelLatitude { get; set; }
        public double? HotelReviewsScore { get; set; }
        public int? HotelReviewsTotal { get; set; }
        public string? HotelReviewsDesc { get; set; }
        public double? HotelLowestPrice { get; set; }
        public List<string>? HotelImageUrls { get; set; }
        public string? HotelNation { get; set; }
        public string? HotelCity { get; set; }
        public string? HotelStreetAddress { get; set; }
        public string? HotelDistrict { get; set; }
        public string? HotelPostcode { get; set; }

        public int? TotalAmenities { get; set; }

        public bool? IsFavorite { get; set; }
    }

    public class PagedHotelResponse
    {
        public List<GetAllHotelDataResponse> Data { get; set; }
        public int TotalCount { get; set; }
    }

}
