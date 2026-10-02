using System;
using System.Collections.Generic;

namespace JWTAPI.Entities;

public partial class Size
{
    public int Id { get; set; }

    public string Size1 { get; set; } = null!;

    public int SizeNumber { get; set; }

    public int Count { get; set; }

    public virtual ICollection<ClothesSize> ClothesSizes { get; set; } = new List<ClothesSize>();
}
