using FluentAssertions;
using NetArchTest.Rules;
using Security.Infrastructure.Contexts;

namespace Security.Test.Architecture;
public class ArchitecturalTests
{
    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherLayers()
    {
        // Arrange
        var assembly = typeof(Domain.Repositories.IUserRepository).Assembly;
        var applicationAssembly = typeof(Application.Features.Users.Services.IUserService).Assembly.ToString();
        var infrastructureAssembly = typeof(SecurityDbContext).Assembly.ToString();
        var endpointAssembly = typeof(Endpoint.Api.Controllers.V1.UsersController).Assembly.ToString();

        var otherProjectAssembly = new[]
        {
            applicationAssembly,
            infrastructureAssembly,
            endpointAssembly
        };
        
        // Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjectAssembly)
            .GetResult();

        // Assert
       result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherLayers()
    {
        // Arrange
        var assembly = typeof(Application.Features.Users.Services.IUserService).Assembly;
        var infrastructureAssembly = typeof(SecurityDbContext).Assembly.ToString();
        var endpointAssembly = typeof(Endpoint.Api.Controllers.V1.UsersController).Assembly.ToString();

        var otherProjectAssembly = new[]
        {
            infrastructureAssembly,
            endpointAssembly
        };
        
        // Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjectAssembly)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherLayers()
    {
        // Arrange
        var assembly = typeof(SecurityDbContext).Assembly;
        var endpointAssembly = typeof(Endpoint.Api.Controllers.V1.UsersController).Assembly.ToString();

        var otherProjectAssembly = new[]
        {
            endpointAssembly
        };
        
        // Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjectAssembly)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}