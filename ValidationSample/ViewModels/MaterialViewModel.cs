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
        /// 検証機能付き使用量プロパティ。
        /// 入力Viewの値プロパティとバインディングする場合は、ValidatableObjectクラスが
        /// 管理している値のプロパティである「Value」プロパティとバインディングすること。
        /// </summary>
        public ValidatableObject<string> ValidatableQuantity { get; set; }

        /// <summary>
        /// 使用量検証エラーメッセージ
        /// </summary>
        public ReactiveProperty<string> ValidationErrorMsg { get; } = new ReactiveProperty<string>();

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
            ValidatableQuantity = new ValidatableObject<string>
            {
                Value = quantity.ToString(),
            };

            // 使用量が変更されたら妥当性を検証し、PropertyChangeイベントを発火する。
            ValidatableQuantity.PropertyChanged += (s, e) =>
            {
                Debug.WriteLine($"MaterialVM OnPropertyChanged({e.PropertyName})");
                ValidateQuantity();
                RaisePropertyChanged(e.PropertyName);
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
                ValidatableQuantity.Errors.FirstOrDefault();
        }
    }
}
