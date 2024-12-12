namespace Application.Features.OutBoxes.Queries.GetList;

public class GetOutboxListDto
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string Error { get; set; }

    public GetOutboxListDto()
    {
        
    }
    public GetOutboxListDto(
        Guid id,
        string type,
        string content,
        DateTime createdAt,
        DateTime? processedAt,
        string error)
    {
        Id = id;
        Type = type;
        Content = content;
        CreatedAt = createdAt;
        ProcessedAt = processedAt;
        Error = error;
    }
}
