using MGH.Core.CrossCutting.Exceptions.Types;


namespace Domain.Entities.Books.Exceptions;

public class BookAuthorNotFoundException() : BookException("book author not found");
