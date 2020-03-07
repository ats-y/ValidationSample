# バリデーションサンプル

```plantuml
'left to right direction

namespace Views{
    class MultipleInputPage{}
    class MaterialView{}

    MultipleInputPage -r-> "*" MaterialView
}

namespace ViewModels{
    class MultipleInputPageViewModel{}
    class MaterialViewModel{}

    MultipleInputPageViewModel -r-> "Materials *" MaterialViewModel
}

Views.MultipleInputPage --> "BindingContext" ViewModels.MultipleInputPageViewModel

Views.MaterialView --> "BindingContext" ViewModels.MaterialViewModel
```