namespace GolobeTravelApi.Models
{
    public class User
    {
        public int Id { get; set; }                      
        public string CognitoId { get; set; } = null!;     
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName => FirstName + " " +  LastName;
        public string Email { get; set; } = null!;          
        public List<UserFavorites> Favorites { get; set; } = new List<UserFavorites>();
        public List<HotelBooking> Bookings { get; set; } = new List<HotelBooking>();
    }
}
