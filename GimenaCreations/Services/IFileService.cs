namespace GimenaCreations.Services;

public interface IFileService
{
    Task<ICollection<Entities.File>> GetFilesToDownloadAsync(int orderId, string userId);
}
