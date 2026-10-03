using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JWTAPI.Entities;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly DateTime { get; set; }

    public int Status { get; set; }

    public int UserId { get; set; }

    [JsonIgnore]
    public virtual ICollection<OrderClothe> OrderClothes { get; set; } = new List<OrderClothe>();

    public virtual Status StatusNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
