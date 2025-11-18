namespace Library.Application.Features.OutBoxes.Queries.GetList;

public class GetOutboxListDto
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public object Payload { get; set; }
    public DateTime OccurredOn { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string Error { get; set; }

    public GetOutboxListDto()
    {
        
    }
    public GetOutboxListDto(
        Guid id,
        string type,
        object content,
        DateTime occurredOn,
        DateTime? processedAt,
        string error)
    {
        Id = id;
        Type = type;
        Payload = content;
        OccurredOn = occurredOn;
        ProcessedAt = processedAt;
        Error = error;
    }
}
