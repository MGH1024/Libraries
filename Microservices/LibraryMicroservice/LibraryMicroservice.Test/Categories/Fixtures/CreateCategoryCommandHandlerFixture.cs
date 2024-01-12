using Application.Features.Categories.Commands.CreateCategory;
using Application.Features.Categories.Rules;
using AutoMapper;
using NSubstitute;
using Application.Interfaces.UnitOfWork;
using Application.Interfaces.Validation;
using Moq;
using TestProject.Base.Fixtures;
using TestProject.Base.Mocks;

namespace TestProject.Categories.Fixtures;

public class CreateCategoryCommandHandlerFixture
{
    public readonly IMapper Mapper;

    //public readonly IUnitOfWork UnitOfWork;
    public readonly IValidationService ValidationService;
    public CreateCategoryCommandHandler CreateCategoryCommandHandler;
    public Mock<IUnitOfWork> UnitOfWorkMock;
    public CategoryBusinessRules CategoryBusinessRules;

    public CreateCategoryCommandHandlerFixture()
    {
        Mapper = Substitute.For<IMapper>();
        //UnitOfWork = Substitute.For<IUnitOfWork>();
        ValidationService = new ValidationServiceFixture().ValidationService;
        UnitOfWorkMock = MockUnitOfWork.GetUnitOfWork();
        CategoryBusinessRules = new CategoryBusinessRules();
        CreateCategoryCommandHandler =
            new CreateCategoryCommandHandler(Mapper, UnitOfWorkMock.Object, ValidationService, CategoryBusinessRules);
    }
}