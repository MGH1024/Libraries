using Application.Features.OutBoxes.Queries.GetList;
using MGH.Core.Application.Responses;
using MGH.Core.Domain.Outboxes;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Paging;

namespace Application.Features.Libraries.Extensions;

public static class OutboxExtensions
{
    public static GetListResponse<GetOutboxListDto> ToGetOutboxListDto(this IPaginate<OutboxMessage> libraries)
    {
        return new GetListResponse<GetOutboxListDto>
        {
            Count = libraries.Count,
            Index = libraries.Index,
            Pages = libraries.Pages,
            Size = libraries.Size,
            HasNext = libraries.HasNext,
            HasPrevious = libraries.HasPrevious,
            Items = libraries.Items.Select(a => new GetOutboxListDto
            {
                Id = a.Id,
                Content = a.Content,
                Error = a.Error,
                Type = a.Type,
                ProcessedAt = a.ProcessedAt,
                CreatedAt = a.CreatedAt,
            }).ToList()
        };
    }
}