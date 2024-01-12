using Application.Features.Categories.Commands.UpdateCategory;

namespace TestProject.Categories.Builders;

public class UpdateCategoryDtoBuilder
{
    private int _code;
    private int _order;
    private string _title;
    private string _description;

    public UpdateCategoryDtoBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public UpdateCategoryDtoBuilder WithCode(int code)
    {
        _code = code;
        return this;
    }

    public UpdateCategoryDtoBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public UpdateCategoryDtoBuilder WithOrder(int order)
    {
        _order = order;
        return this;
    }

    public UpdateCategoryDto Build()
    {
        return new UpdateCategoryDto
        {
            Code = _code,
            Order = _order,
            Title = _title,
            Description = _description,
        };
    }
}