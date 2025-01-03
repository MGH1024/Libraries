using MGH.Core.Application.Responses;

namespace Security.Application.Features.Users.Commands.Update;

public record UpdatedUserResponse(int Id, string FirstName, string LastName, string Email, bool Status) : IResponse;