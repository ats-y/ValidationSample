using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using ValidationSample.Validation;

namespace ValidationSample.ViewModels
{
    /// <summary>
    /// 「検証＋コマンド」ページのViewModel
    /// </summary>
    public class WithCommandPageViewModel : BindableBase
    {
        public ReactiveProperty<string> ValidatableQuantity { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<TimeSpan> StartTime { get; set; } = new ReactiveProperty<TimeSpan>();
        public ReactiveProperty<TimeSpan> EndTime { get; set;  } = new ReactiveProperty<TimeSpan>();

        /// <summary>
        /// 使用量検証エラーメッセージ
        /// </summary>
        public ReactiveProperty<string> ValidationErrorMsg { get; } = new ReactiveProperty<string>();

        /// <summary>
        /// 登録コマンド
        /// </summary>
        public DelegateCommand RegisterCommand { get; }

        /// <summary>
        /// 使用量バリデータ
        /// </summary>
        private Validator<string> _quantityValidator;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WithCommandPageViewModel()
        {
            // 登録コマンド。
            // ObservesPropertyでCanRegister()を動かしたいプロパティを設定する。
            RegisterCommand = new DelegateCommand(
                () => OnRegisterCommand(),
                () => CanRegister())
                .ObservesProperty(() => StartTime.Value)
                .ObservesProperty(() => EndTime.Value)
                .ObservesProperty(() => ValidatableQuantity.Value);

            // 使用量バリデータの作成。
            _quantityValidator = new Validator<string>
            {
                Rules = new List<IValidationRule<string>>
                {
                    // 空チェック。
                    new IsNullOrEmptyValidationRule<string>{
                        ErrorMessage = "空でーす",
                    },
                    // 桁数チェック。
                    new IsDigitValidationRule<string>
                    {
                        ErrorMessage ="整数5桁、小数4桁じゃないでーす",
                        IntegerDigits = 5,
                        DecimalDigits = 4,
                    },
                },
            };

            ValidatableQuantity.Value = "";

            // 使用量変化の購読。
            ValidatableQuantity.Subscribe(x =>
            {
                Debug.WriteLine($"ValidatableQuantity.Subscribe {ValidatableQuantity.Value}");

                // 使用量を検証し、エラーメッセージを表示する。
                IEnumerable<string> errors = _quantityValidator.Errors(x);
                ValidationErrorMsg.Value = errors.FirstOrDefault();
            });

            Debug.WriteLine("WithCommandPageViewModel.ctor() end.");
        }

        /// <summary>
        /// 登録可能か判定する。
        /// </summary>
        /// <returns></returns>
        private bool CanRegister()
        {
            Debug.WriteLine("CanRegister()");

            // 使用量の検証結果が妥当でなければ登録不可。
            if (!_quantityValidator.IsValid(ValidatableQuantity.Value))
            {
                return false;
            }

            // 開始時間が終了時間よりあとであれば登録不可。
            if (EndTime.Value < StartTime.Value) return false;

            // 登録可。
            return true;
        }

        private void OnRegisterCommand()
        {
            Debug.WriteLine("OnRegisterCommand()");
        }
    }
}
