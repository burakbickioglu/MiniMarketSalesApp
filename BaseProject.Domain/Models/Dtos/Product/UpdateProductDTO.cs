namespace BaseProject.Domain.Models.Dtos.Product;

public class UpdateProductDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public string? Type { get; set; }
    public decimal Price { get; set; }
}
