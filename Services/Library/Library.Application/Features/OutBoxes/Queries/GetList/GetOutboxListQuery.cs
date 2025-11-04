using MediatR;
using Library.Domain.Outboxes;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using Library.Application.Features.OutBoxes.Extensions;

namespace Library.Application.Features.OutBoxes.Queries.GetList;

public class GetOutboxListQuery(PageRequest pageRequest) : IRequest<GetListResponse<GetOutboxListDto>>
{
    public PageRequest PageRequest { get; set; } = pageRequest;

    public GetOutboxListQuery() : this(new PageRequest { PageIndex = 0, PageSize = 100 })
    {
    }

    public class GetOutboxListQueryHandler(IOutBoxRepository outBoxRepository) : IRequestHandler<GetOutboxListQuery, GetListResponse<GetOutboxListDto>>
    {
        public async Task<GetListResponse<GetOutboxListDto>> Handle(GetOutboxListQuery request,
            CancellationToken cancellationToken)
        {
            var outboxes = await outBoxRepository.GetListAsync(request.ToGetListAsyncMode());
            return outboxes.ToGetOutboxListDto();
        }
    }
}