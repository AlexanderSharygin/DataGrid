using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
  class APIModel
    {
        List<Column> _Columns = new List<Column>();
        public List<Column> Columns { get =>_Columns; }
        public void ChangeSortDirection(Sort direction)
        {
            Column item = (Column)_Columns.Select(k => k.IsSortedBy = true);
           
                item.SortDirecion = direction;
           
        }
        public void ChangeSortedColumn(int columnIndex)
        {
            if (columnIndex < _Columns.Count)
            {
                Column CurrentlySorted = (Column)_Columns.Select(k => k.IsSortedBy = true);
                CurrentlySorted.IsSortedBy = false;
                _Columns[columnIndex].IsSortedBy = true;
            }
        }
        public void ChangeIndexes(int firstColumnIndex, int secondColumnIndex)
        {
            _Columns[firstColumnIndex].Index = secondColumnIndex;
            _Columns[secondColumnIndex].Index = firstColumnIndex;
        }
        public void AddColumn(List<string> source, int columnIndex)
        {
            string columnHeader = source.First();
            int columnWidth = source.Max(k => k.Length);
            _Columns.Insert(columnIndex, new Column(columnHeader, columnIndex, columnWidth));
            for (int i = 0; i < _Columns.Count; i++)
            {
                _Columns[i].Index = i;
            }
        }
        public void RemoveColumn(int columnIndex)
        {
           
            
                _Columns.RemoveAt(columnIndex);
                for (int i = 0; i < _Columns.Count; i++)
                {              
                    _Columns[i].Index=i;
                }
               
            
            
        }
        public void Update(List<List<string>> source)
        {
            if (_Columns.Count == 0)
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
                    List<string> a = _Columns.Select(k => k.HeaderText).ToList<string>();
                    if (a.IndexOf(source.First()[i])==-1)
                    {
                        var columnStrings = source.Select(k => k[i]).ToList<string>();
                        AddColumn(columnStrings, i);
                    }
                 }
                for (int i = 0; i < _Columns.Count; i++)
                {
                    bool toDelete = true;
                    for (int j = 0; j < source.First().Count; j++)
                    {
                        if (_Columns[i].HeaderText == source.First()[j])
                        {
                            toDelete = false;
                            break;
                        }
                    }
                    if (toDelete)
                    {
                        RemoveColumn(i);
                    }
                }
            }
        }

    }
    public class Column
    {
       
        public Column(string headerText, int index, int width)
        {
            HeaderText = headerText;
            Index = index;
            Width = width;
        }
        public string HeaderText { get; set; }
        public int Index { get; set; }
        public int Width { get; set; }
        public Sort SortDirecion { get; set; } = Sort.None;
        public bool IsSortedBy { get; set; } = false;

    }
    public enum Sort
    {
        ASC,
        DESC,
        None
    }

}
