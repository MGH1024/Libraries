namespace Application.Features.Libraries.Queries.GetList;

public class GetLibraryListDto(Guid id, string title,DateTime createdDate)
{
    public Guid Id { get; set; } = id;
    public string Title { get; set; } = title;
    public DateTime CreatedAt { get; set; } = createdDate;

    public GetLibraryListDto() : this(new Guid(), string.Empty,new DateTime())
    {
    }
}
