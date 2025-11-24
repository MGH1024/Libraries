namespace Library.Application.Features.PublicLibraries.Queries.GetList;

public class GetListQueryResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
