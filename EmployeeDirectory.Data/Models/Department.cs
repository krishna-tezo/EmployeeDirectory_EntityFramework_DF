namespace EmployeeDirectory.Data.Models;

public partial class Department
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
