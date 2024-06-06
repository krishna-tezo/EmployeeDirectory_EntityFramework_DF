namespace EmployeeDirectory.Models.Models
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }

        public static ValidationResult Success(string errorMessage = null!)
        {
            return new ValidationResult
            {
                IsValid = true,
                ErrorMessage = errorMessage
            };
        }

        public static ValidationResult Fail(string errorMessage)
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
