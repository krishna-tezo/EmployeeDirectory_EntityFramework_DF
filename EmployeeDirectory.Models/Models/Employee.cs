using Playground.Models.Interfaces;

namespace EmployeeDirectory.Models.Models;

public partial class Employee : IHasAuditProperties
{
    public string Id { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public DateOnly? Dob { get; set; }

    public string? MobileNumber { get; set; }

    public DateOnly? JoinDate { get; set; }

    public string? ProjectId { get; set; }

    public string? RoleId { get; set; }

    public bool? IsDeleted { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateOnly? CreatedDate { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateOnly? ModifiedDate { get; set; }

    public virtual ICollection<Manager> Managers { get; set; } = new List<Manager>();

    public virtual Project? Project { get; set; }

    public virtual Role? Role { get; set; }
}
