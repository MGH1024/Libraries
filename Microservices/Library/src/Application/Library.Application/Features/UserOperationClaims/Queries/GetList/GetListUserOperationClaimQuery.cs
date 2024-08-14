using AutoMapper;
using Domain;
using MediatR;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.UserOperationClaims.Queries.GetList;

public class GetListUserOperationClaimQuery(PageRequest pageRequest)
    : IRequest<GetListResponse<GetListUserOperationClaimListItemDto>>
{
    public PageRequest PageRequest { get; set; } = pageRequest;

    public GetListUserOperationClaimQuery() : this(new PageRequest { PageIndex = 0, PageSize = 10 })
    {
    }

    public class GetListUserOperationClaimQueryHandler(
        IUow uow,
        IMapper mapper)
        : IRequestHandler<GetListUserOperationClaimQuery, GetListResponse<GetListUserOperationClaimListItemDto>>
    {
        public async Task<GetListResponse<GetListUserOperationClaimListItemDto>> Handle(
            GetListUserOperationClaimQuery request,
            CancellationToken cancellationToken
        )
        {
            var userOperationClaims = await uow.UserOperationClaim.GetListAsync(
                new GetListAsyncModel<UserOperationClaim>
                {
                    Index = request.PageRequest.PageIndex,
                    Size = request.PageRequest.PageSize
                });

            var mappedUserOperationClaimListModel =
                mapper.Map<GetListResponse<GetListUserOperationClaimListItemDto>>(userOperationClaims);
            return mappedUserOperationClaimListModel;
        }
    }
}