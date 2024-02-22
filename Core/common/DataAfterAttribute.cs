using System.ComponentModel.DataAnnotations;

public class DateAfterAttribute : ValidationAttribute {
    private readonly string _startDateProperty;

    public DateAfterAttribute(string startDateProperty) {
        _startDateProperty = startDateProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
        var startDateProperty = validationContext.ObjectType.GetProperty(_startDateProperty);

        if (startDateProperty == null) {
            return new ValidationResult($"Unknown property: {_startDateProperty}");
        }

        var startDateValue = (DateTime)startDateProperty.GetValue(validationContext.ObjectInstance, null);

        if ((DateTime)value <= startDateValue) {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}