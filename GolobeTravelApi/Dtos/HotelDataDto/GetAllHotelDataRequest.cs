using FluentValidation;

namespace GolobeTravelApi.Dtos.HotelDataDto
{
    public class GetAllHotelDataRequest
    {
        public int? Page { get; set; }
        public int? RecordsPerPage { get; set; }

        public string? EntityName { get; set; }

        public string? Rating { get; set; }

        public int? PriceMin { get; set; }

        public int? PriceMax { get; set; }

        public List<string>? Amenities { get; set; }
    }

    public class GetAllHotelDataRequestValidator : AbstractValidator<GetAllHotelDataRequest>
    {
        public GetAllHotelDataRequestValidator()
        {
            RuleFor(r=>r.EntityName).NotEmpty()
                .WithMessage("Please specify entity/location name");
            RuleFor(r => r.Page)
            .GreaterThanOrEqualTo(1)
            .When(r => r.Page.HasValue)
            .WithMessage("Page number must be a positive integer.");

            RuleFor(r => r.RecordsPerPage)
                .GreaterThanOrEqualTo(1).When(r => r.RecordsPerPage.HasValue)
                .WithMessage("You must return at least one record.")
                .LessThanOrEqualTo(100)
                .WithMessage("You cannot return more than 100 records.");

            RuleFor(r => r.PriceMin)
                .GreaterThanOrEqualTo(0).When(r => r.PriceMin.HasValue)
                .WithMessage("PriceMin cannot be negative.");

            RuleFor(r => r.PriceMax)
                .GreaterThanOrEqualTo(0).When(r => r.PriceMax.HasValue)
                .WithMessage("PriceMax cannot be negative.");

            RuleFor(r => r)
                .Must(r => !r.PriceMin.HasValue || !r.PriceMax.HasValue || r.PriceMin <= r.PriceMax)
                .WithMessage("PriceMin cannot be greater than PriceMax.");

            RuleFor(r => r.Rating)
                .Must(r => string.IsNullOrEmpty(r) || int.TryParse(r, out var rating) && rating >= 1 && rating <= 5)
                .WithMessage("Rating must be a number between 1 and 5.");
        }
    }
}




