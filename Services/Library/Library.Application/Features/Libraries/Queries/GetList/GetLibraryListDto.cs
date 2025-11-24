namespace Library.Application.Features.Libraries.Queries.GetList;

public class GetLibraryListDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public GetLibraryListDto() { }

    public GetLibraryListDto(Guid id, string code, string title, string location, string district, DateTime createdAt)
    {
        Id = id;
        Code = code;
        Title = title;
        Location = location;
        District = district;
        CreatedAt = createdAt;
    }
}
