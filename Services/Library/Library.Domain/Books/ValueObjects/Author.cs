using Library.Domain.Books.Exceptions;
using MGH.Core.Domain.BaseModels;

namespace Library.Domain.Books.ValueObjects;

public class Author : ValueObject
{
    public string FullName { get; }
    public string NationalCode { get; }

    public Author(string fullName, string nationalCode)
    {
        if (string.IsNullOrEmpty(fullName))
            throw new AuthorNameException();

        if (string.IsNullOrEmpty(nationalCode))
            throw new NationalCodeException();

        if (nationalCode.Length != 10)
            throw new NationalCodeLengthException();

        FullName = fullName;
        NationalCode = nationalCode;
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return NationalCode;
    }
}