namespace GolobeTravelApi.Models;
public class HotelEntity
{
    public string EntityId { get; set; } = null!;
    public string? EntityName { get; set; }
    public string? EntityLocation { get; set; }
    public string? EntityHierarchy { get; set; }
    public List<HotelData> Hotels { get; set; } = new List<HotelData>();
}
