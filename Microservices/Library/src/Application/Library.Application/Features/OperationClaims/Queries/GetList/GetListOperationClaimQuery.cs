using AutoMapper;
using Domain.Security;
using MediatR;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Application.Features.OperationClaims.Queries.GetList;

public class GetListOperationClaimQuery(PageRequest pageRequest)
    : IRequest<GetListResponse<GetListOperationClaimListItemDto>>
{
    public PageRequest PageRequest { get; set; } = pageRequest;

    public GetListOperationClaimQuery() : this(new PageRequest { PageIndex = 0, PageSize = 10 })
    {
    }

    public class GetListOperationClaimQueryHandler
        : IRequestHandler<GetListOperationClaimQuery, GetListResponse<GetListOperationClaimListItemDto>>
    {
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IMapper _mapper;

        public GetListOperationClaimQueryHandler(IOperationClaimRepository operationClaimRepository, IMapper mapper)
        {
            _operationClaimRepository = operationClaimRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListOperationClaimListItemDto>> Handle(
            GetListOperationClaimQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<OperationClaim> operationClaims = await _operationClaimRepository.GetListAsync(
                new GetListAsyncModel<OperationClaim>
                {
                    Index = request.PageRequest.PageIndex,
                    Size = request.PageRequest.PageSize,
                    CancellationToken = cancellationToken
                });
            var response =
                _mapper.Map<GetListResponse<GetListOperationClaimListItemDto>>(
                    operationClaims
                );
            return response;
        }
    }
}