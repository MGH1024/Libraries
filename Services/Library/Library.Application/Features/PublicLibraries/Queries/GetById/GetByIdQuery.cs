using MediatR;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.PublicLibraries.Constants;

namespace Library.Application.Features.PublicLibraries.Queries.GetById;

[Roles(PublicLibraryOperationClaims.GetById)]
public record GetByIdQuery(Guid Id)
    : IRequest<GetByIdQueryResponse>;
