using FluentAssertions;
using NetArchTest.Rules;

namespace Security.Test.Architecture;

public class DesignTests
{
    [Fact]
    public void Handlers_Should_Not_HaveDependencyOnDomain()
    {
        // Arrange
        var assembly = typeof(Application.Features.Users.Services.IUserService).Assembly;

        // Act
        var testResult =
            Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Handler")
                .Should()
                .HaveDependencyOn(typeof(Domain.Repositories.IUserRepository).Assembly.ToString())
                .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Controllers_Should_HaveDependencyOnMediatR()
    {
        // Arrange
        var assembly = typeof(Endpoint.Api.Controllers.V1.UsersController).Assembly;

        // Act
        var testResult =
            Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Controller")
                .Should()
                .HaveDependencyOn("MediatR")
                .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
}