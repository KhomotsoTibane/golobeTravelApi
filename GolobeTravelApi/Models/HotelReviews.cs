namespace GolobeTravelApi.Models
{
    public class HotelReviews
    {
        public int Id { get; set; }
        public long ReviewId { get; set; }
        public string ReviewUserType { get; set; } = "Guest";
        public string? ReviewScoreDesc { get; set; }
        public string? ReviewTitle { get; set; }
        public string? ReviewText { get; set; }
        public DateTime? RewviewCommentDate { get; set; }
        public int HotelId { get; set; }
        public HotelData? Hotel { get; set; }
    }
}
