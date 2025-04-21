using GolobeTravelApi;
using GolobeTravelApi.Data;
using GolobeTravelApi.Dtos.FavoritesDto;
using GolobeTravelApi.Dtos.HotelDataDto;
using GolobeTravelApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace golobApi.Controllers
{
    [Route("favorites")]
    public class FavoritesController : BaseController
    {

        private readonly ApplicationDbContext _dbContext;
        public FavoritesController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Get user hotel favorites
        /// </summary>
        /// <param name="cognitoId">The id of the user.</param>
        /// <returns>A list of favorited hotels by the user</returns>
        [Authorize]
        [HttpGet("user/{cognitoId}")]
        [ProducesResponseType(typeof(UserFavoriteHotelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserFavorites(string cognitoId)
        {
            var user = await _dbContext.TblUser
                .Include(u => u.Favorites)
                .FirstOrDefaultAsync(u => u.CognitoId == cognitoId);

            if (user == null)
                return NotFound($"User with ID '{cognitoId}' not found.");


            var hotelIds = user.Favorites.Select(f => f.HotelId.ToString()).ToList();

            var hotels = await _dbContext.TblHotelData
                .Where(h => hotelIds.Contains(h.HotelId))
                .ToListAsync();
            var favorites = hotels.Select(HotelDataToUserFavoriteHotelResponse);

            return Ok(favorites);
        }

        /// <summary>
        /// Add and remove hotel from user favorites
        /// </summary>
        ///    /// <param name="cognitoId">The id of the user</param>
        /// <param name="favoriteRequest">The hotel id</param>
        /// <returns>A list of favorited hotels by the user</returns>
        [Authorize]
        [HttpPost("user/{cognitoId}/toggle")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ToggleFavorite([FromRoute]string cognitoId, [FromBody] ToggleFavoriteRequest favoriteRequest)
        {
 
            var existingUser = await _dbContext.TblUser.FirstOrDefaultAsync(u => u.CognitoId == cognitoId);
            if (existingUser == null)
            {
                return NotFound($"User with id '{cognitoId}' not found.");
            }

            var parsedHotelId = int.Parse(favoriteRequest.HotelId);

            var existingFavorite = await _dbContext.TblUserFavorites
                .FirstOrDefaultAsync(f => f.UserId == existingUser.Id && f.HotelId == parsedHotelId);

            if (existingFavorite != null)
            {
                _dbContext.TblUserFavorites.Remove(existingFavorite);
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "Hotel removed from favorites", favoriteRequest.HotelId});
            }
            else
            {
                var favorite = new UserFavorites
                {
                    UserId = existingUser.Id,
                    HotelId = parsedHotelId,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.TblUserFavorites.Add(favorite);
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "Hotel added to favorites", favoriteRequest.HotelId });
            }
        }


        private static UserFavoriteHotelResponse HotelDataToUserFavoriteHotelResponse(HotelData hotelData)
        {
            var imagesList = JsonConvert.DeserializeObject<List<string>>(hotelData.HotelImageUrls!);
            return new UserFavoriteHotelResponse
            {
                HotelId = hotelData.HotelId,
                HotelName = hotelData.HotelName,
                HotelStars = hotelData.HotelStars,
                HotelReviewsScore = hotelData.HotelReviewsScore,
                HotelReviewsTotal = hotelData.HotelReviewsTotal,
                HotelReviewsDesc = hotelData.HotelReviewsDesc,
                HotelLowestPrice = hotelData.HotelLowestPrice,
                HotelImageUrls = imagesList,
                HotelNation = hotelData.HotelNation,
                HotelCity = hotelData.HotelCity,
                HotelStreetAddress = hotelData.HotelStreetAddress,
                TotalAmenities = hotelData.HotelAmenities?.RootElement.EnumerateArray().Count()
            };
        }


    } 
}
