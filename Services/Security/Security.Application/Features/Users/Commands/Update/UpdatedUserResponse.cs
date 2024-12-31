using MGH.Core.Application.Responses;

namespace Application.Features.Users.Commands.Update;

public record UpdatedUserResponse(int Id, string FirstName, string LastName, string Email, bool Status) : IResponse;