namespace EmployeeDirectory.Core
{
    public interface IValidator
    {
        ValidationResult ValidateEmail(string email);
        ValidationResult ValidateMobileNumber(string number);
    }
}