
namespace BaseProject.Service.Mapper;

public class BaseEntityProfile : Profile
{
    public BaseEntityProfile()
    {
        CreateMap<BaseEntity, BaseEntityDTO>().ReverseMap();
       
        CreateMap<GeneralContent, GeneralContentDTO>().ReverseMap();
    }
}
