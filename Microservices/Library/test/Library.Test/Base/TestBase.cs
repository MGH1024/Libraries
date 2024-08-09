using System.Reflection;
using Api.Controllers;
using Application;
using Domain;
using Domain.Entities.Libraries;
using NetArchTest.Rules;
using Persistence.Contexts;
using Persistence.Repositories;

namespace LibraryMicroservice.Test.Base;

public abstract class ArchitecturalTestsBase
{
    protected static Assembly ApplicationAssembly => typeof(ApplicationServiceRegistration).Assembly;
    protected static Assembly DomainAssembly => typeof(Library).Assembly;
    protected static Assembly InfrastructureAssembly => typeof(LibraryRepository).Assembly;
    protected static Assembly PersistenceAssembly => typeof(LibraryDbContext).Assembly;
    protected static Assembly ApiAssembly => typeof(LibrariesController).Assembly;
    protected static Assembly FrameworkPersistenceAssembly => typeof(IUow).Assembly;

    protected static void AssertAreImmutable(IEnumerable<Type> types)
    {
        IList<Type> failingTypes = new List<Type>();
        foreach (var type in types)
        {
            if (type.GetFields().Any(x => !x.IsInitOnly) || type.GetProperties().Any(x => x.CanWrite))
            {
                failingTypes.Add(type);
                break;
            }
        }

        Assert.Null(failingTypes);
    }

    protected static void AssertFailingTypes(IEnumerable<Type> types)
    {
        Assert.Null(types);
    }

    protected static void AssertArchTestResult(TestResult result)
    {
        AssertFailingTypes(result.FailingTypes);
    }
}