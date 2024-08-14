using AutoMapper;
using Domain;
using MediatR;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.Users.Queries.GetList;

public class GetListUserQuery : IRequest<GetListResponse<GetListUserListItemDto>>
{
    public PageRequest PageRequest { get; set; }

    public GetListUserQuery()
    {
        PageRequest = new PageRequest { PageIndex = 0, PageSize = 10 };
    }

    public GetListUserQuery(PageRequest pageRequest)
    {
        PageRequest = pageRequest;
    }

    public class GetListUserQueryHandler(IUow uow, IMapper mapper)
        : IRequestHandler<GetListUserQuery, GetListResponse<GetListUserListItemDto>>
    {
        public async Task<GetListResponse<GetListUserListItemDto>> Handle(GetListUserQuery request,
            CancellationToken cancellationToken)
        {
            var users = await uow.User.GetListAsync(
                new GetListAsyncModel<User>
                {
                    Index = request.PageRequest.PageIndex,
                    Size = request.PageRequest.PageSize,
                    CancellationToken = cancellationToken
                });

            var response = mapper.Map<GetListResponse<GetListUserListItemDto>>(users);
            return response;
        }
    }
}