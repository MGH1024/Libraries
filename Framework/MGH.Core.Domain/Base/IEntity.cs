namespace MGH.Core.Domain.Base;

public interface IEntity<out T> 
{
    T Id { get; }
}