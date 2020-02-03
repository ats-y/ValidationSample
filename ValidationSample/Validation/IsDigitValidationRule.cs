using System;
using System.Text.RegularExpressions;

namespace ValidationSample.Validation
{
    public class IsDigitValidationRule<T> : IValidationRule<T>
    {
        public uint IntegerDigits;
        public uint DecimalDigits;

        public string ErrorMessage { get; set; }

        public IsDigitValidationRule()
        {
        }

        public bool IsValidated(T value)
        {
            string str = value.ToString();
            //Regex regex = new Regex(@"(^[0-9]{1,5})+(\.[0-9]{1,4})?$");
            Regex regex = new Regex($"(^[0-9]{{1,{IntegerDigits}}})+(\\.[0-9]{{1,{DecimalDigits}}})?$");
            return regex.IsMatch(str);
        }
    }
}
