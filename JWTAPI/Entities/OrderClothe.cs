using System;
using System.Collections.Generic;

namespace JWTAPI.Entities;

public partial class OrderClothe
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ClothesId { get; set; }

    public virtual Clothe Clothes { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
