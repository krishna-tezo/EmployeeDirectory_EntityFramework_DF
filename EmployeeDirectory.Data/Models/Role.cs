namespace EmployeeDirectory.Data.Models;

public partial class Role
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? DepartmentId { get; set; }

    public string? Description { get; set; }

    public string? LocationId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Location? Location { get; set; }
}
