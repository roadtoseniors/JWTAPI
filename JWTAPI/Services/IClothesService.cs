using JWTAPI.Entities;

namespace JWTAPI.Services;

public interface IClothesService
{
    List<Clothe> GetAll();
    List<Clothe> GetByName(string name);
    Task<ClothesSize?> UpdateSizeQuantityAsync(int clotheId, int sizeId, int quantity);
    Task<Clothe?> SetAvailabilityAsync(int clotheId, int statusId);
}