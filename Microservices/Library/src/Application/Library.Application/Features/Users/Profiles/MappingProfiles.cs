using AutoMapper;
using Application.Features.Users.Commands.AddUser;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Users.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, CreateUserCommand>().ReverseMap();
        CreateMap<User, CreatedUserResponse>().ReverseMap();
        // CreateMap<User, UpdateUserCommand>().ReverseMap();
        // CreateMap<User, UpdatedUserResponse>().ReverseMap();
        // CreateMap<User, UpdateUserFromAuthCommand>().ReverseMap();
        // CreateMap<User, UpdatedUserFromAuthResponse>().ReverseMap();
        // CreateMap<User, DeleteUserCommand>().ReverseMap();
        // CreateMap<User, DeletedUserResponse>().ReverseMap();
        // CreateMap<User, GetByIdUserResponse>().ReverseMap();
        // CreateMap<User, GetListUserListItemDto>().ReverseMap();
        // CreateMap<IPaginate<User>, GetListResponse<GetListUserListItemDto>>().ReverseMap();
    }
}
