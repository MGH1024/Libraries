using MediatR;
using Domain.Entities.Libraries;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using Application.Features.Libraries.Extensions;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Filters;

namespace Application.Features.Libraries.Queries.GetList;

public class GetLibraryListQuery(PageRequest pageRequest) : IRequest<GetListResponse<GetLibraryListDto>>
{
    public PageRequest PageRequest { get; set; } = pageRequest;

    public GetLibraryListQuery() : this(new PageRequest { PageIndex = 0, PageSize = 10 })
    {
    }

    public class GetLibraryListQueryHandler(ILibraryRepository libraryRepository)
        : IRequestHandler<GetLibraryListQuery, GetListResponse<GetLibraryListDto>>
    {
        public async Task<GetListResponse<GetLibraryListDto>> Handle(GetLibraryListQuery request,
            CancellationToken cancellationToken)
        {
            var dyn = new DynamicQuery();
            dyn.Filter = new Filter("Name", "eq", "string1_DistrictOne", "and", null);
            dyn.Sort = null;
            var libraries = await libraryRepository.GetDynamicListAsync(
                dynamic: dyn,
                predicate: null,
                include: null,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            return libraries.ToGetLibraryListDto();
        }
    }
}