using Application.Features.Categories.Commands.CreateCategory;
using Application.Interfaces;
using Application.Interfaces.UnitOfWork;
using Application.Models.Responses;
using AutoMapper;
using NSubstitute;
using MGH.Exceptions;
using Domain.Repositories;
using Moq;
using TestProject.Categories.Builders;
using TestProject.Categories.Fixtures;

namespace TestProject.Categories.Tests.Commands;

public class CreateCategoryCommandHandlerTests : IClassFixture<CreateCategoryCommandHandlerFixture>
{
    private readonly IMapper _mapper;

    //private readonly IUnitOfWork _unitOfWork;
    private readonly CreateCategoryCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUow;

    public CreateCategoryCommandHandlerTests(CreateCategoryCommandHandlerFixture createCategoryCommandHandlerFixture)
    {
        _mapper = createCategoryCommandHandlerFixture.Mapper;
        //_unitOfWork = createCategoryCommandHandlerFixture.UnitOfWork;
        _handler = createCategoryCommandHandlerFixture.CreateCategoryCommandHandler;
        _mockUow = createCategoryCommandHandlerFixture.UnitOfWorkMock;
    }


    [Fact]
    public async Task GivenDuplicateException_WhenTitleIsDuplicate_ThenThrows()
    {
        var dto = new CreateCategoryDtoBuilder()
            .WithCode(1)
            .WithDescription("desc")
            .WithTitle("title1")
            .Build();

        var result = await _handler
            .Handle(new CreateCategoryCommand { CreateCategory = dto }, CancellationToken.None);

        Assert.Equal(new ApiResponse
        (
            message: "Title is duplicate in Entity type: Domain.Entities.Shop.Category ",
            data: null,
            validationMessages: null
        ), result);
    }
}