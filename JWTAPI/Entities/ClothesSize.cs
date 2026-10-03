using System;
using System.Collections.Generic;

namespace JWTAPI.Entities;

public partial class ClothesSize
{
    public int Id { get; set; }

    public int ClothesId { get; set; }

    public int SizeId { get; set; }

    public virtual Clothe Clothes { get; set; } = null!;

    public virtual Size Size { get; set; } = null!;
}
