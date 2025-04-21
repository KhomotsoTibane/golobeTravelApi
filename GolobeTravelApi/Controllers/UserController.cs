using GolobeTravelApi.Data;
using GolobeTravelApi.Dtos.FavoritesDto;
using GolobeTravelApi.Dtos.UserDto;
using GolobeTravelApi.Models;
using GolobeTravelApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GolobeTravelApi.Controllers
{
    [Route("user")]
    public class UserController : BaseController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly EmailService _emailService;

        public UserController(ApplicationDbContext dbContext, EmailService emailService)
        {
            this._dbContext = dbContext;
            this._emailService = emailService;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>All user records.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _dbContext.TblUser.ToArrayAsync();
          
           
            return Ok(users.Select(UserToUserResponse));
        }

        /// <summary>
        /// Gets a muser by ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The single user record.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _dbContext.TblUser.Include(f => f.Favorites).SingleOrDefaultAsync(u => u.CognitoId == id);
            if (user == null)
            {
                return NotFound();
            }
            var userResponse = UserToUserResponse(user);
            return Ok(userResponse);
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="userRequest">The user to be created.</param>
        /// <returns>A link to the main member that was created.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserFromSignUp([FromBody] CreateUserRequest userRequest)
        {
            var newUser = new User
            {
                FirstName = userRequest.FirstName,
                UserName = userRequest.UserName,
                LastName = userRequest.LastName,
                Email = userRequest.Email,
                CognitoId = userRequest.CognitoId,
                Favorites = [],
                Bookings = [],
            };

            _dbContext.TblUser.Add(newUser);

            await _dbContext.SaveChangesAsync();
            await _emailService.SendWelcomeMessage(newUser.Email, newUser.FullName);

            return CreatedAtAction(nameof(GetUserById), new { id = newUser.CognitoId }, newUser);


        }

        private static UserResponse UserToUserResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                CognitoId = user.CognitoId,
                FullName = user.FullName,
                Email = user.Email,
                Favorites = user.Favorites.Select(f => new UserFavoritesResponse
                {
                    HotelId = f.HotelId,
                    CreatedAt = DateTime.Now,
                }).ToList(),
            };
        }
    }
}
