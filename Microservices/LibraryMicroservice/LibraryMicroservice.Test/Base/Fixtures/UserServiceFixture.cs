using Application.Interfaces.Security;
using NSubstitute;

namespace TestProject.Base.Fixtures;

public class UserServiceFixture
{
    public readonly IUserService UserService;

    public UserServiceFixture()
    {
        UserService = Substitute.For<IUserService>();
    }
}