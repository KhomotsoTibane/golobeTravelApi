using GolobeTravelApi.Models;
using System.Text.Json;

namespace GolobeTravelApi.Dtos.HotelDataDto
{
    public class GetHotelDetailsResponse
    {
        public string? HotelId { get; set; }

        public string? HotelName { get; set; }

        public int? HotelStars { get; set; }

        public double? HotelLongitude { get; set; }

        public double? HotelLatitude { get; set; }

        public string? HotelDistance { get; set; }

        public string? HotelRelevantPoiDistance { get; set; }

        public double? HotelReviewsScore { get; set; }

        public int? HotelReviewsTotal { get; set; }

        public string? HotelReviewsDesc { get; set; }

        public double? HotelLowestPrice { get; set; }

        public List<string>? HotelImageUrls { get; set; }

        public string? HotelHotelDescription { get; set; }

        public JsonDocument? HotelAmenities { get; set; }

        public string? HotelCheckinTime { get; set; }

        public string? HotelCheckoutTime { get; set; }

        public string? HotelNation { get; set; }

        public string? HotelCity { get; set; }

        public string? HotelStreetAddress { get; set; }

        public string? HotelDistrict { get; set; }

        public string? HotelPostcode { get; set; }

        public List<AmenityImages>? HotelAdditionalImageUrls { get; set; }

        public bool? IsFavorite { get; set; }
        public int? TotalAmenities { get; set; }

    }

    public class AmenityImages
    {
        public string Category { get; set; }
        public string Thumbnail { get; set; }
        public string Gallery { get; set; }
        public string Dynamic { get; set; }
    }
}
