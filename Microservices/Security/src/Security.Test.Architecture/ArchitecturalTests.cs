using FluentAssertions;

namespace Security.Test.Architecture;

using NetArchTest.Rules;
using Xunit;

public class ArchitecturalTests
{
    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherLayers()
    {
        // Arrange
        var assembly = typeof(Domain.Repositories.IUserRepository).Assembly;
        var applicationAssembly = typeof(Application.Models.ApiConfiguration).Assembly.ToString();
        var infrastructureAssembly = typeof(Persistence.Contexts.SecurityDbContext).Assembly.ToString();
        var endpointAssembly = typeof(Api.Controllers.AppController).Assembly.ToString();

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
        var assembly = typeof(Application.Models.ApiConfiguration).Assembly;
        var infrastructureAssembly = typeof(Persistence.Contexts.SecurityDbContext).Assembly.ToString();
        var endpointAssembly = typeof(Api.Controllers.AppController).Assembly.ToString();

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
        var assembly = typeof(Persistence.Contexts.SecurityDbContext).Assembly;
        var endpointAssembly = typeof(Api.Controllers.AppController).Assembly.ToString();

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