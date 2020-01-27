
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Prism.AppModel;
using Xamarin.Forms;

namespace ValidationSample.ViewModels
{
    public class MultipleInputPageViewModel : IPageLifecycleAware
    {
        /// <summary>
        /// 材料VM一覧
        /// </summary>
        public ObservableCollection<MaterialViewModel> Materials { get; set; }

        /// <summary>
        /// 登録コマンド
        /// </summary>
        public Command RegisterCommand { get; }

        public MultipleInputPageViewModel()
        {
            // 登録コマンド
            RegisterCommand = new Command(
                execute: _ => OnRegisterCommand(),
                canExecute: x => CanRegister());

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
            RegisterCommand.ChangeCanExecute();
        }

        /// <summary>
        /// 登録可能か判定する。
        /// </summary>
        /// <returns></returns>
        private bool CanRegister()
        {
            Debug.WriteLine("CanRegister()");

            // 1つでも材料が無効であれば登録不可。
            foreach(MaterialViewModel vm in Materials)
            {
                bool isValid = vm.ValidatableQuantity.IsValid();
                Debug.WriteLine($"{vm.Name} = {isValid}");
                if (!isValid)
                {
                    return false;
                }
            }

            // すべての材料が有効であれば登録可。
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
            if(Materials != null)
            {
                foreach(MaterialViewModel vm in Materials)
                {
                    vm.PropertyChanged -= OnMaterialPropertyChanged;
                }
            }
        }
    }
}
