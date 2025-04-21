using FluentValidation;

namespace GolobeTravelApi.Dtos.HotelReviewsDto
{
    public class GetAllHotelReviewsRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int? RecordsPerPage { get; set; }

        public string HotelId { get; set; }
    }

    public class GetAllHotelReviewsRequestValidator : AbstractValidator<GetAllHotelReviewsRequest>
    {
        public GetAllHotelReviewsRequestValidator()
        {
            RuleFor(b => b.HotelId)
            .NotEmpty().WithMessage("A Hotel is required to fetch reviews");

            RuleFor(r => r.Page)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be set to a positive non-zero integer.");

            RuleFor(r => r.RecordsPerPage)
                .GreaterThanOrEqualTo(1).WithMessage("You must return at least one record.")
                .LessThanOrEqualTo(100).WithMessage("You cannot return more than 100 records.");
        }
    }
}
