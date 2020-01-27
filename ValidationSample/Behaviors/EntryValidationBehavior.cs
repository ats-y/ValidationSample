using System;
using Xamarin.Forms;

namespace ValidationSample.Behaviors
{
    public class EntryValidationBehavior : Behavior<Entry>
    {
        public EntryValidationBehavior()
        {
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            // TextChangedイベントハンドラの登録。
            bindable.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            // TextChangedイベントハンドラの解除。
            bindable.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(bindable);
        }

        /// <summary>
        /// 検証付きTextChangedイベントハンドラ。
        /// 入力値が空だったら背景を赤くする。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            Entry entry = sender as Entry;
            if (entry == null) return;
            if(string.IsNullOrEmpty(args.NewTextValue))
            {
                // 背景色を赤くする。
                entry.BackgroundColor = Color.Red;
            }
            else
            {
                // 背景色をなしにする。
                entry.BackgroundColor = Color.Transparent;
            }
        }
    }
}
