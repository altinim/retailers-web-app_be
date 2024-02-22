using System.ComponentModel.DataAnnotations;

namespace Core.Utilities {

    public class DateAfterAttribute : ValidationAttribute {
        public string OtherPropertyName { get; set; }

        public DateAfterAttribute(string otherPropertyName) {
            OtherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            var otherProperty = validationContext.ObjectType.GetProperty(OtherPropertyName);
            if (otherProperty == null) {
                return new ValidationResult($"Property {OtherPropertyName} does not exist.");
            }

            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance) as DateTime?;
            var thisValue = value as DateTime?;

            if (thisValue.HasValue && otherValue.HasValue && thisValue.Value <= otherValue.Value) {
                return new ValidationResult($"This date must be after {OtherPropertyName}.");
            }

            return ValidationResult.Success;
        }
    }
}
