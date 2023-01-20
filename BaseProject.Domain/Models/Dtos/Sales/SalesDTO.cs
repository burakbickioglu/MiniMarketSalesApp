using BaseProject.Domain.Models.Dtos.Basket;
using BaseProject.Domain.Models.Dtos.User;

namespace BaseProject.Domain.Models.Dtos.Sales;

public class SalesDTO
{
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }
    public Guid? BasketId { get; set; }
    public virtual BasketDTO? Basket { get; set; }
    public string AppUserId { get; set; }
    public virtual UserDTO AppUser { get; set; }
    public PaymentType PaymentType { get; set; }
}
