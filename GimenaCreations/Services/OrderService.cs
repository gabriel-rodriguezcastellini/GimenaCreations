using GimenaCreations.Data;
using GimenaCreations.Entities;
using Microsoft.EntityFrameworkCore;

namespace GimenaCreations.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context) => _context = context;

    public async Task CreateOrderAsync(Order order)
    {
        await _context.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Order>> GetAllOrdersAsync(string userId) => await _context.Orders
        .Include(x => x.Items)
        .ThenInclude(x => x.Files)
        .OrderByDescending(x => x.Date)
        .Where(x => x.ApplicationUserId == userId)
        .Include(x => x.Items)
        .AsNoTracking()
        .ToListAsync();

    public async Task<Order> GetOrderByIdAsync(int id, string userId) =>
        await _context.Orders.Include(x => x.Address).Include(x => x.Items).Include(x => x.ApplicationUser).FirstAsync(x => x.Id == id && x.ApplicationUserId == userId);

    public async Task<Order> GetOrderByIdAsync(int id) =>
        await _context.Orders.Include(x => x.Address).Include(x => x.Items).FirstAsync(x => x.Id == id);

    public async Task UpdateOrderStatusAsync(Order order, OrderStatus orderStatus, string description = null)
    {
        order.Status = orderStatus;

        if (!string.IsNullOrWhiteSpace(description))
        {
            order.Description = description;
        }

        _context.Update(order);
        await _context.SaveChangesAsync();
    }
}
