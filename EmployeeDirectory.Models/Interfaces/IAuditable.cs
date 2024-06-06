namespace EmployeeDirectory.Models.Interfaces
{
    public interface IAuditable
    {
        DateOnly CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateOnly? ModifiedDate { get; set; }
        string? ModifiedBy { get; set; }


    }
}
