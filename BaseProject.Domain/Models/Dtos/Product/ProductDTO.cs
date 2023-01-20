namespace BaseProject.Domain.Models.Dtos.Product;

public class ProductDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public string? Type { get; set; }
    public decimal Price { get; set; }
    public virtual ICollection<BasketProductDTO>? BasketProducts { get; set; }

}
