namespace Library.Application.Features.PublicLibraries.Queries.GetList;

public record GetListQueryResponse(
    Guid Id,
    string Code,
    string Title,
    string Location,
    string District,
    DateTime CreatedAt
);
