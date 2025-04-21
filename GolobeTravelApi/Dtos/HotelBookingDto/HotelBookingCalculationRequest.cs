using FluentValidation;

namespace GolobeTravelApi.Dtos.HotelBookingDto
{
    public class HotelBookingCalculationRequest
    {
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int Rooms { get; set; }
        public decimal BaseNightlyRate { get; set; }
    }

    public class HotelBookingCalculationRequestValidator : AbstractValidator<HotelBookingCalculationRequest>
    {
        public HotelBookingCalculationRequestValidator()
        {
            RuleFor(r => r.CheckinDate)
                     .NotEmpty().WithMessage("Check-in date is required.")
                     .Must(date => date.Date >= DateTime.Today).WithMessage("Check-in date must be today or in the future.");

            RuleFor(r => r.CheckoutDate)
                .NotEmpty().WithMessage("Check-out date is required.")
                .GreaterThan(r => r.CheckinDate).WithMessage("Check-out date must be after check-in date.");

            RuleFor(r => r.Adults)
                .GreaterThan(0).WithMessage("At least one adult is required for this booking.")
                .LessThanOrEqualTo(30).WithMessage("Maximum 30 adults allowed per booking.");

            RuleFor(r => r.Children)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(10).WithMessage("Maximum 10 children allowed.");

            RuleFor(r => r.Rooms)
                .GreaterThan(0).WithMessage("At least one room is required for this booking.")
                .LessThanOrEqualTo(10).WithMessage("Maximum 10 rooms allowed per booking.");

            RuleFor(r => r.BaseNightlyRate)
                .GreaterThan(0).WithMessage("Base nightly rate must be greater than zero.");
        }
    }
}
