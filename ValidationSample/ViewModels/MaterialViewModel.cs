using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Prism.Mvvm;
using Reactive.Bindings;
using ValidationSample.Validation;
using Xamarin.Forms;

namespace ValidationSample.ViewModels
{
    public class MaterialViewModel : BindableBase
    {
        public string Name { get; set; }

        /// <summary>
        /// 検証機能付き使用量プロパティ
        /// </summary>
        private ValidatableObject<string> _validatableQuantity;
        public ValidatableObject<string> ValidatableQuantity
        {
            get
            {
                return _validatableQuantity;
            }

            set
            {
                SetProperty(ref _validatableQuantity, value);
            }
        }

        /// <summary>
        /// 使用量変更コマンド
        /// </summary>
        public ReactiveCommand QuantityChangeCommand { get; } = new ReactiveCommand();

        /// <summary>
        /// 使用量検証エラーメッセージ
        /// </summary>
        public ReactiveProperty<string> ValidationErrorMsg { get; } = new ReactiveProperty<string>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MaterialViewModel()
        {
            _validatableQuantity = new ValidatableObject<string>();
            _validatableQuantity.Rules = new List<IValidationRule<string>> {
                new IsNullOrEmptyValidationRule<string>
                {
                    ErrorMessage = "使用量を入力してください",
                },
                new IsDigitValidationRule<string>
                {
                    IntegerDigits = 5,
                    DecimalDigits = 4,
                    ErrorMessage = "整数5桁、小数4桁の数値を入力してください",
                },
            };

            // 使用量変更コマンドの購読。
            QuantityChangeCommand.Subscribe(x =>
            {
                Debug.WriteLine(x);

                // 使用量を検証結果を表示する。
                ValidationErrorMsg.Value =
                    _validatableQuantity.Errors.FirstOrDefault();

                // 登録コマンドに登録可否の変更を通知する。
                Debug.WriteLine("call ChangeCanExecute()");

                RaisePropertyChanged(nameof(ValidatableQuantity));
            });

            // 使用量の初期値を設定する。
            // コンストラクタでの値設定では使用量変更コマンドは動かないので
            // 初期値の検証も行う。
            ValidatableQuantity.Value = "2.";
            ValidationErrorMsg.Value =
                _validatableQuantity.Errors.FirstOrDefault();
        }
    }
}
