namespace GolobeTravelApi.Dtos.HotelEntityDto
{
    public class GetAllEntitiesResponse
    {
        
        public string? EntityName { get; set; }
        public string? EntityHierarchy { get; set; }

        public string? EntityId { get; set; }

        public string? lat { get; set; }
        public string? lng { get; set; }

        public string ImageUrl { get; set; }
    }
}
