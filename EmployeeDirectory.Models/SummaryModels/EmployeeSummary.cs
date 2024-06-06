using EmployeeDirectory.Models.Models;

namespace EmployeeDirectory.Models.SummaryModels
{
    public class EmployeeSummary
    {
        public Employee Employee { get; set; }
        public Role Role { get; set; }
        public Department Department { get; set; }
        public Location Location { get; set; }
        public Project Project { get; set; }
        public Manager Manager { get; set; }
        public string ManagerName { get; set; }
        public bool IsDeleted { get; set; }
    }

}
