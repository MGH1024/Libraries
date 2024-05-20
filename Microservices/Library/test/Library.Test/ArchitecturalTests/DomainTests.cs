using NetArchTest.Rules;

namespace LibraryMicroservice.Test.ArchitecturalTests
{
    public class DomainTests : Base.ArchitecturalTestsBase
    {
        [Fact]
        public void DomainLayer_DoesNotHaveDependency_ToApplicationLayer()
        {
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
                .GetResult();

            AssertArchTestResult(result);
        }

        [Fact]
        public void DomainLayer_DoesNotHaveDependency_ToApplicationContractLayer()
        {
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
                .GetResult();

            AssertArchTestResult(result);
        }

        [Fact]
        public void DomainLayer_DoesNotHaveDependency_ToInfrastructureLayer()
        {
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
                .GetResult();

            AssertArchTestResult(result);
        }

        [Fact]
        public void DomainLayer_DoesNotHaveDependency_ToEndpoint()
        {
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(ApiAssembly.GetName().Name)
                .GetResult();

            AssertArchTestResult(result);
        }

        [Fact]
        public void DomainLayer_HaveDependency_ToDBPersistenceAssembly()
        {
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .HaveDependencyOn(PersistenceAssembly.GetName().Name);

            Assert.NotNull(result.GetTypes());
        }
    }
}
