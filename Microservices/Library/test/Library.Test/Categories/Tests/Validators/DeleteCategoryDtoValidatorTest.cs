using Application.Features.Categories.Commands.DeleteCategory;
using FluentValidation.TestHelper;
using TestProject.Categories.Builders;

namespace TestProject.Categories.Tests.Validators;

public class DeleteCategoryDtoValidatorTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GivenInvalidId_WhenValidate_ThenWillInvalid(int id)
    {
        var dto = new DeleteCategoryDtoBuilder()
            .WithId(id)
            .Build();

        var command = new DeleteCategoryCommand
        {
            DeleteCategoryDto = dto,
        };
        var validator = new DeleteCategoryCommandValidator();
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.DeleteCategoryDto.Id);
    }
}