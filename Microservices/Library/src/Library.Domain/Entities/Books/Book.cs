using Library.Domain.Entities.Books.Exceptions;
using Library.Domain.Entities.Books.ValueObjects;
using MGH.Core.Domain.BaseEntity;

namespace Library.Domain.Entities.Books;

public class Book : AggregateRoot<Guid>
{
    public Isbn Isbn { get; }
    public Title Title { get; }
    public UniqueCode UniqueCode { get; }
    public IsReference IsReference { get; }
    public Borrow Borrow { get; }
    public ReturnDate ReturnDate { get; }
    public RegisterBorrowDate RegisterBorrowDate { get; }
    public PublicationDate PublicationDate { get; }
    
    private readonly List<Author> _bookAuthors = new();
    public IReadOnlyCollection<Author> BookAuthors => _bookAuthors;

    public Book(Isbn isbn, Title title, PublicationDate publicationDate,
        UniqueCode uniqueCode, IsReference isReference)
    {
        Id = Guid.NewGuid();
        Isbn = isbn;
        Title = title;
        PublicationDate = publicationDate;
        UniqueCode = uniqueCode;
        IsReference = isReference;
    }
    
    public Book(Isbn isbn, Title title, PublicationDate publicationDate,
        UniqueCode uniqueCode, IsReference isReference,List<Author> bookAuthors)
    {
        Id = Guid.NewGuid();
        Isbn = isbn;
        Title = title;
        PublicationDate = publicationDate;
        UniqueCode = uniqueCode;
        IsReference = isReference;
        _bookAuthors.RemoveAll(a=>!string.IsNullOrEmpty(a.Name));
        bookAuthors.ForEach(AddBookAuthor);
    }
    
    public void AddBookAuthor(Author author)
    {
        if (BookAuthorExist(author.Name))
            throw new BookAuthorAlreadyExistException();
        _bookAuthors.Add(author);
    }

    public void RemoveBookAuthor(Author author)
    {
        if (!BookAuthorExist(author.Name))
            throw new BookAuthorNotFoundException();
        _bookAuthors.Remove(author);
    }
    

    bool BookAuthorExist(string name)
        => _bookAuthors.Exists(a => a.Name == name);
    

    Author GetAuthorByName(string name)
    {
        return _bookAuthors.Find(a => a.Name == name);
    }
    
}