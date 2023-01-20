namespace BaseProject.Domain.Models;

public class BasketProduct : BaseEntity
{
    public Guid BasketId { get; set; }
    public virtual Basket? Basket { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product? Product { get; set; }
}
