namespace EmployeeDirectory.Data.SummaryModels
{
    public class EmployeeSummary
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string MobileNumber { get; set; }
        public DateTime JoinDate { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string RoleId { get; set; }
        public string Role { get; set; }
        public string DepartmentId { get; set; }
        public string Department { get; set; }
        public string LocationId { get; set; }
        public string Location { get; set; }
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }
        public bool IsDeleted { get; set; }
    }

}
