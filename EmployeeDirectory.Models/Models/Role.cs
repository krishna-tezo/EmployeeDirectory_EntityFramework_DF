namespace EmployeeDirectory.Models
{
    public class Role
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? DepartmentId { get; set; }
        public string? Description { get; set; }
        public string? LocationId { get; set; }
        public override string ToString()
        {
            return $"RoleId:{Id} , Name {Name}";
        }
    }
}
