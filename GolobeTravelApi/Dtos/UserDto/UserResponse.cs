using GolobeTravelApi.Dtos.FavoritesDto;
using GolobeTravelApi.Models;

namespace GolobeTravelApi.Dtos.UserDto
{
    public class UserResponse
    {
        public int? Id { get; set; }
        public string CognitoId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;

        public List<UserFavoritesResponse> Favorites { get; set; } = new();
     

    }
}
