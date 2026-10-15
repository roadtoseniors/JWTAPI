using JWTAPI.Context;
using JWTAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAPI.Controllers;

[ApiController]
[Route("api")]
public class ClothesController : ControllerBase
{
    private readonly IClothesService _clothesService;
 
    public ClothesController(IClothesService clothesService)
    {
        _clothesService = clothesService;
    }
 
    [AllowAnonymous]
    [HttpGet("getclothes")]
    public IActionResult GetClothes()
    {
        return Ok(_clothesService.GetAll());
    }
 
    [AllowAnonymous]
    [HttpGet("getclothes/{name}")]
    public IActionResult GetClothesByName(string name)
    {
        return Ok(_clothesService.GetByName(name));
    }
 
    [Authorize(Roles = "1")]
    [HttpPatch("patchsizeclothes/{clotheId}&{sizeId}&{quantity}")]
    public async Task<IActionResult> PatchSizeClothes(int clotheId, int sizeId, int quantity)
    {
        var result = await _clothesService.UpdateSizeQuantityAsync(clotheId, sizeId, quantity);
        return result == null ? NotFound() : Ok(result);
    }
 
    [Authorize(Roles = "1")]
    [HttpPatch("patchclothesavalible/{clotheId}&{statusId}")]
    public async Task<IActionResult> PatchClothesAvailable(int clotheId, int statusId)
    {
        var result = await _clothesService.SetAvailabilityAsync(clotheId, statusId);
        return result == null ? NotFound() : Ok(result);
    }
}