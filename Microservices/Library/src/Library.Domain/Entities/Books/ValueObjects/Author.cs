using Domain.Entities.Books.Exceptions;
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Books.ValueObjects;

public class Author : ValueObject
{
    public string Name { get; }

    public Author(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new AuthorNameException();
        Name = name;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}