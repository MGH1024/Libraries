using IResponse = MGH.Core.Application.Responses.IResponse;

namespace Security.Application.Features.Users.Commands.Delete;

public record DeletedUserResponse(int Id) : IResponse;

