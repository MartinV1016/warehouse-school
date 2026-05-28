using System.ComponentModel.DataAnnotations;
namespace WarehouseAPI.DTOs
{
    public record ItemCreateDTO(
        string Name, 
        string Category,
        int Quantity,
        [RegularExpression(@"^[A-Z]-\d{2}-[A-Z]-\d{2}$", 
        ErrorMessage = "LocationCode must follow the format 'A-01-B-04' (Sector Letter, 2-digit aisle, shelf letter, 2-digit position).")]
        string Location);
}