using BaseProject.Domain.Models.Dtos.Basket;
using BaseProject.Domain.Models.Dtos.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Service.Mapper;

public class BasketProfile : Profile
{
	public BasketProfile()
	{
		CreateMap<Basket, BasketDTO>().ReverseMap();
		CreateMap<BasketProduct, BasketProductDTO>().ReverseMap();
		CreateMap<BasketProduct, AddBasketProductDTO>().ReverseMap();
		CreateMap<Sales, SalesDTO>().ReverseMap();
	}
}
