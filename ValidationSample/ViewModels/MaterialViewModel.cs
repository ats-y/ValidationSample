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
        /// コンストラクタ
        /// </summary>
        public MaterialViewModel()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">薬剤名</param>
        /// <param name="quantity">使用量</param>
        public MaterialViewModel(string name, decimal quantity)
        {
            Name = name;
            _validatableQuantity = new ValidatableObject<string>
            {
                Value = quantity.ToString(),
            };

            ValidatableQuantity.ValueChanged += (s) =>
            {
                ValidateQuantity();
            };

            // 画面表示直後、初期値で妥当性検証結果を表示する。
            ValidateQuantity();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ValidateQuantity()
        {
            // 使用量を検証結果を表示する。
            ValidationErrorMsg.Value =
                _validatableQuantity.Errors.FirstOrDefault();

            // 登録コマンドに登録可否の変更を通知する。
            Debug.WriteLine("call ChangeCanExecute()");
            RaisePropertyChanged(nameof(ValidatableQuantity));
        }
    }
}
