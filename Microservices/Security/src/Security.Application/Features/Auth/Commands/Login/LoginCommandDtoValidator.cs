using FluentValidation;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommandDtoValidator : AbstractValidator<LoginCommandDto>
{
    public LoginCommandDtoValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.Password).NotEmpty().MinimumLength(4);
    }
}