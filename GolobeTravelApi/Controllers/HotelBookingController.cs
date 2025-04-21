using GolobeTravelApi.Data;
using GolobeTravelApi.Dtos.HotelBookingDto;
using GolobeTravelApi.Models;
using GolobeTravelApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace GolobeTravelApi.Controllers
{
    [Route("hotel-booking")]
    public class HotelBookingController : BaseController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly EmailService _emailService;
        public HotelBookingController(ApplicationDbContext dbContext, EmailService emailService)
        {
            this._dbContext = dbContext;
            this._emailService = emailService;
        }

        /// <summary>
        /// Get user booking by booking id
        /// </summary>
        /// <param name="id">The booking id.</param>
        /// <returns>A booking that was created by the user.</returns>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HotelBookingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _dbContext.TblHotelBookings
                .Include(b => b.Hotel)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            var hotelBooking = new HotelBookingResponse
            {
                BookingId = booking.BookingId,
                HotelName = booking.Hotel?.HotelName ?? "",
                CheckinDate = booking.CheckinDate,
                CheckoutDate = booking.CheckoutDate,
                TotalPrice = booking.TotalPrice
            };

            return Ok(hotelBooking);
        }
        /// <summary>
        /// Create a booking for a user and send confirmation email
        /// </summary>
        /// <param name="request">The booking deatails to be processed</param>
        /// <returns>A link to the booking that was created and confirmation email</returns>
        [Authorize]
        [HttpPost("create-new")]
        [ProducesResponseType(typeof(HotelBookingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserHotelBooking(HotelBookingRequest request)
        {
            var existingUser = await _dbContext.TblUser.SingleOrDefaultAsync(u => u.CognitoId == request.UserId);
            if (existingUser == null)
            {
                return BadRequest();
            }

            var existingHotel = await _dbContext.TblHotelData.SingleOrDefaultAsync(h => h.HotelId == request.HotelId);
            if (existingHotel == null)
            {
                return BadRequest();
            }
            var utcCheckinDate = DateTime.SpecifyKind(request.CheckinDate, DateTimeKind.Local).ToUniversalTime();
            var utcCheckoutDate = DateTime.SpecifyKind(request.CheckoutDate, DateTimeKind.Local).ToUniversalTime();
            var nights = (utcCheckoutDate.Day - utcCheckinDate.Day);
            var total = (request.BasePrice * nights * request.Rooms) + request.ServiceFee + request.Taxes;


            var newBooking = new HotelBooking
            {
                UserId = existingUser.Id,
                HotelId = existingHotel.HotelId,
                CheckinDate = utcCheckinDate,
                CheckoutDate = utcCheckoutDate,
                Adults = request.Adults,
                Children = request.Children,
                Rooms = request.Rooms,
                BasePrice = request.BasePrice,
                ServiceFee = request.ServiceFee,
                Taxes = request.Taxes,
                Nights = nights

            };

            _dbContext.TblHotelBookings.Add(newBooking);
            await _dbContext.SaveChangesAsync();

            var zaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time");
            var checkinZA = TimeZoneInfo.ConvertTimeFromUtc(newBooking.CheckinDate, zaTimeZone);
            var checkoutZA = TimeZoneInfo.ConvertTimeFromUtc(newBooking.CheckoutDate, zaTimeZone);
        
            var response = new HotelBookingResponse
            {
                BookingId = newBooking.BookingId,
                HotelName = existingHotel.HotelName!,
                CheckinDate = checkinZA,
                CheckoutDate = checkoutZA,
                TotalPrice = total,
            };

            await _emailService.SendBookingConfirmationEmail(existingUser.Email, existingUser.FirstName, existingHotel.HotelName!, checkinZA, checkoutZA, total, newBooking.Rooms, newBooking.Adults, newBooking.Children);
            return CreatedAtAction(nameof(GetBookingById), new { id = newBooking.BookingId }, response);
        }


        /// <summary>
        /// Calculate booking fee
        /// </summary>
        /// <param name="request">The booking deatails to be calculated</param>
        /// <returns>Booking fee details</returns>
        [HttpPost("calculate-total")]
        [ProducesResponseType(typeof(HotelBookingPricePreviewResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CalculateBookingTotal([FromBody] HotelBookingCalculationRequest request)
        {
            if (request.CheckinDate >= request.CheckoutDate)
                return BadRequest("Checkout must be after checkin.");

            var numberOfNights = (request.CheckoutDate.Date - request.CheckinDate.Date).Days;
            if (numberOfNights < 1)
                return BadRequest("Check-out must be at least one day after check-in.");

            decimal nightlyRate = request.BaseNightlyRate; 
            decimal basePrice = numberOfNights * nightlyRate * request.Rooms;

            decimal serviceFee = basePrice * 0.05m; 
            decimal tax = basePrice * 0.155m;        

            decimal total = basePrice + serviceFee + tax;

            var response = new HotelBookingPricePreviewResponse
            {
                Nights = numberOfNights,
                BasePrice = basePrice,
                ServiceFee = serviceFee,
                Taxes = tax,
                Total = total
            };

            return Ok(response);
        }

    }
}
