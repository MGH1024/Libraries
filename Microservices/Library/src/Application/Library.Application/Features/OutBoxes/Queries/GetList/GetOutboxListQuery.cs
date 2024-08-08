using Application.Features.Libraries.Extensions;
using Application.Features.OutBoxes.Extensions;
using Domain.Entities.Libraries;
using MediatR;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using MGH.Core.Domain.Outboxes;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Filters;

namespace Application.Features.OutBoxes.Queries.GetList;

public class GetOutboxListQuery(PageRequest pageRequest) : IRequest<GetListResponse<GetOutboxListDto>>
{
    public PageRequest PageRequest { get; set; } = pageRequest;

    public GetOutboxListQuery() : this(new PageRequest { PageIndex = 0, PageSize = 100 })
    {
    }

    public class GetOutboxListQueryHandler(IOutBoxRepository outBoxRepository)
        : IRequestHandler<GetOutboxListQuery, GetListResponse<GetOutboxListDto>>
    {
        public async Task<GetListResponse<GetOutboxListDto>> Handle(GetOutboxListQuery request,
            CancellationToken cancellationToken)
        {
            var outboxes = await outBoxRepository.GetListAsync(request.ToGetListAsyncMode(cancellationToken));
            return outboxes.ToGetOutboxListDto();
        }
    }
}