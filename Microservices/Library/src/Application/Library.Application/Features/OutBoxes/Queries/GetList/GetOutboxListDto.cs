using MGH.Core.Application.DTOs.Base;

namespace Application.Features.OutBoxes.Queries.GetList;

public class GetOutboxListDto
    : IDto
{
    public GetOutboxListDto()
    {
        
    }
    public Guid Id { get; set; } 
    public string Type { get; set; } 
    public string Content { get; set; } 
    public DateTime CreatedAt { get; set; } 
    public DateTime? ProcessedAt { get; set; }
    public string? Error { get; set; } 
}