
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Prism.AppModel;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Xamarin.Forms;

namespace ValidationSample.ViewModels
{
    public class MultipleInputPageViewModel : BindableBase, IPageLifecycleAware
    {
        /// <summary>
        /// 材料VM一覧
        /// </summary>
        public ObservableCollection<MaterialViewModel> Materials { get; set; }

        /// <summary>
        /// 開始日
        /// ReacrivePropertyで変更通知。
        /// </summary>
        public ReactiveProperty<DateTime> FromDate { get; } = new ReactiveProperty<DateTime>();

        /// <summary>
        /// 終了日
        /// PrismのSetPropertyで変更通知。
        /// </summary>
        //public ReactiveProperty<DateTime> ToDate { get; } = new ReactiveProperty<DateTime>();
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
            // 開始日・終了日の初期値はシステム日付とする。
            DateTime now = DateTime.Now;
            FromDate.Value = now.Date;
            _toDate = now.Date;

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

            // 材料一覧をセット。
            Materials = new ObservableCollection<MaterialViewModel>
            {
                new MaterialViewModel
                {
                    Name = "材料1",
                },
                new MaterialViewModel
                {
                    Name = "材料2",
                },
                new MaterialViewModel
                {
                    Name = "材料3",
                },
            };

            // 材料一覧の各要素にPropertyChangedイベントハンドラを設定する。
            foreach (MaterialViewModel vm in Materials)
            {
                vm.PropertyChanged += OnMaterialPropertyChanged;
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
            foreach (MaterialViewModel vm in Materials)
            {
                bool isValid = vm.ValidatableQuantity.IsValid();
                Debug.WriteLine($"{vm.Name} = {isValid}");
                if (!isValid)
                {
                    return false;
                }
            }

            // 終了日が開始日より前の場合は登録不可。
            if (ToDate < FromDate.Value) return false;

            return true;
        }

        private void OnRegisterCommand()
        {
            Debug.WriteLine("OnRegisterCommand()");
        }

        public void OnAppearing()
        {
            Debug.WriteLine("IPageLifecycleAware.OnAppearing()");
        }

        public void OnDisappearing()
        {
            Debug.WriteLine("IPageLifecycleAware.OnDisappearing()");

            // TODO:子画面に行くときもイベント解除しちゃうかも。
            if (Materials != null)
            {
                foreach (MaterialViewModel vm in Materials)
                {
                    vm.PropertyChanged -= OnMaterialPropertyChanged;
                }
            }
        }
    }
}
