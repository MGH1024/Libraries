﻿using Library.Domain.Entities.Books.Exceptions;
using MGH.Core.Domain.BaseEntity;

namespace Library.Domain.Entities.Books.ValueObjects;

public class PublicationDate : ValueObject
{
    public DateTime Value { get; }

    public PublicationDate(DateTime value)
    {
        var now = DateTime.Now.Date;
        if (value > now)
            throw new BookPublicationDateException();
        Value = value;
    }

    public static implicit operator DateTime(PublicationDate publicationDate) => publicationDate.Value;
    public static implicit operator PublicationDate(DateTime publicationDate) => new(publicationDate);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}