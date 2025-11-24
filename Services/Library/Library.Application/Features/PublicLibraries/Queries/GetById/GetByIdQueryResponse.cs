namespace Library.Application.Features.PublicLibraries.Queries.GetById;

public class GetByIdQueryResponse
{
    public Guid Id { get; set; }
    public required string Code { get; set; } = string.Empty;
    public required string Title { get; set; } = string.Empty;
    public required string Location { get; set; } = string.Empty;
    public required string District { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

}
