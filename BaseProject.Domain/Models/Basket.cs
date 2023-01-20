namespace BaseProject.Domain.Models;

public class Basket :BaseEntity
{
    public string AppUserId { get; set; }
    public virtual AppUser AppUser { get; set; }
    public decimal TotalPrice { get; set; }
    public virtual ICollection<BasketProduct>? BasketProducts { get; set; }
}
