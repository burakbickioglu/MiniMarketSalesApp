namespace BaseProject.Domain.Models.Dtos.Product;

public class AddProductDTO
{
    public string Name { get; set; }
    public string? Type { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
}
