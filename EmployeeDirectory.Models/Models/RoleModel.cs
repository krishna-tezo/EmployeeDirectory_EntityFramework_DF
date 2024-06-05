namespace EmployeeDirectory.Models.Models;

public partial class RoleModel
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? DepartmentId { get; set; }

    public string? Description { get; set; }

    public string? LocationId { get; set; }

}
