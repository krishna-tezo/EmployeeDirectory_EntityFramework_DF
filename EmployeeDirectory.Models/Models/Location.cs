using System;
using System.Collections.Generic;

namespace EmployeeDirectory.Models.Models;

public partial class Location
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
