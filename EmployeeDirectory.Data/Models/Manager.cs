namespace EmployeeDirectory.Data.Models;
public partial class Manager
{
    public string Id { get; set; } = null!;

    public string? EmpId { get; set; }

    public virtual Employee? Emp { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
