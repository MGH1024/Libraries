using MediatR;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.PublicLibraries.Constants;

namespace Library.Application.Features.PublicLibraries.Queries.GetList;

[Roles(PublicLibraryOperationClaims.GetAll)]
public class GetListQuery(PageRequest pageRequest) : IRequest<GetListResponse<GetListQueryResponse>>
{
    public PageRequest PageRequest { get; set; } = pageRequest;

    public GetListQuery() : this(new PageRequest { PageIndex = 0, PageSize = 10 })
    {
    }
}
