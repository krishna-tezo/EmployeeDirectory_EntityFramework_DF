
namespace EmployeeDirectory.Data.Models;
public partial class Project
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? ManagerId { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Manager? Manager { get; set; }
}
