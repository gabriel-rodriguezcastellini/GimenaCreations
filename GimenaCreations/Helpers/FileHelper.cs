namespace GimenaCreations.Helpers;

public class FileHelper : IFileHelper
{
    private readonly string[] permittedExtensions = { ".jpeg", ".jpg" };

    public async Task CreateFileAsync(string fileName, IFormFile formFile)
    {
        using var stream = File.Create(fileName);
        await formFile.CopyToAsync(stream);
    }

    public async Task<Tuple<byte[], string>> GetFileAsync(IFormFile formFile)
    {
        var ext = Path.GetExtension(formFile.FileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
        {
            return new Tuple<byte[], string>(null, "The file must be an image");
        }

        using var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);

        // Upload the file if less than 2 MB
        return memoryStream.Length < 2097152 ? new Tuple<byte[], string>(memoryStream.ToArray(), null) : new Tuple<byte[], string>(null, "The file is too large.");
    }
}
