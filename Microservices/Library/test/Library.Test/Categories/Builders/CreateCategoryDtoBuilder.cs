using Application.Features.Categories.Commands.CreateCategory;

namespace TestProject.Categories.Builders;

public class CreateCategoryDtoBuilder
{
    private int _code;
    private string _title;
    private string _description;

    public CreateCategoryDtoBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public CreateCategoryDtoBuilder WithCode(int code)
    {
        _code = code;
        return this;
    }

    public CreateCategoryDtoBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public CreateCategoryDto Build()
    {
        return new CreateCategoryDto
        {
            Code = _code,
            Title = _title,
            Description = _description,
        };
    }
}