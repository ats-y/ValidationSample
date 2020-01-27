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
    /// <summary>
    /// 「検証＋コマンド」ページのViewModel
    /// </summary>
    public class WithCommandPageViewModel : BindableBase
    {
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
        public ReactiveCommand QuantityTextChangeCommand { get; } = new ReactiveCommand();

        /// <summary>
        /// 使用量検証エラーメッセージ
        /// </summary>
        public ReactiveProperty<string> ValidationErrorMsg { get; } = new ReactiveProperty<string>();

        /// <summary>
        /// 登録コマンド
        /// </summary>
        public Command RegisterCommand { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WithCommandPageViewModel()
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
            QuantityTextChangeCommand.Subscribe( x =>
            {
                Debug.WriteLine(x);

                // 使用量を検証結果を表示する。
                ValidationErrorMsg.Value = 
                    _validatableQuantity.Errors.FirstOrDefault();

                // 登録コマンドに登録可否の変更を通知する。
                Debug.WriteLine("call ChangeCanExecute()");
                RegisterCommand.ChangeCanExecute();
            });

            // 登録コマンド
            RegisterCommand = new Command(
                execute: _ => OnRegisterCommand(),
                canExecute: x => CanRegister() );

            // 使用量の初期値を設定する。
            // コンストラクタでの値設定では使用量変更コマンドは動かないので
            // 初期値の検証も行う。
            ValidatableQuantity.Value = "2.";
            ValidationErrorMsg.Value =
                _validatableQuantity.Errors.FirstOrDefault();

            Debug.WriteLine("WithCommandPageViewModel.ctor() end.");
        }

        /// <summary>
        /// 登録可能か判定する。
        /// </summary>
        /// <returns></returns>
        private bool CanRegister()
        {
            Debug.WriteLine("CanRegister()");

            // 使用量の検証結果が妥当であれば登録可能。
            return !_validatableQuantity.IsValid();
        }

        private void OnRegisterCommand()
        {
            Debug.WriteLine("OnRegisterCommand()");
        }
    }
}
