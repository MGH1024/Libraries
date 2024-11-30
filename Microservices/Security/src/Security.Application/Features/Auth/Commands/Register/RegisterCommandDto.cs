namespace Application.Features.Auth.Commands.Register;

public record RegisterCommandDto(string Email, string Password, string FirstName, string LastName);