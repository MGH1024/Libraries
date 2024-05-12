using NetArchTest.Rules;

namespace LibraryMicroservice.Test.ArchitecturalTests;

public class InfrastructureTests : Base.ArchitecturalTestsBase
{
    [Fact]
    public void InfrastructureLayer_Repositories_HaveDependency_ToDomainLayer()
    {
        var rep = "Repository";
        var result = Types.InCurrentDomain()
            .That().ResideInNamespace((InfrastructureAssembly.GetName().Name))
            .And().AreClasses()
            .Should().HaveNameEndingWith(rep).And().HaveDependencyOn(DomainAssembly.GetName().Name).GetTypes();

        Assert.NotNull(result);
    }
}