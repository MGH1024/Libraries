using MGH.Core.CrossCutting.Exceptions.Types;


namespace Domain.Entities.Books.Exceptions;

public class BookAuthorNotFoundException : BookException
{
    public BookAuthorNotFoundException() : base("book author not found")
    {
    }
}