using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<User> GetUser()
    {
        var userWithMaxValue = await _context.Users
            .Where(u => u.Orders.Any(o => o.Status == OrderStatus.Delivered && o.CreatedAt.Year == 2003))
            .Select(u => new
            {
                User = u,
                TotalDeliveredValue = u.Orders
                    .Where(o => o.Status == OrderStatus.Delivered && o.CreatedAt.Year == 2003)
                    .Sum(o => o.Price * o.Quantity)
            })
            .OrderByDescending(u => u.TotalDeliveredValue)
            .FirstOrDefaultAsync();

        return userWithMaxValue!.User;
    }

    public async Task<List<User>> GetUsers()
    {
        // temp
        return new List<User>();
    }
}