using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JWTAPI.Entities;

public partial class Size
{
    public int Id { get; set; }

    public string Size1 { get; set; } = null!;

    public int SizeNumber { get; set; }

    public int Count { get; set; }
    [JsonIgnore]
    public virtual ICollection<ClothesSize> ClothesSizes { get; set; } = new List<ClothesSize>();
}
