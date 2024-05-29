namespace EmployeeDirectory.Models
{
    public class Employee
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime DOB { get; set; }
        public string? MobileNumber { get; set; }
        public DateTime JoinDate { get; set; }
        public string ProjectId {  get; set; }
 
        public string RoleId { get; set; }
        public bool IsDeleted { get; set; }

        public override string ToString()
        {
            return $"EmpId: {Id} , Name: {FirstName} {LastName} , RoleId: {RoleId}";
        }
        
    }
}