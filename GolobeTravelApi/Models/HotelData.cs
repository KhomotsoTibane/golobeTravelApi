using System.Text.Json;
namespace GolobeTravelApi.Models;
public class HotelData
{
    public int Id { get; set; }
    public string EntityId { get; set; } = null!;
    public string HotelId { get; set; } = null!;
    public string HotelDetailsId { get; set; } = null!;
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
    public string? HotelPartnerName { get; set; }
    public string? HotelImageUrls { get; set; }
    public string? HotelAdditionalImageUrls { get; set; }
    public string? HotelHotelDescription { get; set; }
    public JsonDocument? HotelAmenities { get; set; }
    public string? HotelCheckinTime { get; set; }
    public string? HotelCheckoutTime { get; set; }
    public string? HotelNation { get; set; }
    public string? HotelCity { get; set; }
    public string? HotelStreetAddress { get; set; }
    public string? HotelDistrict { get; set; }
    public string? HotelPostcode { get; set; }
    public HotelEntity? Entity { get; set; }   
    public List<HotelReviews>? Reviews { get; set; }
}
