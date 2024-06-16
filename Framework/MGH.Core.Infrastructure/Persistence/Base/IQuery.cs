namespace MGH.Core.Infrastructure.Persistence.Base;

public interface IQuery<T>
{
    IQueryable<T> Query();
}
