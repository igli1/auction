using System.ComponentModel.DataAnnotations;

namespace auction.Helpers;
public class AllowedCharactersAttribute : ValidationAttribute
{
    private readonly string allowedCharacters;

    public AllowedCharactersAttribute(string allowedCharacters)
    {
        this.allowedCharacters = allowedCharacters;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null && value is string stringValue)
        {
            if (stringValue.Any(c => !allowedCharacters.Contains(c)))
            {
                return new ValidationResult($"The field {validationContext.DisplayName} contains invalid characters.");
            }
        }

        return ValidationResult.Success;
    }
}