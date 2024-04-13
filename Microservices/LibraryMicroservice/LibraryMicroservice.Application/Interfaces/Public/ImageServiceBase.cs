using Microsoft.AspNetCore.Http;
using MGH.Core.CrossCutting.Exceptions.Types;

namespace Application.Interfaces.Public;

public abstract class ImageServiceBase
{
    public abstract Task<string> UploadAsync(IFormFile formFile);

    public async Task<string> UpdateAsync(IFormFile formFile, string imageUrl)
    {
        await FileMustBeInImageFormat(formFile);

        await DeleteAsync(imageUrl);
        return await UploadAsync(formFile);
    }

    public abstract Task DeleteAsync(string imageUrl);

    protected async Task FileMustBeInImageFormat(IFormFile formFile)
    {
        List<string> extensions = [".jpg", ".png", ".jpeg", ".webp"];

        var extension = Path.GetExtension(formFile.FileName)?.ToLower();
        if (!extensions.Contains(extension))
            throw new BusinessException("Unsupported format");
        await Task.CompletedTask;
    }
}
