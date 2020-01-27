using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ValidationSample.Views
{
    public partial class SimpleValidationPage : ContentPage
    {
        public SimpleValidationPage()
        {
            InitializeComponent();

            // ページ表示直後に検証動作を実行させる。
            Quantity.Text = "";
        }
    }
}
