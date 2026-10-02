using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JWTAPI.Entities;

public partial class Clothe
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public int CountPurchases { get; set; }

    public int TypeClothesId { get; set; }

    public bool IsAvailable { get; set; }
    [JsonIgnore]
    public virtual ICollection<ClothesSize> ClothesSizes { get; set; } = new List<ClothesSize>();
    [JsonIgnore]
    public virtual ICollection<OrderClothe> OrderClothes { get; set; } = new List<OrderClothe>();

    public virtual TypeClothe TypeClothes { get; set; } = null!;
}
