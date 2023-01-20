namespace BaseProject.Service.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AppUser, UserDTO>().ReverseMap();
        CreateMap<AppUser, DeleteDTO>().ReverseMap();
        CreateMap<AppUser, UserListViewModel>();
    }

}
