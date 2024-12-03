using FluentValidation;

namespace Application.Features.Auth.Commands.UserLogin;

public class UserLoginCommandDtoValidator : AbstractValidator<UserLoginCommandDto>
{
    public UserLoginCommandDtoValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.Password).NotEmpty().MinimumLength(4);
    }
}