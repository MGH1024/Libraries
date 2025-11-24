using MediatR;
using Library.Domain.Outboxes;
using MGH.Core.Application.Requests;
using Library.Application.Features.OutBoxes.Extensions;

namespace Library.Application.Features.OutBoxes.Queries.GetList;

public class GetOutboxListQuery(PageRequest pageRequest) : IRequest<List<GetOutboxListDto>>
{
    public PageRequest PageRequest { get; set; } = pageRequest;

    public GetOutboxListQuery() : this(new PageRequest { PageIndex = 0, PageSize = 100 })
    {
    }

    public class GetOutboxListQueryHandler(IOutboxMessageRepository outBoxRepository) : IRequestHandler<GetOutboxListQuery, List<GetOutboxListDto>>
    {
        public async Task<List<GetOutboxListDto>> Handle(GetOutboxListQuery request,
            CancellationToken cancellationToken)
        {
            var outboxes = await outBoxRepository.GetListAsync(
                request.PageRequest.PageIndex,
                request.PageRequest.PageSize);

            return outboxes.ToGetOutboxListDto();
        }
    }
}