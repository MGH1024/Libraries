using MGH.Core.Application.DTOs.Base;

namespace Application.Features.OutBoxes.Queries.GetList;

public class GetOutboxListDto(
    Guid id,
    string type,
    string content,
    DateTime createdAt,
    DateTime? processedAt,
    string error)
    : IDto
{
    public Guid Id { get; set; } = id;
    public string Type { get; set; } = type;
    public string Content { get; set; } = content;
    public DateTime CreatedAt { get; set; } = createdAt;
    public DateTime? ProcessedAt { get; set; } = processedAt;
    public string Error { get; set; } = error;

    public GetOutboxListDto() : this(new Guid(),string.Empty,string.Empty,new DateTime(),
        new DateTime(),string.Empty)
    {
            
    }
}