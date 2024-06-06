using EmployeeDirectory.Models.Models;

namespace EmployeeDirectory.Models.SummaryModels
{
    public class RoleSummary
    {
        public Role Role { get; set; }
        public Location Location { get; set; }
        public Department Department { get; set; }
        public string Description { get; set; }
        
    }
}
