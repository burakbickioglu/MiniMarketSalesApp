namespace BaseProject.Domain.Models;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public int Stock { get; set; }
    public string? Type { get; set; }
    public decimal Price { get; set; }
    public virtual ICollection<BasketProduct>? BasketProducts { get; set; }
}
