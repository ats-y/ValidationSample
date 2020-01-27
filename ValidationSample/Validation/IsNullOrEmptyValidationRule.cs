using System;
namespace ValidationSample.Validation
{
    public class IsNullOrEmptyValidationRule<T> : IValidationRule<T>
    {
        public IsNullOrEmptyValidationRule()
        {
        }

        public string ErrorMessage { get; set; }

        public bool IsValidated(T value)
        {
            string str = value as string;
            return !string.IsNullOrEmpty(str);
        }
    }
}
