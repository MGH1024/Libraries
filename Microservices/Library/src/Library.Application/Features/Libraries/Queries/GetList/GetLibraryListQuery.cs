using Application.Features.Libraries.Constants;
using MediatR;
using Domain.Entities.Libraries;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using Application.Features.Libraries.Extensions;
using MGH.Core.Application.Pipelines.Authorization;

namespace Application.Features.Libraries.Queries.GetList;

[Roles(LibraryOperationClaims.GetLibraries)]
public class GetLibraryListQuery(PageRequest pageRequest) : IRequest<GetListResponse<GetLibraryListDto>>
{
    public PageRequest PageRequest { get; set; } = pageRequest;

    public GetLibraryListQuery() : this(new PageRequest { PageIndex = 0, PageSize = 10 })
    {
    }
}

public class GetLibraryListQueryHandler(ILibraryRepository libraryRepository)
    : IRequestHandler<GetLibraryListQuery, GetListResponse<GetLibraryListDto>>
{
    public async Task<GetListResponse<GetLibraryListDto>> Handle(GetLibraryListQuery request,
        CancellationToken cancellationToken)
    {
        var libraries = await libraryRepository
            .GetDynamicListAsync(request.ToGetDynamicListAsyncModel(cancellationToken));
        return libraries.ToGetLibraryListDto();
    }
}