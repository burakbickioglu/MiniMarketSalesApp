using BaseProject.Domain.Models.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Domain.Models.Dtos.Basket;

public class BasketDTO
{
    public Guid Id { get; set; }
    public string AppUserId { get; set; }
    public virtual UserDTO AppUser { get; set; }
    public decimal TotalPrice { get; set; }
    public virtual ICollection<BasketProductDTO>? BasketProducts { get; set; }
}
