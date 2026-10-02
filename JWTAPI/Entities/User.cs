using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JWTAPI.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Phone { get; set; } = null!;

    public string? Adress { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Role { get; set; }
    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role RoleNavigation { get; set; } = null!;
}
