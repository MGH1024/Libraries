using Application.Interfaces;
using Application.Interfaces.UnitOfWork;
using Moq;
using Domain.Repositories;
using TestProject.Categories.Mocks;

namespace TestProject.Base.Mocks;

public static class MockUnitOfWork
{
    public static Mock<IUnitOfWork> GetUnitOfWork()
    {
        var mockUow = new Mock<IUnitOfWork>();
        var categoryRepositoryMock = CategoryRepositoryMock.GetCategoryRepository();
        mockUow.Setup(r => r.CategoryRepository).Returns(categoryRepositoryMock.Object);
        return mockUow;
    }
}