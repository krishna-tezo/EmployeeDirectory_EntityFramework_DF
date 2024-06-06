using EmployeeDirectory.Models.Models;

namespace EmployeeDirectory.Core
{
    public interface IValidationService
    {
        ValidationResult ValidateEmail(string email);
        ValidationResult ValidateMobileNumber(string number);
    }
}