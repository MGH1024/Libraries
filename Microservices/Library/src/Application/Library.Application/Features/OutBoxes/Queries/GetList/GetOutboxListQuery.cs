using Application.Features.Libraries.Extensions;
using Domain.Entities.Libraries;
using MediatR;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using MGH.Core.Domain.Outboxes;

namespace Application.Features.OutBoxes.Queries.GetList;

public class GetOutboxListQuery(PageRequest pageRequest) : IRequest<IList<OutboxMessage>>
{
    public PageRequest PageRequest { get; set; } = pageRequest;

    public GetOutboxListQuery() : this(new PageRequest { PageIndex = 0, PageSize = 10 })
    {
    }

    public class GetLibraryListQueryHandler(IOutBoxRepository outBoxRepository)
        : IRequestHandler<GetOutboxListQuery, GetListResponse<GetOutboxListDto>>
    {
        public async Task<IList<GetOutboxListDto>> Handle(GetOutboxListQuery request,
            CancellationToken cancellationToken)
        {
            var outboxes = await outBoxRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                orderBy: a => a.OrderBy(x => x.CreatedAt),
                cancellationToken: cancellationToken
            );
            return outboxes.Items;
            //return outboxes.ToGetOutboxListDto();
        }
    }
}