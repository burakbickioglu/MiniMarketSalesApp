namespace BaseProject.Domain.Models.Dtos.Basket;

public class AddBasketProductDTO
{
    public Guid BasketId { get; set; }
    public Guid ProductId { get; set; }
}
