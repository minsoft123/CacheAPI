using System.ComponentModel.DataAnnotations;

namespace CacheAPI.Attributes
{
    public class MinExpiration : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null && Convert.ToInt32(value) > 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Expiration must be greater than 0 (zero).");
            }
        }
    }
}
