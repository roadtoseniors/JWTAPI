namespace JWTAPI.Entities.Dtos;

public record ClotheDto
{
    public int Id {get; init;}
    public string Name { get; init; }
    public decimal Price { get; init; }
    public string Description { get; init; }
    public int CountPurchases { get; init; }
    public bool IsAvialable {get; init; }
}