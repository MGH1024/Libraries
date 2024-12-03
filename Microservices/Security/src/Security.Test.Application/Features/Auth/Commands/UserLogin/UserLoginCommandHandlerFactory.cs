using Application.Features.Auth.Commands.UserLogin;
using Security.Test.Base;

namespace Security.Test.Features.Auth.Commands.UserLogin;

public static class UserLoginCommandHandlerFactory
{
    public static  UserLoginCommandHandler GetUserLoginCommandHandler(HandlerTestsFixture fixture)
    {
        return new UserLoginCommandHandler(fixture.MockUnitOfWork.Object,fixture.MockAuthService.Object,
            fixture.MockAuthBusinessRules.Object);
    }
}