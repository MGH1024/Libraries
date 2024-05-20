namespace MGH.Core.Infrastructure.Persistence.Repositories;

public interface IQuery<T>
{
    IQueryable<T> Query();
}
