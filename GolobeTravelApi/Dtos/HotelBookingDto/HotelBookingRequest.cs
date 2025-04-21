using FluentValidation;
using GolobeTravelApi.Models;

namespace GolobeTravelApi.Dtos.HotelBookingDto
{
    public class HotelBookingRequest
    {
        public string UserId { get; set; }
        public string HotelId { get; set; } = null!;
        public DateTime CheckinDate { get; set; }
        public string checkinTime { get; set; }
        public DateTime CheckoutDate { get; set; }
        public string checkoutTime { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int Rooms { get; set; }
        public decimal BasePrice { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice => BasePrice + ServiceFee + Taxes;

    }

    public class HotelBookingRequestValidator : AbstractValidator<HotelBookingRequest>
    {
        public HotelBookingRequestValidator() {
            RuleFor(b => b.UserId)
            .NotEmpty().WithMessage("A User is required to make a booking.");

            RuleFor(b => b.HotelId)
                .NotEmpty().WithMessage("A Hotel is required to make a booking");

            RuleFor(r => r.CheckinDate)
                   .NotEmpty().WithMessage("Check-in date is required.")
                   .Must(date => date.Date >= DateTime.Today).WithMessage("Check-in date must be today or in the future.");

            RuleFor(r => r.checkinTime).NotEmpty().WithMessage("Checkin time cannot be empty");


            RuleFor(r => r.checkoutTime).NotEmpty().WithMessage("Checkout time cannot be empty");

            RuleFor(r => r.CheckoutDate)
                .NotEmpty().WithMessage("Check-out date is required.")
                .GreaterThan(r => r.CheckinDate).WithMessage("Check-out date must be after check-in date.");

            RuleFor(r => r.Adults)
                .GreaterThan(0).WithMessage("At least one adult is required for this booking.")
                .LessThanOrEqualTo(30).WithMessage("Maximum 30 adults allowed per booking.");

            RuleFor(r => r.Children)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(10).WithMessage("Maximum 10 children allowed per booking.");

            RuleFor(r => r.Rooms)
                .GreaterThan(0).WithMessage("At least one room is required for this booking.")
                .LessThanOrEqualTo(10).WithMessage("Maximum 10 rooms allowed per booking.");

            RuleFor(r => r.BasePrice)
                .GreaterThan(0).WithMessage("Base nightly rate must be greater than zero.");

            RuleFor(b => b.Adults)
                .GreaterThan(0).WithMessage("At least one adult must be included.");

            RuleFor(b => b.Rooms)
                .GreaterThan(0).WithMessage("At least one room must be booked.");

            RuleFor(b => b.BasePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Base price cannot be negative.");

            RuleFor(b => b.Taxes)
                .GreaterThanOrEqualTo(0).WithMessage("Taxes cannot be negative.");

            RuleFor(b => b.ServiceFee)
                .GreaterThanOrEqualTo(0).WithMessage("Service fee cannot be negative.");
        }
    }
}
