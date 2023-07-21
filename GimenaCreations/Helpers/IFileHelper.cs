namespace GimenaCreations.Helpers;

public interface IFileHelper
{
    Task CreateFileAsync(string fileName, IFormFile formFile);
    Task<Tuple<byte[], string>> GetFileAsync(IFormFile formFile);
}
