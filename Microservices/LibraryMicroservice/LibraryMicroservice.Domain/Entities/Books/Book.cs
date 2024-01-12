using Domain.Entities.Books.Exceptions;
using Domain.Entities.Books.ValueObjects;
using MGH.Core.Domain.Concretes;

namespace Domain.Entities.Books;

public class Book : AggregateRoot<Guid>
{
    public BookIsbn BookIsbn { get; }
    public BookTitle BookTitle { get; }
    public BookUniqueCode BookUniqueCode { get; }
    public BookIsReference BookIsReference { get; }
    public BookBorrow BookBorrow { get; }
    public BookReturnDate BookReturnDate { get; }
    public BookRegisterBorrowDate BookRegisterBorrowDate { get; }
    public BookPublicationDate BookPublicationDate { get; }
    
    private readonly List<BookAuthor> _bookAuthors = new();
    public IReadOnlyCollection<BookAuthor> BookAuthors => _bookAuthors;

    public Book(BookIsbn bookIsbn, BookTitle bookTitle, BookPublicationDate bookPublicationDate,
        BookUniqueCode bookUniqueCode, BookIsReference bookIsReference)
    {
        Id = Guid.NewGuid();
        BookIsbn = bookIsbn;
        BookTitle = bookTitle;
        BookPublicationDate = bookPublicationDate;
        BookUniqueCode = bookUniqueCode;
        BookIsReference = bookIsReference;
    }
    
    public Book(BookIsbn bookIsbn, BookTitle bookTitle, BookPublicationDate bookPublicationDate,
        BookUniqueCode bookUniqueCode, BookIsReference bookIsReference,List<BookAuthor> bookAuthors)
    {
        Id = Guid.NewGuid();
        BookIsbn = bookIsbn;
        BookTitle = bookTitle;
        BookPublicationDate = bookPublicationDate;
        BookUniqueCode = bookUniqueCode;
        BookIsReference = bookIsReference;
        _bookAuthors.RemoveAll(a=>!string.IsNullOrEmpty(a.Name));
        bookAuthors.ForEach(AddBookAuthor);
    }
    
    public void AddBookAuthor(BookAuthor bookAuthor)
    {
        if (BookAuthorExist(bookAuthor.Name))
            throw new BookAuthorAlreadyExistException();
        _bookAuthors.Add(bookAuthor);
    }

    public void RemoveBookAuthor(BookAuthor bookAuthor)
    {
        if (!BookAuthorExist(bookAuthor.Name))
            throw new BookAuthorNotFoundException();
        _bookAuthors.Remove(bookAuthor);
    }
    

    bool BookAuthorExist(string name)
        => _bookAuthors.Exists(a => a.Name == name);
    

    BookAuthor GetAuthorByName(string name)
    {
        return _bookAuthors.Find(a => a.Name == name);
    }
    
}