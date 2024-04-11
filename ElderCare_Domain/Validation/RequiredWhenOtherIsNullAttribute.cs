using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Domain.Validation
{
    public class RequiredWhenOtherIsNullAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        // Set the name of the property to compare
        public RequiredWhenOtherIsNullAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var valueNotNull = value != null;
            var otherValueNotNull = validationContext.ObjectType.GetProperty(_comparisonProperty)!.GetValue(validationContext.ObjectInstance) != null;
            
            if (valueNotNull && otherValueNotNull)
            {
                return new ValidationResult(ErrorMessage = $"This attribute only have value when {_comparisonProperty} is NULL");
            }
            return ValidationResult.Success;
        }
    }
}
