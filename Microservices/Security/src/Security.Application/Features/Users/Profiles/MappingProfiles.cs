using System.Linq.Expressions;
using Application.Features.Auth.Commands.Login;
using Application.Features.Users.Commands.Create;
using Application.Features.Users.Commands.Delete;
using Application.Features.Users.Commands.Update;
using Application.Features.Users.Commands.UpdateFromAuth;
using Application.Features.Users.Queries.GetById;
using Application.Features.Users.Queries.GetList;
using Application.Helpers;
using AutoMapper;
using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Persistence.EF.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Persistence.EF.Models.Paging;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Users.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, CreateUserCommand>().ReverseMap();
        CreateMap<User, CreatedUserResponse>().ReverseMap();
        CreateMap<User, UpdateUserCommand>().ReverseMap();
        CreateMap<User, UpdatedUserResponse>().ReverseMap();
        CreateMap<User, UpdateUserFromAuthCommand>().ReverseMap();
        CreateMap<User, UpdatedUserFromAuthResponse>().ReverseMap();
        CreateMap<User, DeleteUserCommand>().ReverseMap();
        CreateMap<User, DeletedUserResponse>().ReverseMap();
        CreateMap<User, GetUserByIdResponse>().ReverseMap();
        CreateMap<User, GetListUserListItemDto>().ReverseMap();
        CreateMap<IPaginate<User>, GetListResponse<GetListUserListItemDto>>().ReverseMap();

        CreateMap<DeleteUserCommand, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt
                => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Id == src.Id)))
            .ForMember(dest => dest.CancellationToken, opt
                => opt.MapFrom<CancellationTokenResolver<DeleteUserCommand, GetModel<User>>>()).ReverseMap();
            
            
        CreateMap<UpdateUserCommand, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt
                => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Id == src.Id)))
            .ForMember(dest => dest.CancellationToken, opt
                => opt.MapFrom<CancellationTokenResolver<UpdateUserCommand, GetModel<User>>>()).ReverseMap();
        
        
        CreateMap<UpdateUserFromAuthCommand, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt
                => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Id == src.Id)))
            .ForMember(dest => dest.CancellationToken, opt
                => opt.MapFrom<CancellationTokenResolver<UpdateUserFromAuthCommand, GetModel<User>>>()).ReverseMap();
        
        CreateMap<GetUserByIdQuery, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt
                => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Id == src.Id)))
            .ForMember(dest => dest.CancellationToken, opt
                => opt.MapFrom<CancellationTokenResolver<GetUserByIdQuery, GetModel<User>>>()).ReverseMap();
        
    }
}