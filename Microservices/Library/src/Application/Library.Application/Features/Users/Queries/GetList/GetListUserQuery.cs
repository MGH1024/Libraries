using Application.Features.Users.Extensions;
using AutoMapper;
using Domain;
using MediatR;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Securities.Security.Constants;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.Users.Queries.GetList;

public class GetListUserQuery(PageRequest pageRequest)
    : IRequest<GetListResponse<GetListUserListItemDto>>, ISecuredRequest
{
    public PageRequest PageRequest { get; set; } = pageRequest;

    public GetListUserQuery() : this(new PageRequest { PageIndex = 0, PageSize = 10 })
    {
    }

    public string[] Roles => new[] { GeneralOperationClaims.GetUsers };
}

public class GetListUserQueryHandler(IUow uow, IMapper mapper)
    : IRequestHandler<GetListUserQuery, GetListResponse<GetListUserListItemDto>>
{
    public async Task<GetListResponse<GetListUserListItemDto>> Handle(GetListUserQuery request,
        CancellationToken cancellationToken)
    {
        var users = await uow.User.GetListAsync(request.ToGetListAsyncModel(cancellationToken));
        return mapper.Map<GetListResponse<GetListUserListItemDto>>(users);
    }
}