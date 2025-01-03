namespace Security.Application.Features.Auth.Commands.RegisterUser;

public record RegisterUserCommandDto(string Email, string Password, string FirstName, string LastName);