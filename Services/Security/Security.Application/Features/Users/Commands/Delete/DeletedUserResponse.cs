using Nest;
using IResponse = MGH.Core.Application.Responses.IResponse;

namespace Application.Features.Users.Commands.Delete;

public record DeletedUserResponse(int Id) : IResponse;
