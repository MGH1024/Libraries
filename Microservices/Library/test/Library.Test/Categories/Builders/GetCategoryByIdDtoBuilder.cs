using Application.Features.Categories.Queries.GetCategory;

namespace TestProject.Categories.Builders;

public class GetCategoryByIdDtoBuilder
{
    private int _id;

    public GetCategoryByIdDtoBuilder WithId(int id)
    {
        _id = id;
        return this;
    }


    public GetCategoryByIdDto Build()
    {
        return new GetCategoryByIdDto
        {
            Id = _id,
        };
    }
}