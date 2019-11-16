using System.Collections.Generic;
using System.Linq;


namespace Parser
{
  class APICore
    {
        public List<Column> Columns { get; private set; } = new List<Column>();
        public void ChangeSortDirection(Sort direction)
        {
            Column item = (Column)Columns.Select(k => k.IsSortedBy = true);           
            item.SortDirecion = direction;           
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
            Columns = Columns.OrderBy(k => k.Index).ToList();
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
        public void UpdateColumns(List<List<string>> source)
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
