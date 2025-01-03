using AutoMapper;
using MediatR;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Infrastructure.Caching.Models;
using Security.Application.Features.Users.Constants;
using Security.Application.Features.Users.Rules;
using Security.Domain;

namespace Security.Application.Features.Users.Queries.GetById;

[Roles(UsersOperationClaims.GetUsers)]
public class GetUserByIdQuery : IRequest<GetUserByIdResponse>, ICacheRequest
{
    public int Id { get; set; }
    public string CacheKey => $"GetUserById={Id}";
    public int AbsoluteExpirationRelativeToNow => 60;
}

public class GetUserByIdQueryHandler(
    IUow uow,
    IMapper mapper,
    IUserBusinessRules userBusinessRules)
    : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await uow.User.GetAsync(request.Id, cancellationToken);
        await userBusinessRules.UserShouldBeExistsWhenSelected(user);

        var response = mapper.Map<GetUserByIdResponse>(user);
        return response;
    }
}