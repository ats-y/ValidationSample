using System;
using System.ComponentModel.DataAnnotations;

namespace ValidationSample.Validation
{
    public class RangeValidationRule : ValidationAttribute
    {
        public int MinLimit;
        public int MaxLimit;

        public RangeValidationRule()
        {
        }

        public override bool IsValid(object value)
        {
            if (!int.TryParse(value.ToString(), out int target))
            {
                return false;
            }

            return (MinLimit <= target && target <= MaxLimit);
        }
    }
}
