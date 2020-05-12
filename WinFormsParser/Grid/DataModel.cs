using Parser.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Parser
{
    class Source
    {
      
        public SortDirections _SortDirection = SortDirections.None;
      
     
        public ColumnTypesList DataTypes { get; set; } = new ColumnTypesList();     
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
      
        public event PropertyChangedEventHandler Event_PropertyChanged;
      
        public Source()
        {
            Columns.CollectionChanged += ColumnsChannge;
        }
       
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            Event_PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private void ColumnsChannge(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                int index = 0;
                foreach (var column in Columns)
                {
                    column.Index = index;
                    index++;

                }
                SortDirection = SortDirections.None;
                SortedColumnIndex = -1;

            }
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                int newIndex = e.NewStartingIndex;
                int columnsCount = 0;
                foreach (var column in Columns)
                {
                    if (column.Index == Columns[newIndex].Index)
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
        string _Body;
        public string Body { get=>_Body; set { _Body = value; BodyToPrint = _Body; } }
        public string BodyToPrint { get; set; }
        public int SourceColumnIndex { get; set; }
        public int BuferRowIndex { get; set; }     
    }   
  
    internal class Column
    {
      
        string _HeaderText;
        bool _Visible;
        Type _DataType;
      
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
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
     
        public Column(string headerText, Type type)
        {
            _HeaderText = headerText;
            Index = 0;
            Width = 1;
            DataType = type;
            SetDefaultDataFormat();
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
    class Row
    {
        public List<Cell> Cells { get; set; } = new List<Cell>();
    }
    class Page
    {
        public int SkipElementsCount { get; set; }
        public int TakeElementsCount { get; set; }
        public int Number { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int DownScrollValue { get; set; }
        public int UpScrollValue { get; set; }
    }
    public enum SortDirections
    {
        ASC,
        DESC,
        None
    }

    class ColumnTypesList
    {
        internal Dictionary<string, Type> TypesCollection { get; } = new Dictionary<string, Type>();
        public ColumnTypesList()
        {
            TypesCollection.Add("String", typeof(String));
            TypesCollection.Add("Integer", typeof(Int32));
            TypesCollection.Add("Date/Time", typeof(DateTime));
            TypesCollection.Add("Boolean", typeof(Boolean));
        }
        public string GetKeyByValue(Type t)
        {
            string key = "";
            foreach (var item in TypesCollection)
            {
                if (item.Value == t)
                {
                    key = item.Key;
                    break;
                }
            }
            return key;
        }
    }
    
  


}
