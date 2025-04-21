using FluentValidation;

namespace GolobeTravelApi.Dtos.FavoritesDto
{
    public class ToggleFavoriteRequest
    {
        public string HotelId { get; set; }
    }

    public class ToggleFavoriteRequestValidator : AbstractValidator<ToggleFavoriteRequest>
    {
        public ToggleFavoriteRequestValidator()
        {
            RuleFor(r => r.HotelId)
                .NotEmpty().WithMessage("Please specify a hotelId")
                .Must(id => !string.IsNullOrWhiteSpace(id?.Trim())).WithMessage("HotelId cannot be blank");
        }
    }
}
