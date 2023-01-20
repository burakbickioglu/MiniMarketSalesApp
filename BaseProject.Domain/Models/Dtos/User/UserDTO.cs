
using BaseProject.Domain.Models.Dtos.Basket;
using BaseProject.Domain.Models.Dtos.Sales;

namespace BaseProject.Domain.Models.Dtos.User;

public class UserDTO
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public string? AvatarImage { get; set; }
    public DateTimeOffset LastLogin { get; set; }
    //public virtual ICollection<string>? UserTitles { get; set; }
    public virtual ICollection<string>? Roles { get; set; }
    public virtual ICollection<BasketDTO> Baskets { get; set; }
    public virtual ICollection<SalesDTO> Sales { get; set; }
}
