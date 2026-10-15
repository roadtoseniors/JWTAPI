using JWTAPI.Context;
using JWTAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace JWTAPI.Services;

public class OrderService : IOrderService
{
    private readonly MyDbContext _context;
 
    public OrderService(MyDbContext context)
    {
        _context = context;
    }
 
    public List<Order> GetByStatus(int statusId)
    {
        return _context.Orders.Where(o => o.Status == statusId).ToList();
    }
 
    public List<Order> GetByUser(int userId)
    {
        return _context.Orders.Where(o => o.UserId == userId).ToList();
    }
 
    public async Task<Order?> UpdateStatusAsync(int orderId, int statusId)
    {
        var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
            return null;
 
        order.Status = statusId;
        await _context.SaveChangesAsync();
 
        return order;
    }
 
    public async Task<Order?> CreateAsync(Order order, string? login)
    {
        var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Login == login);
        if (currentUser == null)
            return null;
 
        order.UserId = currentUser.Id;
        order.DateTime = DateOnly.FromDateTime(DateTime.Now);
        order.Status = 1;
 
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
 
        return order;
    }
}