using Library.Application.Features.Libraries.Constants;
using Library.Application.Features.Libraries.Profiles;
using Library.Domain.Libraries;
using MediatR;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;

namespace Library.Application.Features.Libraries.Queries.GetList;

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