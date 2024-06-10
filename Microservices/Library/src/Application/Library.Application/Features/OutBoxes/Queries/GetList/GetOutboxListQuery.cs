using Application.Features.Libraries.Extensions;
using Domain.Entities.Libraries;
using MediatR;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;

namespace Application.Features.OutBoxes.Queries.GetList;

public class GetOutboxListQuery(PageRequest pageRequest) : IRequest<GetListResponse<GetOutboxListDto>>
{
    public PageRequest PageRequest { get; set; } = pageRequest;

    public GetOutboxListQuery() : this(new PageRequest { PageIndex = 0, PageSize = 10 })
    {
    }

    public class GetOutboxListQueryHandler(IOutBoxRepository outBoxRepository)
        : IRequestHandler<GetOutboxListQuery, GetListResponse<GetOutboxListDto>>
    {
        public async Task<GetListResponse<GetOutboxListDto>> Handle(GetOutboxListQuery request,
            CancellationToken cancellationToken)
        {
            var outboxes = await outBoxRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                orderBy: a => a.OrderBy(x => x.CreatedAt),
                cancellationToken: cancellationToken
            );

            return outboxes.ToGetOutboxListDto();
        }
    }
}