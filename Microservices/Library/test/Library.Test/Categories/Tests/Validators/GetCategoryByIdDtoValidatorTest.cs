using Application.Features.Categories.Queries.GetCategory;
using FluentValidation.TestHelper;
using TestProject.Categories.Builders;

namespace TestProject.Categories.Tests.Validators;

public class DeleteCategoryByIdDtoValidatorTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GivenInvalidId_WhenValidate_ThenWillInvalid(int id)
    {
        var dto = new GetCategoryByIdDtoBuilder()
            .WithId(id)
            .Build();

        var command = new GetCategoryQuery
        {
            Id = dto.Id,
        };

        var validator = new GetCategoryQueryValidator();
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}