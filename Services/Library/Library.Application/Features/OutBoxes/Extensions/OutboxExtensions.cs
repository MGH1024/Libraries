using MGH.Core.Domain.Entities;
using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using Library.Application.Features.OutBoxes.Queries.GetList;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;

namespace Library.Application.Features.OutBoxes.Extensions;

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

    public static GetListModelAsync<OutboxMessage> ToGetListAsyncMode(this GetOutboxListQuery query)
    {
        return new GetListModelAsync<OutboxMessage>
        {
            Index = query.PageRequest.PageIndex,
            Size = query.PageRequest.PageSize
        };
    }
    
}