using JWTAPI.Entities;

namespace JWTAPI.Services;

public interface IOrderService
{
    List<Order> GetByStatus(int statusId);
    List<Order> GetByUser(int userId);
    Task<Order?> UpdateStatusAsync(int orderId, int statusId);
    Task<Order?> CreateAsync(Order order, string? login);
}