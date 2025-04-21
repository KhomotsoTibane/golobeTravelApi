using GolobeTravelApi.Models;

namespace GolobeTravelApi.Dtos.HotelReviewsDto
{
    public class GetAllHotelReviewsResponse
    {
        public long ReviewId { get; set; }
        public string? UserType { get; set; }
        public string? ScoreDesc { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public DateTime? CommentDate { get; set; }
    }
}
