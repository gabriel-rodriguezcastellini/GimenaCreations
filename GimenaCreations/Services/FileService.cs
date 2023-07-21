using GimenaCreations.Data;
using Microsoft.EntityFrameworkCore;

namespace GimenaCreations.Services;

public class FileService : IFileService
{
    private readonly ApplicationDbContext _context;

    public FileService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Entities.File>> GetFilesToDownloadAsync(int orderId, string userId)
    {
        return await _context.Files.Include(x => x.OrderItem).ThenInclude(x=>x.Order).Where(x => x.OrderItem.OrderId == orderId && x.OrderItem.Order.ApplicationUserId == userId)
            .ToListAsync();
    }
}
