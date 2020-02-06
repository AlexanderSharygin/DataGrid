using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;



namespace Parser
{

    public static class ObservableCollectionExtensions
    {
        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        {
            var sortableList = new List<T>(collection);
            sortableList.Sort(comparison);
            for (int i = 0; i < sortableList.Count; i++)
            {
                collection.Move(collection.IndexOf(sortableList[i]), i);
            }
        }
    }
       
    class APICore : INotifyPropertyChanged
    {
        public Types AllTypes { get; set; } = new Types();
        public Sort _SortDirection = Sort.None;
        public Sort SortDirection
        {
            get => _SortDirection;
            set
            {
                _SortDirection = value;
                OnPropertyChanged();
            }
        }
        public int SortedColumnIndex { get; set; } = -1;
        public bool IsEditorOpened { get; set; } 
        public Type EditorComponentType { get; set; }     
        public bool IsEditorNedded  { get; set; }
        public bool IsTypeSelectorOpened { get; set; } 
        public ObservableCollection<Column> Columns { get; } = new ObservableCollection<Column>();
        public event PropertyChangedEventHandler PropertyChanged;
        public APICore()
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
                SortDirection = Sort.None;
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
        internal void SortColumns()
        {
           Columns.Sort((a, b) => { return a.Index.CompareTo(b.Index); });
        }   
    }

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
            Index =0;
            Width = 1;
           
            DataType =type;
        }     
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
    public enum Sort
    {
        ASC,
        DESC,
        None
    }
    class Types
    {
       internal Dictionary<string, Type> TypesCollection { get; } = new Dictionary<string, Type>();
        public Types()
        {
            TypesCollection.Add("String", typeof(String));
            TypesCollection.Add("Integer", typeof(Int32));
            TypesCollection.Add("Date/Time", typeof(DateTime));
        }
        public string GetKeyyValue(Type t)
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
