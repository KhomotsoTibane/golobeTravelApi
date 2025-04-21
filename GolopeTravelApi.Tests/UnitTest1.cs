using GolobeTravelApi.Dtos.HotelBookingDto;
using GolobeTravelApi.Dtos.HotelDataDto;
using GolobeTravelApi.Dtos.UserDto;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace GolopeTravelApi.Tests
{
    public class BasicTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public BasicTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllEntityData_ReturnsOkResult()
        {
            HttpClient client = _factory.CreateClient();

            var response = await client.GetAsync("hotel-entity");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetEntityDataByName_ReturnsOkResult()
        {
            HttpClient client = _factory.CreateClient();

            var respone = await client.GetAsync("hotel-entity/Mpumalanga");

            respone.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task GetHotelsByEntityLocation_ReturnsOkResult()
        {
            HttpClient client = _factory.CreateClient();

            var respone = await client.GetAsync("by-entity-location?EntityName=Paris");

            respone.EnsureSuccessStatusCode();
        }

        //[Fact]
        //public async Task CreateUser_ReturnsCreatedResult()
        //{
        //    var client = _factory.CreateClient();
        //    var request = new User
        //    {
        //        CognitoId = "d13cddsfv8-2081-7056-a235-4582135",
        //        UserName = "TestUser",
        //        FirstName = "Tester",
        //        LastName = "Bester",
        //        Email = "bester@email.com"
        //    };

        //    var response = await client.PostAsJsonAsync("/user", request);

        //    Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        //    var result = await response.Content.ReadFromJsonAsync<UserResponse>();
        //    Assert.Equal(request.CognitoId, result.CognitoId);
        //} 

        [Fact]
        public async Task GetUserById_ReturnsUserResult()
        {

            var client = _factory.CreateClient();


            var response = await client.GetAsync("/user/f6929264-b071-704b-d4ed-a27b9a1d15e5");


            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateUser_ReturnsBadRequestResult()
        {

            var client = _factory.CreateClient();

            //Arrange
            var invalidUser = new CreateUserRequest();


            //Act
            var response = await client.PostAsJsonAsync("/user", invalidUser);


            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            Assert.NotNull(problemDetails);
            Assert.Contains("CognitoId", problemDetails.Errors.Keys);
            Assert.Contains("FirstName", problemDetails.Errors.Keys);
            Assert.Contains("LastName", problemDetails.Errors.Keys);
            Assert.Contains("UserName", problemDetails.Errors.Keys);
            Assert.Contains("Email", problemDetails.Errors.Keys);
            Assert.Contains("The FirstName field is required.", problemDetails.Errors["FirstName"]);
            Assert.Contains("The Email field is required.", problemDetails.Errors["Email"]);
        }

        [Fact]
        public async Task GetHotelsByEntityLocation_ReturnsThreeHotelsResult()
        {

            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync("by-entity-location?entityname=Baku&page=1&recordsPerPage=3");

            response.EnsureSuccessStatusCode();


            //Assert
            var hotels = await response.Content.ReadFromJsonAsync<PagedHotelResponse>();

            Assert.NotNull(hotels);
            Assert.Equal(3, hotels.Data.Count);
        }


        [Fact]
        public async Task CalculateBookingPrice_CheckinBeforeCheckout_ReturnsBadRequestResult()
        {

            var client = _factory.CreateClient();

            //Arrange
            var invalidBookingDetails = new
            {
                checkinDate = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd"),
                checkoutDate = DateTime.Today.ToString("yyyy-MM-dd"),
                adults = 4,
                children = 1,
                rooms = 2,
                baseNightlyRate = 4017m
            };

            var jsonBody = new StringContent(
               JsonSerializer.Serialize(invalidBookingDetails),
               Encoding.UTF8,
               "application/json"
           );

            //Act
            var response = await client.PostAsJsonAsync("/hotel-booking/calculate-total", jsonBody);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            Assert.NotNull(problemDetails);
            Assert.Contains("CheckoutDate", problemDetails.Errors.Keys);
            Assert.Contains("Check-out date must be after check-in date.", problemDetails.Errors["CheckoutDate"]);
        }

        [Fact]
        public async Task CalculateBookingPrice_CheckinSameAsCheckout_ReturnsBadRequestResult()
        {
            var client = _factory.CreateClient();

            //Arrange
            var invalidBookingDetails = new
            {
                checkinDate = DateTime.Today.ToString("yyyy-MM-dd"),
                checkoutDate = DateTime.Today.ToString("yyyy-MM-dd"),
                adults = 4,
                children = 1,
                rooms = 2,
                baseNightlyRate = 4017m
            };

            var jsonBody = new StringContent(
               JsonSerializer.Serialize(invalidBookingDetails),
               Encoding.UTF8,
               "application/json"
             );

            //Act
            var response = await client.PostAsJsonAsync("/hotel-booking/calculate-total", jsonBody);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            Assert.NotNull(problemDetails);
            Assert.Contains("CheckoutDate", problemDetails.Errors.Keys);
            Assert.Contains("Check-out date must be after check-in date.", problemDetails.Errors["CheckoutDate"]);
        }

        [Fact]
        public async Task CalculateBookingPrice_CorrectDates_ReturnsOkResult()
        {

            var client = _factory.CreateClient();

            // Arrange
            var payload = new
            {
                checkinDate = DateTime.Today.ToString("yyyy-MM-dd"),
                checkoutDate = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd"),
                adults = 4,
                children = 1,
                rooms = 2,
                baseNightlyRate = 4017m
            };

            var jsonBody = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var response = await client.PostAsync("hotel-booking/calculate-total", jsonBody);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<HotelBookingPricePreviewResponse>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            Assert.NotNull(result);
            Assert.Equal(2, result.Nights);
            Assert.Equal(16068m, result.BasePrice);
            Assert.Equal(803.4m, result.ServiceFee);
            Assert.Equal(2490.54m, result.Taxes);
            Assert.Equal(19361.94m, result.Total);

        }

        [Fact]
        public async Task CalculateBookingPrice_MaxAdultsAndChildren_ReturnsSuccessResult()
        {
            var client = _factory.CreateClient();

            //Arrange
            var payloadd = new
            {
                checkinDate = DateTime.Today.ToString("yyyy-MM-dd"),
                checkoutDate = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd"),
                Adults = 30,
                children = 10,
                Rooms = 10,
                baseNightlyRate = 4017m
            };

            var jsonBody = new StringContent(
                JsonSerializer.Serialize(payloadd),
                Encoding.UTF8,
                "application/json"
            );
            //Act

            var response = await client.PostAsync("hotel-booking/calculate-total", jsonBody);

            //Assert
            response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<HotelBookingPricePreviewResponse>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            Assert.NotNull(result);
            Assert.Equal(2, result.Nights);
            Assert.Equal(80340m, result.BasePrice);
            Assert.Equal(4017m, result.ServiceFee);
            Assert.Equal(12452.7m, result.Taxes);
            Assert.Equal(96809.7m, result.Total);
        }

        [Fact]
        public async Task CalculateTotal_ZeroAdults_ReturnsValidationError()
        {
            var client = _factory.CreateClient();

            //Arrange
            var payload = new
            {
                checkinDate = DateTime.Today.ToString("yyyy-MM-dd"),
                checkoutDate = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd"),
                adults = 0,
                children = 1,
                rooms = 2,
                baseNightlyRate = 4017m
            };

            var jsonBody = new StringContent(
               JsonSerializer.Serialize(payload),
               Encoding.UTF8,
               "application/json"
           );

            //Act
            var response = await client.PostAsync("hotel-booking/calculate-total", jsonBody);


            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            Assert.NotNull(problemDetails);
            Assert.Contains("Adults", problemDetails.Errors.Keys);
            Assert.Contains("At least one adult is required for this booking.", problemDetails.Errors["Adults"]);
        }

        

    }
}