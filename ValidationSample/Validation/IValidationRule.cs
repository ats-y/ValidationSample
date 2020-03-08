namespace ValidationSample.Validation
{
    /// <summary>
    /// 検証ルールインターフェイス。
    /// </summary>
    /// <typeparam name="T">検証対象オブジェクトの型</typeparam>
    public interface IValidationRule<T>
    {
        /// <summary>
        /// 検証結果が妥当でない場合のメッセージ。
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        /// 値が妥当か検証する。
        /// </summary>
        /// <param name="value">検証対象の値</param>
        /// <returns>true：妥当</returns>
        bool IsValidated(T value);
    }
}
