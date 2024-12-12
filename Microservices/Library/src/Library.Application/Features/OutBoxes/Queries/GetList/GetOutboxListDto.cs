namespace Application.Features.OutBoxes.Queries.GetList;

public record GetOutboxListDto(Guid Id, string Type, string Content, DateTime CreatedAt, DateTime? ProcessedAt, string Error);