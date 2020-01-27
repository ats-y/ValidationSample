using System;
namespace ValidationSample.Validation
{
    /// <summary>
    /// 検証ルールインタフェイス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValidationRule<T>
    {
        /// <summary>
        /// 検証エラーメッセージ
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        /// 値を検証する
        /// </summary>
        /// <param name="value">検証対象の値</param>
        /// <returns>true：検証OK</returns>
        bool IsValidated(T value);
    }
}
