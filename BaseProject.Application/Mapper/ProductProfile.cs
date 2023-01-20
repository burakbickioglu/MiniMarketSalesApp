using BaseProject.Domain.Models.Dtos.Product;

namespace BaseProject.Service.Mapper;

public class ProductProfile : Profile
{
	public ProductProfile()
	{
		CreateMap<Product, AddProductDTO>().ReverseMap();
		CreateMap<Product, UpdateProductDTO>().ReverseMap();
		CreateMap<ProductDTO, UpdateProductDTO>().ReverseMap();
        CreateMap<Product, ProductDTO>().ReverseMap();
	}
}
