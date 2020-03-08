# バリデーションサンプル

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