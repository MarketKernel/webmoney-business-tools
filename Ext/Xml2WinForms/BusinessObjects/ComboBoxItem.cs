using System;

namespace Xml2WinForms
{
    public class ComboBoxItem
    {
        private string _text;
        private object _value;

        public string Text
        {
            get => _text;
            set => _text = value ?? throw new ArgumentNullException(nameof(value));
        }

        public object Value
        {
            get => _value;
            set => _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        internal ComboBoxItem()
        {
        }

        public ComboBoxItem(string text, object value)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
