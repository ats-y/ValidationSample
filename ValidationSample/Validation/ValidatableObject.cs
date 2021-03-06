﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ValidationSample.Validation
{
    /// <summary>
    /// 検証機能付きオブジェクト。
    /// T型のオブジェクトに検証機能をラッパーする。
    /// </summary>
    /// <typeparam name="T">対象の型</typeparam>
    public class ValidatableObject<T> : INotifyPropertyChanged
    {
        /// <summary>
        /// オブジェクト自身の値。
        /// </summary>
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                // 値を保存し、プロパティ変更イベントを発火する。
                _value = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        /// <summary>
        /// 検証ルールリスト。
        /// </summary>
        public List<IValidationRule<T>> Rules = new List<IValidationRule<T>>();

        /// <summary>
        /// 値変更イベント。
        /// （INotifyPropertyChangedの実装）
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public ValidatableObject()
        {
        }

        /// <summary>
        /// 値が妥当かどうか。
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
                    .Where(v => !v.IsValidated(Value))
                    .Select(v => v.ErrorMessage);
                return e;
            }
        }
    }
}
