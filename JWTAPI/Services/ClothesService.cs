using JWTAPI.Context;
using JWTAPI.Entities;

namespace JWTAPI.Services;

public class ClothesService : IClothesService
{
    private readonly MyDbContext _context;
 
    public ClothesService(MyDbContext context)
    {
        _context = context;
    }
 
    public List<Clothe> GetAll()
    {
        return _context.Clothes.ToList();
    }
 
    public List<Clothe> GetByName(string name)
    {
        return _context.Clothes
            .Where(c => c.Name.ToLower().Contains(name.ToLower()))
            .ToList();
    }
 
    public async Task<ClothesSize?> UpdateSizeQuantityAsync(int clotheId, int sizeId, int quantity)
    {
        var clotheSize = _context.ClothesSizes
            .FirstOrDefault(x => x.ClothesId == clotheId && x.SizeId == sizeId);
 
        if (clotheSize == null)
            return null;
 
        clotheSize.Size.Count = quantity;
        await _context.SaveChangesAsync();
 
        return clotheSize;
    }
 
    public async Task<Clothe?> SetAvailabilityAsync(int clotheId, int statusId)
    {
        var clothe = _context.Clothes.FirstOrDefault(o => o.Id == clotheId);
        if (clothe == null)
            return null;
 
        clothe.IsAvailable = statusId != 0;
        await _context.SaveChangesAsync();
 
        return clothe;
    }
}