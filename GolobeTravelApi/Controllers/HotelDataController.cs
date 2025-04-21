using GolobeTravelApi.Data;
using GolobeTravelApi.Dtos.HotelDataDto;
using GolobeTravelApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace GolobeTravelApi.Controllers
{
    [Route("hotel-data")]
    public class HotelDataController : BaseController
    {

        private readonly ApplicationDbContext _dbContext;

        public HotelDataController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Get hotel data based on Location.
        /// </summary>
        /// <param name="request">The hotel location data to query.</param>
        /// <returns>A list of hotels based on location</returns>
        [HttpGet("/by-entity-location")]
        [ProducesResponseType(typeof(GetAllHotelDataResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getHotelDataByLocation([FromQuery] GetAllHotelDataRequest? request)
        {
            Console.WriteLine("request", request);

            if (request.EntityName.IsNullOrEmpty())
            {
                return BadRequest();
            }

            var location = await _dbContext.TblHotelEntities
                .SingleOrDefaultAsync(i => i.EntityName == request.EntityName);

            if (location == null)
            {
                return NotFound();
            }


            int page = request?.Page ?? 1;
            int numberOfRecords = request?.RecordsPerPage ?? 3;

            var totalCount = await _dbContext.TblHotelData
                .CountAsync(e => e.EntityId == location.EntityId);

            IQueryable<HotelData> query = _dbContext.TblHotelData
                .Where(e => e.EntityId == location.EntityId)
                .Skip((page - 1) * numberOfRecords)
                .Take(numberOfRecords);

            string? cognitoUserId = User.Identity?.IsAuthenticated == true ? User.FindFirst("cognito:username")?.Value : null;
            List<int> userFavoriteHotelIds = new();
            if (cognitoUserId != null)
            {
                var user = await _dbContext.TblUser.SingleOrDefaultAsync(u => u.CognitoId == cognitoUserId);
                if (user != null)
                {
                    userFavoriteHotelIds = await _dbContext.TblUserFavorites.Where(f => f.UserId == user.Id)
                        .Select(f => f.HotelId)
                        .ToListAsync();
                }
            }


            if (request != null)
            {
                if (!string.IsNullOrWhiteSpace(request.Rating) && int.TryParse(request.Rating, out int parsedRating))
                {
                    query = query.Where(r => r.HotelStars >= parsedRating);
                }

                //if(request.Amenities !=null && request.Amenities.Any())
                //{
                //    foreach (var item in request.Amenities)
                //    {
                //        query = query.Where(a => a.HotelAmenities!.Contains(item));
                //    }
                //}

                if (request.PriceMin != null && request.PriceMax != null)
                {
                    query = query.Where(p => p.HotelLowestPrice >= request.PriceMin && p.HotelLowestPrice <= request.PriceMax);
                }

            }


            var hotelData = await query.ToArrayAsync();
            if (hotelData == null)
            {
                return NotFound();
            }

            var hotelResponses = hotelData
            .Select(h => HotelDataToHotelDataResponse(h, userFavoriteHotelIds))
            .ToList();

            return Ok(new PagedHotelResponse { Data = hotelResponses, TotalCount = totalCount });

        }



        /// <summary>
        /// Get hotel details.
        /// </summary>
        /// <param name="hotelName">The name of the hotel which details we are querying.</param>
        /// <returns>A the hotel details on the requested hotel</returns>
        [HttpGet("/{hotelName}/details")]
        [ProducesResponseType(typeof(GetHotelDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getHotelDetails(string hotelName)
        {

            IQueryable<HotelData> query = _dbContext.TblHotelData.Where(e => e.HotelName == hotelName);

            var hotelData = await query.FirstOrDefaultAsync();
            if (hotelData == null)
            {
                return NotFound();
            }
            string? cognitoUserId = User.Identity?.IsAuthenticated == true ? User.FindFirst("cognito:username")?.Value : null;
            List<int> userFavoriteHotelIds = new();
            if (cognitoUserId != null)
            {
                var user = await _dbContext.TblUser.SingleOrDefaultAsync(u => u.CognitoId == cognitoUserId);
                if (user != null)
                {
                    userFavoriteHotelIds = await _dbContext.TblUserFavorites.Where(f => f.UserId == user.Id)
                        .Select(f => f.HotelId)
                        .ToListAsync();
                }
            }


            return Ok(HotelDetailsToGetHotelDetailsResponse(hotelData, userFavoriteHotelIds));
        }


        private static GetHotelDetailsResponse HotelDetailsToGetHotelDetailsResponse(HotelData hotelData, List<int> userFavoriteHotelIds)
        {
            //var amenitiesList = JsonConvert.DeserializeObject<List<string>>(hotelData.HotelAmenities!);
            var imagesList = JsonConvert.DeserializeObject<List<string>>(hotelData.HotelImageUrls!);
            List<AmenityImages> amenitiesImageList = JsonConvert.DeserializeObject<List<AmenityImages>>(hotelData.HotelAdditionalImageUrls);

            return new GetHotelDetailsResponse
            {
                HotelId = hotelData.HotelId,
                HotelName = hotelData.HotelName,
                HotelStars = hotelData.HotelStars,
                HotelLongitude = hotelData.HotelLongitude,
                HotelLatitude = hotelData.HotelLatitude,
                HotelDistance = hotelData.HotelDistance,
                HotelRelevantPoiDistance = hotelData.HotelRelevantPoiDistance,
                HotelReviewsScore = hotelData.HotelReviewsScore,
                HotelReviewsTotal = hotelData.HotelReviewsTotal,
                HotelReviewsDesc = hotelData.HotelReviewsDesc,
                HotelLowestPrice = hotelData.HotelLowestPrice,
                HotelImageUrls = imagesList,
                HotelHotelDescription = hotelData.HotelHotelDescription,
                HotelAmenities = hotelData.HotelAmenities,
                HotelCheckinTime = hotelData.HotelCheckinTime,
                HotelCheckoutTime = hotelData.HotelCheckoutTime,
                HotelNation = hotelData.HotelNation,
                HotelCity = hotelData.HotelCity,
                HotelStreetAddress = hotelData.HotelStreetAddress,
                HotelDistrict = hotelData.HotelDistrict,
                HotelPostcode = hotelData.HotelPostcode,
                HotelAdditionalImageUrls = amenitiesImageList,
                IsFavorite = userFavoriteHotelIds.Contains(int.Parse(hotelData.HotelId))
                //TotalAmenities = hotelData.HotelAmenities?.Count ?? 0
            };
        }
        private static GetAllHotelDataResponse HotelDataToHotelDataResponse(HotelData hotelData, List<int> userFavoriteHotelIds)
        {
            var imagesList = JsonConvert.DeserializeObject<List<string>>(hotelData.HotelImageUrls!);
            return new GetAllHotelDataResponse
            {
                HotelId = hotelData.HotelId,
                HotelName = hotelData.HotelName,
                HotelStars = hotelData.HotelStars,
                HotelLongitude = hotelData.HotelLongitude,
                HotelLatitude = hotelData.HotelLatitude,
                HotelReviewsScore = hotelData.HotelReviewsScore,
                HotelReviewsTotal = hotelData.HotelReviewsTotal,
                HotelReviewsDesc = hotelData.HotelReviewsDesc,
                HotelLowestPrice = hotelData.HotelLowestPrice,
                HotelImageUrls = imagesList,
                HotelNation = hotelData.HotelNation,
                HotelCity = hotelData.HotelCity,
                HotelStreetAddress = hotelData.HotelStreetAddress,
                HotelDistrict = hotelData.HotelDistrict,
                HotelPostcode = hotelData.HotelPostcode,
                TotalAmenities = hotelData.HotelAmenities?.RootElement.EnumerateArray().Count(),
                IsFavorite = userFavoriteHotelIds.Contains(int.Parse(hotelData.HotelId))
            };


        }
    }
}

