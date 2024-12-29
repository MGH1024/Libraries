using Library.Domain.Books.Exceptions;
using MGH.Core.Domain.BaseEntity;

namespace Library.Domain.Books.ValueObjects;

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