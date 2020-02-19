using Parser.Properties;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Parser
{
    internal class Column : INotifyPropertyChanged
    {
        string _HeaderText;
        bool _Visible;
        Type _DataType;
        public bool IsSignedToPropertyChange { get; set; }
        public int Index { get; set; }
        public int Width { get; set; }
        public Type DataType 
        { get
            { return _DataType; }
            set
            {
                _DataType = value;
                SetDefaultDataFormat();
            } 
        }
        public string DataFormat { get; set; }
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
            SetDefaultDataFormat();
        }
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private void SetDefaultDataFormat()
        {
            if (DataType == typeof(DateTime))
            {
                DataFormat = Resources.DefaultDataFormat;
            }
            else
            { 
                DataFormat = "";
            }
        }
    }
}
