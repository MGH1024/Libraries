using Application.Features.Auth.Commands.RevokeToken;
using AutoMapper;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Auth.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RefreshTkn, RevokedTokenResponse>().ReverseMap();
    }
}
