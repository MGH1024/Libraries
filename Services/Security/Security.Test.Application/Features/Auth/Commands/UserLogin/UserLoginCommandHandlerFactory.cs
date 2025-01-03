using Security.Application.Features.Auth.Commands.UserLogin;
using Security.Test.Application.Base;

namespace Security.Test.Application.Features.Auth.Commands.UserLogin;

public static class UserLoginCommandHandlerFactory
{
    public static  UserLoginCommandHandler GetUserLoginCommandHandler(HandlerTestsFixture fixture)
    {
        return new UserLoginCommandHandler(fixture.MockUnitOfWork.Object,fixture.MockAuthService.Object,
            fixture.MockAuthBusinessRules.Object);
    }
}