using AutoMapper;
using Application.Features.Auth.Commands.Login;
using Application.Features.Users.Queries.GetList;
using MGH.Core.Application.DTOs.Security;
using MGH.Core.Application.Requests;

namespace Api.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UserForLoginDto, LoginCommand>()
            .ForCtorParam("userForLoginDto", opt => opt.MapFrom(src => src))
            .ForCtorParam("ipAddress", opt => opt.MapFrom((src,context) => context.Items["IpAddress"]));
        
        CreateMap<PageRequest, GetListUserQuery>()
            .ForCtorParam("PageRequest", opt => opt.MapFrom(src => src));
    }
}