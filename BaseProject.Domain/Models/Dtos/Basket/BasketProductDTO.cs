using BaseProject.Domain.Models.Dtos.Basket;
using BaseProject.Domain.Models.Dtos.Product;

namespace BaseProject.Domain.Models.Dtos;

public class BasketProductDTO
{
    public Guid Id { get; set; }
    public Guid BasketId { get; set; }
    public virtual BasketDTO? Basket { get; set; }
    public Guid ProductId { get; set; }
    public virtual ProductDTO? Product { get; set; }
}
