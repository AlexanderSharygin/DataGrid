using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Parser
{
  class APICore
    {


      public ObservableCollection<Column> Columns { get;  set; } = new ObservableCollection<Column>();
       

        public APICore()
        {
            Columns.CollectionChanged += ColumnsChannge;
           
        }
        public void ChangeSortDirection(Sort direction)
        {
            
            Column item = (Column)Columns.Select(k => k.IsSortedBy = true);           
            item.SortDirecion = direction;           
        }

        private void ColumnsChannge(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action==NotifyCollectionChangedAction.Add)
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
                if (columnsCount>1)
                {
                   int a = Columns.Max(k => k.Index);
                    Columns[newIndex].Index = a + 1;
                }
                for (int i = 0; i < Columns.Count; i++)
                {

                    for (int j = i + 1; j < Columns.Count; j++)
                    {
                        if (Columns[i].HeaderText == Columns[j].HeaderText)
                        {
                            Columns[j].HeaderText = Columns[j].HeaderText + "_Copy";

                        }
                    }
                }

                int MaxColumnsItemsCount = Columns.Max(k => k.Items.Count);
                foreach (var item in Columns)
                {

                    if (item.Items.Count< MaxColumnsItemsCount)
                    {
                        int a = MaxColumnsItemsCount - item.Items.Count;
                        for (int i = 0; i<a; i++)
                        {
                            item.Items.Add("");
                        }
                    }
                }
            }
           
        }

        public void ChangeSortedColumn(int columnIndex)
        {
            if (columnIndex < Columns.Count)
            {
                Column CurrentlySorted = (Column)Columns.Select(k => k.IsSortedBy = true);
                CurrentlySorted.IsSortedBy = false;
                Columns[columnIndex].IsSortedBy = true;
            }
        }
        public void SortColumns()
        {
           Columns = new ObservableCollection<Column>(Columns.OrderBy(k => k.Index));
        }
        public void ChangeColumnsPlaces(int firstColumnIndex, int secondColumnIndex)
        {
            Columns[firstColumnIndex].Index = secondColumnIndex;
            Columns[secondColumnIndex].Index = firstColumnIndex;
            SortColumns();
        }
        public void AddColumn(List<string> source, int columnIndex)
        {
            string columnHeader = source.First();
            int columnWidth = source.Max(k => k.Length);
            Columns.Insert(columnIndex, new Column(columnHeader, columnIndex, columnWidth));
            for (int i = 0; i < Columns.Count; i++)
            {
                Columns[i].Index = i;
            }
        }
        public void DeleteColumn(int columnIndex)
        {              
               Columns.RemoveAt(columnIndex);
                for (int i = 0; i < Columns.Count; i++)
                {              
                    Columns[i].Index=i;
                }          
        }
        public void UpdateColumns(ObservableCollection<List<string>> source)
        {
            if (Columns.Count == 0)
            {
                for (int i = 0; i < source.First().Count; i++)
                {
                    var columnStrings = source.Select(k => k[i]).ToList<string>();
                    AddColumn(columnStrings, i);
                  
                }
            }
            else
            {
                for (int i = 0; i < source.First().Count; i++)
                {
                    List<string> ExisitinColumns = Columns.Select(k => k.HeaderText).ToList<string>();
                    if (ExisitinColumns.IndexOf(source.First()[i])==-1)
                    {
                        var columnStrings = source.Select(k => k[i]).ToList<string>();
                        AddColumn(columnStrings, i);
                    
                    }
                 }
                for (int i = 0; i < Columns.Count; i++)
                {
                    bool toDelete = true;
                    for (int j = 0; j < source.First().Count; j++)
                    {
                        if (Columns[i].HeaderText == source.First()[j])
                        {
                            toDelete = false;                           
                            break;
                        }
                    }
                    if (toDelete)
                    {
                        DeleteColumn(i);
                    }
                }
            }
        }

    }
    internal class Column
    {

        public string _HeaderText;
        public Column(string headerText, int index, int width)
        {
            HeaderText = headerText;
            Index = index;
            Width = width;
            AllTypes = new Types();
            Type  = typeof(String);

        }
        public Column(string headerText, int index, Type type, List<string> items)
        {
            _HeaderText = headerText;
            Index = index;
            Type = type;
           
            AllTypes = new Types();
            Items = items;
            Visible = true;
            Width = (items.Max(k=>k.Length)>headerText.Length)? items.Max(k => k.Length) : headerText.Length ;
        }
        public string HeaderText
        {
            get => _HeaderText;
            set
            {
                Width = value.Length;
                _HeaderText = value;
            }

        }
        public int Index { get; set; }
        public int Width { get; set; }
        public Sort SortDirecion { get; set; } = Sort.None;
        public bool IsSortedBy { get; set; } = false;
        public Type Type { get; set; }
        public bool Visible { get; set; }
        public Types AllTypes { get; set; }
        public List<string> Items { get; set; } = new List<string>();
       

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
