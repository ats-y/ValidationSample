
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Prism.AppModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using ValidationSample.Models;
using ValidationSample.Validation;
using Xamarin.Forms;

namespace ValidationSample.ViewModels
{
    public class MultipleInputPageViewModel : BindableBase, IInitialize
    {
        /// <summary>
        /// 材料VM一覧
        /// </summary>
        public ObservableCollection<MaterialViewModel> Materials { get; } = new ObservableCollection<MaterialViewModel>();

        /// <summary>
        /// 開始日
        /// ReacrivePropertyで変更通知するパターン。
        /// </summary>
        public ReactiveProperty<DateTime> FromDate { get; } = new ReactiveProperty<DateTime>();

        /// <summary>
        /// 終了日
        /// PrismのSetPropertyで変更通知するパターン。
        /// </summary>
        private DateTime _toDate;
        public DateTime ToDate
        {
            get => _toDate;
            set => SetProperty(ref _toDate, value);
        }

        /// <summary>
        /// 登録コマンド
        /// </summary>
        public DelegateCommand RegisterCommand { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public MultipleInputPageViewModel()
        {
            // 登録コマンド。
            // 解説：
            // canExecuteMethodにこのコマンドが実行可能かどうか判断する処理を指定する。
            // ObservesPropertyに指定したプロパティが変化したらcanExecuteMethodが呼び出される。
            // ObservesPropertyには、INotifyPropertyChangedによる変更通知機能が備わった
            // プロパティを指定する（すなわち、PrismのSetPropertyやReactivePropertyでもOK）
            RegisterCommand = new DelegateCommand(
                executeMethod: () => OnRegisterCommand(),
                canExecuteMethod: () => CanRegister())
                    .ObservesProperty(() => FromDate.Value)
                    .ObservesProperty(() => ToDate);
        }

        /// <summary>
        /// Prism画面初期化。
        /// Prism Ver7.1以前のNavigatingTo()相当。
        /// </summary>
        /// <param name="parameters"></param>
        public void Initialize(INavigationParameters parameters)
        {
            // 開始日・終了日の初期値はシステム日付とする。
            DateTime now = DateTime.Now;
            FromDate.Value = now.Date;
            ToDate = now.Date;

            // parametersで渡ってきた薬剤一覧、と仮定。
            List<Material> inputMaterials = new List<Material>()
            {
                new Material
                {
                    Name = "材料1",
                    OrderQuantity = 0,
                },
                new Material
                {
                    Name = "材料2",
                    OrderQuantity = 10.5M,
                },
                new Material
                {
                    Name = "材料3",
                    OrderQuantity = 12.123456M,
                },
            };

            // 使用量の基本妥当性ルールを作成しておく。
            List<IValidationRule<string>> validations = new List<IValidationRule<string>>
            {
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

            // 材料ViewModelを生成する。
            foreach (Material item in inputMaterials)
            {
                // 基本妥当性ルールと材料ごとの依頼量超過妥当性ルールを材料ViewModelに設定する。
                MaterialViewModel mvm = new MaterialViewModel(
                                                item.Name, item.OrderQuantity );
                mvm.ValidatableQuantity.Rules.AddRange(validations);
                mvm.ValidatableQuantity.Rules.Add(
                    new MaximumValidationRule<string>(
                        item.OrderQuantity, $"依頼量({item.OrderQuantity})以下にしてね"));

                // 材料ViewModelの状態変化を監視する。
                mvm.PropertyChanged += OnMaterialPropertyChanged;
                Materials.Add(mvm);
            }
        }

        /// <summary>
        /// 材料一覧プロパティ変更イベントハンドラ。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaterialPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("OnMaterialPropertyChanged");

            // 登録コマンドの実行可否変化を通知する。
            RegisterCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// 登録可能か判定する。
        /// </summary>
        /// <returns></returns>
        private bool CanRegister()
        {
            Debug.WriteLine("CanRegister()");

            // 1つでも材料が無効であれば登録不可。
            if (Materials != null)
            {
                foreach (MaterialViewModel vm in Materials)
                {
                    bool isValid = vm.ValidatableQuantity.IsValid();
                    Debug.WriteLine($"{vm.Name} = {isValid}");
                    if (!isValid) return false;
                }
            }

            // 終了日が開始日より前の場合は登録不可。
            if (ToDate < FromDate.Value) return false;

            // 登録可。
            return true;
        }

        private void OnRegisterCommand()
        {
            Debug.WriteLine("OnRegisterCommand()");
        }
    }
}
