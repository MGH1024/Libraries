using System.Linq.Expressions;
using Application.Features.Auth.Commands.RegisterUser;
using Application.Features.Auth.Commands.RevokeToken;
using AutoMapper;
using MGH.Core.Infrastructure.Persistence.EF.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Auth.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RefreshTkn, RevokedTokenResponse>().ReverseMap();

        CreateMap<RegisterUserCommand, User>()
            .ForMember(d => d.Email, src =>
                src.MapFrom(a => a.RegisterUserCommandDto.Email))
            .ForMember(d => d.FirstName, src =>
                src.MapFrom(a => a.RegisterUserCommandDto.FirstName))
            .ForMember(d => d.LastName, src =>
                src.MapFrom(a => a.RegisterUserCommandDto.LastName))
            .ReverseMap();

        CreateMap<string, GetBaseModel<User>>()
            .ForMember(dest => dest.Predicate, opt => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Email == src)))
            .ForMember(dest => dest.EnableTracking, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.WithDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.CancellationToken, opt => opt.Ignore());

        CreateMap<int, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Id == src)))
            .ForMember(dest => dest.EnableTracking, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.WithDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.CancellationToken, opt => opt.Ignore());
    }
}