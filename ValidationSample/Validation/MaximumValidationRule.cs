using System;
namespace ValidationSample.Validation
{
    /// <summary>
    /// 最大値超過検証ルール。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MaximumValidationRule<T> : IValidationRule<T>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="maximum">最大値</param>
        /// <param name="error">エラーメッセージ</param>
        public MaximumValidationRule(decimal maximum, string error)
        {
            Maximum = maximum;
            ErrorMessage = error;
        }

        public string ErrorMessage { get; set; }

        public decimal Maximum { get; set; }

        public bool IsValidated(T value)
        {
            if (!(value is string strValue)) return false;
            if (!decimal.TryParse(strValue, out decimal decValue)) return false;
            return decValue <= Maximum;
        }
    }
}
