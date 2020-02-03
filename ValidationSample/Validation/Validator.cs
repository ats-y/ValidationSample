using System.Collections.Generic;
using System.Linq;

namespace ValidationSample.Validation
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Validator<T>
    {
        /// <summary>
        /// 検証ルールリスト
        /// </summary>
        public List<IValidationRule<T>> Rules = new List<IValidationRule<T>>();

        public Validator()
        {
        }

        /// <summary>
        /// 値が妥当かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsValid(T value)
        {
            return !Errors(value).Any();
        }

        /// <summary>
        /// 検証ルールリストのうち、エラーだったルールのエラーメッセージ一覧を返す。
        /// </summary>
        public IEnumerable<string> Errors(T value)
        {
            IEnumerable<string> e = Rules
                .Where(v => !v.IsValidated(value))
                .Select(v => v.ErrorMessage);
            return e;
        }
    }
}
