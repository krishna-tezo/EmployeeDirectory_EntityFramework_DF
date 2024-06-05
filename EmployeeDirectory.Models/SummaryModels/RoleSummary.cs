using EmployeeDirectory.Models.Models;

namespace EmployeeDirectory.Models.SummaryModels
{
    public class RoleSummary
    {
        public RoleModel Role { get; set; }
        public LocationModel Location { get; set; }
        public DepartmentModel Department { get; set; }
        public string Description { get; set; }
        //public string Id { get; set; }
        //public string Name { get; set; }
        //public string DepartmentId { get; set; }
        //public string Department { get; set; }
        //public string LocationId { get; set; }
        //public string Location { get; set; }
    }
}
