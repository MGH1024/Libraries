using AutoMapper;
using MGH.Core.Application.Requests;
using Library.Application.Features.PublicLibraries.Queries.GetById;

namespace Library.Endpoint.Api
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<PageRequest, GetByIdQuery>();
        }
    }
}
