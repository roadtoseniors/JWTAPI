using System;
using System.Collections.Generic;

namespace JWTAPI.Entities;

public partial class TypeClothe
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Clothe> Clothes { get; set; } = new List<Clothe>();
}
