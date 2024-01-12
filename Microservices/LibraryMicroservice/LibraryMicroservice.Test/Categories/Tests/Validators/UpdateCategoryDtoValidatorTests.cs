using Application.Features.Categories.Commands.UpdateCategory;
using FluentValidation.TestHelper;
using TestProject.Categories.Builders;

namespace TestProject.Categories.Tests.Validators;

public class UpdateCategoryDtoValidatorTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenEmptyTitle_WhenValidate_ThenWillInvalid(string title)
    {
        var dto = new UpdateCategoryDtoBuilder()
            .WithCode(1)
            .WithDescription("desc")
            .WithTitle(title)
            .Build();

        var command = new UpdateCategoryCommand
        {
            UpdateCategoryDto = dto,
        };

        var validator = new UpdateCategoryCommandValidator();
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateCategoryDto.Title);
    }


    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GivenInvalidCode_WhenValidate_ThenWillInvalid(int code)
    {
        var dto = new UpdateCategoryDtoBuilder()
            .WithCode(code)
            .WithDescription("desc")
            .WithTitle("title")
            .Build();

        var command = new UpdateCategoryCommand
        {
            UpdateCategoryDto = dto,
        };

        var validator = new UpdateCategoryCommandValidator();
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UpdateCategoryDto.Code);
    }
}