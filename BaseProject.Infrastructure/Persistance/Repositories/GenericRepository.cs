namespace BaseProject.Infrastructure.Persistance.Repositories;

public class GenericRepository<T> : DataRepository<T, DataContext>, IGenericRepository<T> where T : class, IBaseEntity
{
    public GenericRepository(DataContext context, ICurrentUserService userService) : base(context, userService)
    {
    }
}
