using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Order> GetOrder()
    {
        var order = await _context.Orders
            .Include(o => o.User)
            .Where(o => o.Quantity > 1)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        return order!;
    }

    public  async Task<List<Order>> GetOrders()
    {
        var orders = await _context.Orders
            .Include(o => o.User)
            .Where(o => o.User.Status == UserStatus.Active)
            .OrderBy(o => o.CreatedAt)
            .ToListAsync();
        
        return orders;
    }
}