using Application.Features.OutBoxes.Queries.GetList;
using MGH.Core.Application.Responses;
using MGH.Core.Domain.Outboxes;
using MGH.Core.Persistence.Models.Filters;
using MGH.Core.Persistence.Models.Paging;

namespace Application.Features.OutBoxes.Extensions;

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

    public static GetListAsyncModel<OutboxMessage> ToGetListAsyncMode(this GetOutboxListQuery query,CancellationToken cancellationToken)
    {
        return new GetListAsyncModel<OutboxMessage>
        {
            Index = query.PageRequest.PageIndex,
            Size = query.PageRequest.PageSize,
            CancellationToken = cancellationToken
        };
    }
    
}