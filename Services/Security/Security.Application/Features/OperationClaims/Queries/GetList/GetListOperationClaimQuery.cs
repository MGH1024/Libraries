using AutoMapper;
using MediatR;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Security.Application.Features.OperationClaims.Constants;
using Security.Domain.Repositories;

namespace Security.Application.Features.OperationClaims.Queries.GetList;

[Roles(OperationClaimOperationClaims.GetOperationClaims)]
public class GetListOperationClaimQuery(PageRequest pageRequest)
    : IRequest<GetListResponse<GetListOperationClaimListItemDto>>
{
    public PageRequest PageRequest { get; set; } = pageRequest;

    public GetListOperationClaimQuery() : this(new PageRequest { PageIndex = 0, PageSize = 10 })
    {
    }
}

public class GetListOperationClaimQueryHandler(IOperationClaimRepository operationClaimRepository, IMapper mapper)
    : IRequestHandler<GetListOperationClaimQuery, GetListResponse<GetListOperationClaimListItemDto>>
{
    public async Task<GetListResponse<GetListOperationClaimListItemDto>> Handle(
        GetListOperationClaimQuery request,
        CancellationToken cancellationToken
    )
    {
        IPaginate<OperationClaim> operationClaims = await operationClaimRepository.GetListAsync(
            new GetListModelAsync<OperationClaim>
            {
                Index = request.PageRequest.PageIndex,
                Size = request.PageRequest.PageSize,
                CancellationToken = cancellationToken
            });
        var response =
            mapper.Map<GetListResponse<GetListOperationClaimListItemDto>>(
                operationClaims
            );
        return response;
    }
}