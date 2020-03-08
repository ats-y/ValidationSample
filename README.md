# バリデーションサンプル

## 複数入力項目の検証

サンプル画面：MultipleInputPage
```plantuml
'left to right direction

namespace Views{
    class MultipleInputPage{}
    note left of Views.MultipleInputPage : 複数入力項目の検証画面
    
    class MaterialView{}
    note right of Views.MaterialView : 材料表示エリア(一行分)

    MultipleInputPage -r-> "*" MaterialView
}

namespace ViewModels{
    class MultipleInputPageViewModel{}
    class MaterialViewModel{}

    MultipleInputPageViewModel -r-> "Materials *" MaterialViewModel
}

note right of ViewModels.MaterialViewModel : 材料表示エリア(一行分)の\nViewModel。
namespace Validation{
    class ValidatableObject<string>{
        bool IsValid()
        IEnumerable<string> Errors
    }
    note left : 検証機能付きオブジェクト。
    interface IValidationRule<T>{}
    note left : 妥当性ルール。

    ValidatableObject --> "Rules *" IValidationRule

    class IsDigitValidationRule
    note bottom : 整数・小数桁ルール。
    IsDigitValidationRule .u.|> IValidationRule

    class IsNullOrEmptyValidationRule
    note bottom : 未入力ルール。
    IsNullOrEmptyValidationRule .u.|> IValidationRule

    class MaximumValidationRule
    note bottom : 最大値ルール。
    MaximumValidationRule .u.|> IValidationRule
}

Views.MultipleInputPage --> "BindingContext" ViewModels.MultipleInputPageViewModel

Views.MaterialView --> "BindingContext" ViewModels.MaterialViewModel

ViewModels.MaterialViewModel --> "ValidatableQuantity" Validation.ValidatableObject
note on link 
    妥当性機能付き材料量。
end note
```

### できること
#### 1.動的なViewの表示。

StackLayoutのBindableLayoutを利用して複数の材料情報を一覧形式で表示する。

#### 2.妥当性の検証。

妥当性検証機能付きオブジェクトに任意の妥当性ルールを追加し検証する。

参考：Microsoft エンタープライズアプリケーションパターン - 検証
https://docs.microsoft.com/ja-jp/xamarin/xamarin-forms/enterprise-application-patterns/validation

#### 3.DelegateCommandによるボタン活性制御。

複数のプロパティを監視し、それらが条件を満たした時だけボタンを活性化する。

---
### DelegateCommandによるボタン活性制御の解説。

サンプルは「登録」ボタン。<br>
各材料の使用量と開始日・終了日の日付順が全て妥当な場合に登録ボタンを活性化する。

* 使用量の妥当性
    * 値が入力されていること。
    * 入力内容が数値で、整数桁・小数桁がそれぞれ5桁・4桁以内であること。
    * 画面表示直後の初期表示値以下であること。
* 終了日・開始日の妥当性
    * 終了日が開始日以降の日付であること。

** MultipleInputPageViewModel.cs 抜粋 **
```C#
// 複数入力項目の検証画面のViewModel
public class MultipleInputPageViewModel : BindableBase, IInitialize
{
    // 登録ボタンコマンド。
    public DelegateCommand RegisterCommand { get; }

    // 開始日
    // （ReacrivePropertyで変更通知するパターン）
    public ReactiveProperty<DateTime> FromDate { get; } = new ReactiveProperty<DateTime>();

    // 終了日
    // （PrismのSetPropertyで変更通知するパターン）
    private DateTime _toDate;
    public DateTime ToDate
    {
        get => _toDate;
        set => SetProperty(ref _toDate, value);
    }

    // コンストラクタ
    public MultipleInputPageViewModel()
    {
        RegisterCommand = new DelegateCommand(
            executeMethod: () => OnRegisterCommand(),
            canExecuteMethod: () => CanRegister())
                .ObservesProperty(() => FromDate.Value)
                .ObservesProperty(() => ToDate);
    }

    // 登録ボタンを活性化させる場合にtrueを返す。
    private bool CanRegister() { /* 略。 */ }

    // 登録ボタンコマンド処理（タップされた時の処理）
    private void OnRegisterCommand() { /* 略。 */ }
}
```

#### DelegateCommandのポイント

登録ボタンのCommandプロパティをDelegateCommand型でバインディングする。

DelegateCommandコンストラクタの引数canExecuteMethodに登録ボタンの活性化判定を行う処理を設定する。この処理がtrueを返すとボタンが活性化する。

DelegateCommandのObservesProperty()に監視したいプロパティを設定する。<br>
これでそのプロパティ値が変更されるとコンストラクタの引数canExecuteMethodに設定した処理が呼び出されるようになる。<br>

> この仕組みはINotifyPropertyChangedで実現しているので、ViewModelにINotifyPropertyChangedを実装するか、PrismのBindableBaseを継承する必要がある。

#### 材料の使用量が変更された時にcanExecuteMethodを発火させる方法

DelegateCommandのRaiseCanExecuteChanged()を呼び出すことでcanExecuteMethodが発火し登録ボタンの活性制御が行われる。

** MultipleInputPageViewModel.cs 抜粋 **
```C#
/// 材料一覧プロパティ変更イベントハンドラ。
private void OnMaterialPropertyChanged(object sender, PropertyChangedEventArgs e)
{
    RegisterCommand.RaiseCanExecuteChanged();
}
```


