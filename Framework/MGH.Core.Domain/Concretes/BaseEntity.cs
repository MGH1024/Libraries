﻿using MGH.Core.Domain.Abstracts;

namespace MGH.Core.Domain.Concretes;

public class BaseEntity<T> : IEntity<T>,IEquatable<T>
{
    public T Id { get; protected set; }
    public bool Equals(T obj)
    {
        if (obj is BaseEntity<T> entity)
            return Equals(entity);

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public bool Equals(BaseEntity<T> other)
    {
        return other != null && Id.Equals(other.Id);
    }
}