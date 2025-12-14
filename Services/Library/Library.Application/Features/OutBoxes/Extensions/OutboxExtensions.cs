using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Persistence.Paging;
using MGH.Core.Infrastructure.Persistence.Entities;
using Library.Application.Features.OutBoxes.Queries.GetList;

namespace Library.Application.Features.OutBoxes.Extensions;

public static class OutboxExtensions
{
    public static GetListResponse<GetOutboxListDto> ToGetOutboxListDto(this IPagedResult<OutboxMessage> libraries)
    {
        return new GetListResponse<GetOutboxListDto>
        {
            Count = libraries.TotalCount,
            Index = libraries.PageIndex,
            Pages = libraries.TotalPages,
            Size = libraries.PageSize,
            HasNext = libraries.HasNext,
            HasPrevious = libraries.HasPrevious,
            Items = libraries.Items.Select(a => new GetOutboxListDto
            {
                Id = a.Id,
                Payload = a.Payload,
                Error = a.Error,
                Type = a.Type,
                ProcessedAt = a.ProcessedAt,
                OccurredOn = a.OccurredOn,
            }).ToList()
        };
    }

    public static List<GetOutboxListDto> ToGetOutboxListDto(this IEnumerable<OutboxMessage> messages)
    {
        return messages.Select(a => new GetOutboxListDto
        {
            Id = a.Id,
            Type = a.Type,
            Payload = a.Payload,
            OccurredOn = a.OccurredOn,
        }).ToList();
    }
}