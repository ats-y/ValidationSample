using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Prism.Mvvm;
using Reactive.Bindings;
using ValidationSample.Validation;
using Xamarin.Forms;

namespace ValidationSample.ViewModels
{
    /// <summary>
    /// 材料ViewModel
    /// </summary>
    public class MaterialViewModel : BindableBase
    {
        /// <summary>
        /// 材料名。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 検証機能付き使用量プロパティ
        /// </summary>
        private ValidatableObject<string> _validatableQuantity;
        public ValidatableObject<string> ValidatableQuantity
        {
            get => _validatableQuantity;
            set => SetProperty(ref _validatableQuantity, value);
        }

        /// <summary>
        /// 使用量検証エラーメッセージ
        /// </summary>
        public ReactiveProperty<string> ValidationErrorMsg { get; } = new ReactiveProperty<string>();

        /// <summary>
        /// 状態変更イベントハンドラ。
        /// </summary>
        /// <param name="sender"></param>
        public delegate void StatusChangedEventHandler(object sender);

        /// <summary>
        /// 材料ViewModel状態変更イベント。
        /// </summary>
        public event StatusChangedEventHandler StatusChanged;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MaterialViewModel()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">材料名</param>
        /// <param name="quantity">使用量</param>
        public MaterialViewModel(string name, decimal quantity)
        {
            // 属性を構築。
            Name = name;
            _validatableQuantity = new ValidatableObject<string>
            {
                Value = quantity.ToString(),
            };

            // 使用量が変更されたら妥当性を検証し、状態変更イベントを発火する。
            ValidatableQuantity.ValueChanged += (s) =>
            {
                ValidateQuantity();
                StatusChanged?.Invoke(this);
            };

            // 画面表示直後、初期値で妥当性検証結果を表示する。
            ValidateQuantity();
        }

        /// <summary>
        /// 使用量の妥当性を検証し、エラーがあれば表示する。
        /// </summary>
        private void ValidateQuantity()
        {
            ValidationErrorMsg.Value =
                _validatableQuantity.Errors.FirstOrDefault();
        }
    }
}
