using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Parser
{
    internal class Column : INotifyPropertyChanged
    {
        string _HeaderText;
        bool _Visible;
        public bool IsSignedToPropertyChange { get; set; }
        public int Index { get; set; }
        public int Width { get; set; }
        public Type DataType { get; set; }
        public int XStartPosition { get; set; }
        public int XEndPosition { get; set; }
        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
                OnPropertyChanged();
            }
        }
        public string HeaderText
        {
            get => _HeaderText;
            set
            {
                _HeaderText = value;
                Width = (Width > HeaderText.Length) ? Width : HeaderText.Length;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public Column(string headerText, Type type)
        {
            _HeaderText = headerText;
            Index = 0;
            Width = 1;

            DataType = type;
        }
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
