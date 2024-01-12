using Domain.Entities.Shop;
using Domain.Repositories;
using Moq;

namespace TestProject.Categories.Mocks;

public static class CategoryRepositoryMock
{
    public static Mock<ICategoryRepository> GetCategoryRepository()
    {
        var categories = new List<Category>
        {
            new Category
            {
                //Id = 1,
                Code = 1,
                Order = 1,
                Title = "title1",
                Description = "desc1",
            },
            new Category
            {
                //Id = 2,
                Code = 2,
                Order = 2,
                Title = "title2",
                Description = "desc2",
            }
        };

        var mockRepo = new Mock<ICategoryRepository>();

        mockRepo.Setup(r => r.GetAllAsync(new CancellationToken())).ReturnsAsync(categories);

        mockRepo.Setup(r => r.CreateCategoryAsync(It.IsAny<Category>(),It.IsAny<CancellationToken>()))
            .Returns(Task<Category> (Category category) =>
            {
                categories.Add(category);
                return Task.FromResult(category);
            });
           
            
           

        return mockRepo;
    }
}