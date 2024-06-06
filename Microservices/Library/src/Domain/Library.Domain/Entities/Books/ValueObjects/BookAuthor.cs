using Domain.Entities.Books.Exceptions;
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Books.ValueObjects;

public class BookAuthor : ValueObject
{
    public string Name { get; }

    public BookAuthor(string name)
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