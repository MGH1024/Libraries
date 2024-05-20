using Application.Features.Categories.Commands.DeleteCategory;

namespace TestProject.Categories.Builders;

public class DeleteCategoryDtoBuilder
{
    private int _id;

    public DeleteCategoryDtoBuilder WithId(int id)
    {
        _id = id;
        return this;
    }


    public DeleteCategoryDto Build()
    {
        return new DeleteCategoryDto
        {
            Id = _id,
        };
    }
}