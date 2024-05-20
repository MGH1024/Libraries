using NetArchTest.Rules;

namespace LibraryMicroservice.Test.ArchitecturalTests;

public class ApplicationTests : Base.ArchitecturalTestsBase
{
    [Fact]
    public void ApplicationLayer_DoesNotHaveDependency_ToInfrastructureLayer()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        AssertArchTestResult(result);
    }

    [Fact]
    public void ApplicationLayer_HaveDependency_ToApplicationContract()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .HaveDependencyOn(ApplicationAssembly.GetName().Name)
            .GetTypes();

        Assert.NotNull(result);
    }

    [Fact]
    public void ApplicationLayer_HaveDependency_ToSharedInfrastructureAssembly()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .HaveDependencyOn(FrameworkPersistenceAssembly.GetName().Name)
            .GetTypes();

        Assert.NotNull(result);
    }
}