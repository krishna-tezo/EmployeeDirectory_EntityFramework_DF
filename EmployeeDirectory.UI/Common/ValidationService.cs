using EmployeeDirectory.Models.Models;
using System.Text.RegularExpressions;
namespace EmployeeDirectory.Core
{
    public class ValidationService : IValidationService
    {
        public ValidationResult ValidateEmail(string email)
        {

            string pattern = @"^[\w-\.]+@([\w]+\.)+[\w]{2,4}$";
            if (string.IsNullOrEmpty(email))
            {
                return ValidationResult.Fail("Empty Value");
            }
            else if (!Regex.Match(email, pattern).Success)
            {
                return ValidationResult.Fail("Invalid email format");
            }
            return ValidationResult.Success();
        }

        public ValidationResult ValidateMobileNumber(string number)
        {

            string pattern = @"\d{10}";
            if (string.IsNullOrEmpty(number))
            {
                return ValidationResult.Fail("Empty Value");
            }
            else if (!Regex.Match(number, pattern).Success)
            {
                return ValidationResult.Fail("Invalid Mobile Number");
            }
            return ValidationResult.Success();
        }

    }
}
