using Domain.Entities.Libraries;
using NetArchTest.Rules;

namespace LibraryMicroservice.Test.ArchitecturalTests;

public class ArchTest
{
    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly =  typeof(Library).Assembly;
        //Act
        var result = Types.InAssembly(assembly)
            .Should()
            .NotHaveDependencyOn("LibraryMicroservice.Application")
            .And()
            .NotHaveDependencyOn("LibraryMicroservice.Persistence")
            .And()
            .NotHaveDependencyOn("LibraryMicroservice.Infrastructures")
            .And()
            .NotHaveDependencyOn("LibraryMicroservice.API")
            .GetResult();
        //Assert
        Assert.True(result.IsSuccessful);
    }
}