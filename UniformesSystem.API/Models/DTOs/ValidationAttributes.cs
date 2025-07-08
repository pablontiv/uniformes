using System.ComponentModel.DataAnnotations;

namespace UniformesSystem.API.Models.DTOs
{
    public class PositiveNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is int intValue)
                return intValue > 0;
            if (value is decimal decimalValue)
                return decimalValue > 0;
            if (value is double doubleValue)
                return doubleValue > 0;
            if (value is float floatValue)
                return floatValue > 0;
            
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be a positive number.";
        }
    }

    public class NonNegativeNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is int intValue)
                return intValue >= 0;
            if (value is decimal decimalValue)
                return decimalValue >= 0;
            if (value is double doubleValue)
                return doubleValue >= 0;
            if (value is float floatValue)
                return floatValue >= 0;
            
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} cannot be negative.";
        }
    }
}