using GolobeTravelApi.Data;
using GolobeTravelApi.Dtos.HotelReviewsDto;
using GolobeTravelApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GolobeTravelApi.Controllers
{
    [Route("hotel-reviews")]
    public class HotelReviewsController : BaseController
    {
        private readonly ApplicationDbContext _dbContext;

        public HotelReviewsController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        /// <summary>
        /// Get hotel reviews
        /// </summary>
        /// <param name="request">The hotel which reviews we are getting.</param>
        /// <returns>A list of reviews for a hotel.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getHotelReviews(GetAllHotelReviewsRequest request)
        {
            Console.WriteLine("request", request);
            var existingHotel = await _dbContext.TblHotelData.SingleOrDefaultAsync(e => e.HotelId == request.HotelId);

            if (existingHotel == null)
            {
                return BadRequest();
            }

            int page = request?.Page ?? 1;
            int numberOfRecords = request?.PageSize ?? 3;

            var totalCount = await _dbContext.TblHotelReviews.CountAsync(c => c.HotelId == int.Parse(existingHotel.HotelId));


            IQueryable<HotelReviews> query = _dbContext.TblHotelReviews
                .Where(e => e.HotelId == int.Parse(existingHotel.HotelId))
                .Skip((page - 1) * numberOfRecords)
                .Take(numberOfRecords);

            var reviews = await query.ToArrayAsync();

            if (reviews == null)
            {
                return NotFound();
            }

            return Ok(new { data = reviews, totalCount });
        }
    }
}
