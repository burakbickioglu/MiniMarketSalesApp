namespace BaseProject.Infrastructure.Interfaces.Repositories;

public interface IGenericRepository<T> : IDataRepository<T> where T : class, IBaseEntity
{
}
