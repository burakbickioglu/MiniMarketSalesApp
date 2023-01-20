namespace BaseProject.Domain.Models;

public class Sales :BaseEntity
{
    public decimal TotalPrice { get; set; }
    public Guid? BasketId { get; set; }
    public virtual Basket? Basket { get; set; }
    public string AppUserId { get; set; }
    public virtual AppUser AppUser { get; set; }
    public PaymentType PaymentType { get; set; }
}
