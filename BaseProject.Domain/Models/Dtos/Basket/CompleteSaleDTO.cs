namespace BaseProject.Domain.Models.Dtos.Basket;

public class CompleteSaleDTO
{
    public Guid BasketId { get; set; }
    public PaymentType PaymentType { get; set; }
}
