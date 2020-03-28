using Parser.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Parser
{


    class Source
    {
        public ColumnDataTypesList DataTypes { get; set; } = new ColumnDataTypesList();
        public SortDirections _SortDirection = SortDirections.None;
        public SortDirections SortDirection
        {
            get => _SortDirection;
            set
            {
                _SortDirection = value;
                OnPropertyChanged();
            }
        }
        public int SortedColumnIndex { get; set; } = -1;
        public TypeSelector TypeSelector { get; set; } = new TypeSelector();
        public Control EditorControl { get; set; } = new Control();
        public bool IsEditorOpened { get; set; }
        public bool IsEditorUsed { get; set; }
        public bool IsTypeSelectorOpened { get; set; }
        public ObservableCollection<Column> Columns { get; } = new ObservableCollection<Column>();
        public event PropertyChangedEventHandler PropertyChanged;
        public Source()
        {
            Columns.CollectionChanged += ColumnsChannge;
        }
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private void ColumnsChannge(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                int index = 0;
                foreach (var item in Columns)
                {
                    item.Index = index;
                    index++;

                }
                SortDirection = SortDirections.None;
                SortedColumnIndex = -1;

            }
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                int newIndex = e.NewStartingIndex;
                int columnsCount = 0;
                foreach (var item in Columns)
                {
                    if (item.Index == Columns[newIndex].Index)
                    {
                        columnsCount++;
                    }
                }
                if (columnsCount > 1)
                {
                    int maxColumnsIndex = Columns.Max(k => k.Index);
                    Columns[newIndex].Index = maxColumnsIndex + 1;
                }

            }
        }

    }
    class Cell
    {
        public string Body { get; set; }
        public int SourceColumnIndex { get; set; }
        public int BuferRowIndex { get; set; }
        public int SourceRowIndex { get; set; }

    }
    class Row
    {
        public List<Cell> Cells { get; set; } = new List<Cell>();

    }
    internal class Column
    {
        string _HeaderText;
        bool _Visible;
        Type _DataType;
        public bool IsSignedToPropertyChange { get; set; }
        public int Index { get; set; }
        public int Width { get; set; }
        public Type DataType
        {
            get
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
    class RowComparer : IComparer<Row>
    {
        bool _Direction;
        int _ColumnIndex;
        Type _Type;
        public RowComparer(bool direction, int index, Type t)
        {
            _Direction = direction;
            _ColumnIndex = index;
            _Type = t;
        }
        public int Compare(Row x, Row y)
        {
            object xValue = null;
            object yValue = null;
            if (x.Cells[_ColumnIndex].Body != Resources.UndefinedFieldText)
            {
                try
                {
                    xValue = Convert.ChangeType(x.Cells[_ColumnIndex].Body, _Type);
                }
                catch
                {
                    xValue = null;
                }
            }
            if (y.Cells[_ColumnIndex].Body != Resources.UndefinedFieldText)
            {
                try
                {
                    yValue = Convert.ChangeType(y.Cells[_ColumnIndex].Body, _Type);
                }
                catch
                {
                    yValue = null;
                }
            }

            int dir = (_Direction) ? 1 : -1;

            if (xValue != null && xValue is IComparable)
            {

                return (xValue as IComparable).CompareTo(yValue) * ((_Direction) ? 1 : -1);
            }
            if (yValue != null && xValue is IComparable)
            {
                return (yValue as IComparable).CompareTo(xValue) * ((_Direction) ? 1 : -1);
            }
            else
            {
                return (x.Cells[_ColumnIndex].Body as string).CompareTo(y.Cells[_ColumnIndex].Body as string) * ((_Direction) ? 1 : -1);
            }

        }
    }

    public enum SortDirections
    {
        ASC,
        DESC,
        None
    }

    class ColumnDataTypesList
    {
        internal Dictionary<string, Type> TypesCollection { get; } = new Dictionary<string, Type>();
        public ColumnDataTypesList()
        {
            TypesCollection.Add("String", typeof(String));
            TypesCollection.Add("Integer", typeof(Int32));
            TypesCollection.Add("Date/Time", typeof(DateTime));
            TypesCollection.Add("Boolean", typeof(Boolean));
        }


        public string GetKeyByValue(Type t)
        {
            string res = "";
            foreach (var item in TypesCollection)
            {
                if (item.Value == t)
                {
                    res = item.Key;
                    break;
                }
            }
            return res;
        }
    }

}