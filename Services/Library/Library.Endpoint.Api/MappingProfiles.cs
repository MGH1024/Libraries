using AutoMapper;
using MGH.Core.Application.Requests;
using Library.Application.Features.Libraries.Queries.GetList;

namespace Library.Endpoint.Api
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<PageRequest, GetLibraryListQuery>();
        }
    }
}
