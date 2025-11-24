using MediatR;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.PublicLibraries.Constants;

namespace Library.Application.Features.PublicLibraries.Queries.GetById;

[Roles(PublicLibraryOperationClaims.GetById)]
public class GetByIdQuery : IRequest<GetByIdQueryResponse>
{
    public GetByIdQuery()
    {
            
    }

    public GetByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
