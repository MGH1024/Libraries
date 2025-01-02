using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.RegisterUser;
using Application.Features.Auth.Commands.UserLogin;
using Application.Features.Users.Queries.GetList;
using AutoMapper;
using MGH.Core.Application.Requests;

namespace Security.Endpoint.Api.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UserLoginCommandDto, UserLoginCommand>()
            .ForCtorParam("UserLoginCommandDto", opt => 
                opt.MapFrom(src => src));

        CreateMap<PageRequest, GetListUserQuery>()
            .ForCtorParam("PageRequest", opt =>
                opt.MapFrom(src => src));

        CreateMap<string, RefreshTokenCommand>()
            .ForMember(d => d.RefreshToken, opt
                => opt.MapFrom(src => src));

        CreateMap<RegisterUserCommandDto, RegisterUserCommand>()
            .ForCtorParam("RegisterUserCommandDto", opt
                => opt.MapFrom(src => src));
    }
}