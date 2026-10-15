using System.Security.Claims;
using JWTAPI.Context;
using JWTAPI.Entities;
using JWTAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWTAPI.Controllers;

[ApiController]
[Route("api")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
 
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }
 
    [Authorize]
    [HttpGet("getorderstatus/{statusId}")]
    public IActionResult GetOrdersByStatus(int statusId)
    {
        return Ok(_orderService.GetByStatus(statusId));
    }
 
    [Authorize]
    [HttpGet("getuserorders/{userId}")]
    public IActionResult GetUserOrders(int userId)
    {
        return Ok(_orderService.GetByUser(userId));
    }
 
    [Authorize(Roles = "1")]
    [HttpPatch("patchorderstatus/{orderId}&{statusId}")]
    public async Task<IActionResult> PatchOrderStatus(int orderId, int statusId)
    {
        var order = await _orderService.UpdateStatusAsync(orderId, statusId);
        return order == null ? NotFound() : Ok(order);
    }
 
    [Authorize]
    [HttpPost("post/orders")]
    public async Task<IActionResult> PostOrder(Order order)
    {
        var login = User.FindFirst(ClaimTypes.Name)?.Value;
 
        var created = await _orderService.CreateAsync(order, login);
        return created == null ? Unauthorized() : Ok(created);
    }
}