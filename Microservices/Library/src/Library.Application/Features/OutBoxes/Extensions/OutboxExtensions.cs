using Application.Features.OutBoxes.Queries.GetList;
using MGH.Core.Application.Responses;
using MGH.Core.Domain.Entity.Outboxes;
using MGH.Core.Infrastructure.Persistence.EF.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Persistence.EF.Models.Paging;

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

    public static GetListModelAsync<OutboxMessage> ToGetListAsyncMode(this GetOutboxListQuery query,CancellationToken cancellationToken)
    {
        return new GetListModelAsync<OutboxMessage>
        {
            Index = query.PageRequest.PageIndex,
            Size = query.PageRequest.PageSize,
            CancellationToken = cancellationToken
        };
    }
    
}