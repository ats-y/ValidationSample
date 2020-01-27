using System;
using System.Collections.Generic;
using System.Linq;

namespace ValidationSample.Validation
{
    /// <summary>
    /// 検証機能付きオブジェクト
    /// </summary>
    /// <typeparam name="T">対象の型</typeparam>
    public class ValidatableObject<T>
    {
        /// <summary>
        /// 値
        /// </summary>
        private T _value;
        public T Value
        {
            set { _value = value; }
            get
            {
                return _value;
            }
        }

        /// <summary>
        /// 検証ルールリスト
        /// </summary>
        public List<IValidationRule<T>> Rules = new List<IValidationRule<T>>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ValidatableObject()
        {
        }

        /// <summary>
        /// 値が妥当かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !Errors.Any();
        }

        /// <summary>
        /// 検証ルールリストのうち、エラーだったルールのエラーメッセージ一覧を返す。
        /// </summary>
        public IEnumerable<string> Errors
        {
            get
            {
                IEnumerable<string> e = Rules
                    .Where(v => !v.IsValidated(_value))
                    .Select(v => v.ErrorMessage);
                return e;
            }
        }
    }
}
