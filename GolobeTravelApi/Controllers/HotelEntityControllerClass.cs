using GolobeTravelApi.Data;
using GolobeTravelApi.Dtos.HotelDataDto;
using GolobeTravelApi.Dtos.HotelEntityDto;
using GolobeTravelApi.Models;
using GolobeTravelApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace GolobeTravelApi.Controllers
{
    [Route("hotel-entity")]
    public class HotelEntityController : BaseController
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly ImageService _imageService;

        public HotelEntityController(ApplicationDbContext dbContext, ImageService imageService)
        {
            this._dbContext = dbContext;
            this._imageService = imageService;
        }

        /// <summary>
        /// Get hotel entity/location data.
        /// </summary>
        /// <returns>A list of hotel entities/locations</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getAllHotelEntities()
        {
            IQueryable<HotelEntity> query = _dbContext.TblHotelEntities;

            var hotelEntities = await query.ToListAsync();
            var result = hotelEntities.Select(h => EntityResponseToGetAllEntityResponse(h, _imageService));

            return Ok(result);
        }


        /// <summary>
        /// Get entity/location data by name
        /// </summary>
        /// <param name="name,e">The name of the location/entity.</param>
        /// <returns>A single entity/location by name</returns>
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(GetAllEntitiesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getHotelEntitiesByName([FromRoute] string name)
        {

            var entity = await _dbContext.TblHotelEntities.SingleOrDefaultAsync(x => x.EntityName == name);
            if (entity == null)
            {
                return NotFound();
            }

            return Ok(EntityResponseToGetAllEntityResponse(entity, _imageService));
        }


        private static GetAllEntitiesResponse EntityResponseToGetAllEntityResponse(HotelEntity hotelEntity, ImageService imageService)
        {
            var location = hotelEntity.EntityLocation.Split(',');
            var baseName = hotelEntity.EntityName.Replace(" ", "").ToLower();


            string imageUrl = string.Empty;



            var normalizedName = RemoveDiacritics(baseName);
            var url = imageService.GetEntityImageUrl(normalizedName, "jpg");


            if (url.ToLower().Contains(baseName) | url.ToLower().Contains(normalizedName))
            {
                imageUrl = url;

            }


            return new GetAllEntitiesResponse
            {
                EntityName = hotelEntity.EntityName,
                EntityHierarchy = hotelEntity.EntityHierarchy!.Replace("|", ", "),
                EntityId = hotelEntity.EntityId,
                lat = location[0],
                lng = location[1],
                ImageUrl = imageUrl

            };
        }

        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var normalized = text.Normalize(NormalizationForm.FormD);
            var chars = normalized
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }

    }
}
