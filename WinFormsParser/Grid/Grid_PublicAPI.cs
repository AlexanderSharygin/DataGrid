using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    partial class MyDataGrid
    {
       
        public void ChangeSorting(string columnName, SortDirections sortDirection)
        {
            RemoveEditorFromControls(false);
            var newSortedColumns = Columns.Select(k => k).Where(u => u.HeaderText == columnName).ToList();
            foreach (var item in newSortedColumns)
            {
                if (item.Visible)
                {
                    _API.SortedColumnIndex = item.Index;
                    _API.SortDirection = sortDirection;
                    break;
                }
            }
        }
        public bool RemoveColumn(string columnName)
        {
            bool res = true;
            for (int i = 0; i < _API.Columns.Count; i++)
            {
                if (_API.Columns[i].HeaderText == columnName)
                {
                    if (_API.Columns[i].HeaderText != PrivateKeyColumn)
                    {
                        _API.Columns.RemoveAt(i);
                        break;

                    }
                    else
                    {
                        res = false;
                    }
                }
            }
            return res;
        }
        public bool RemoveColumn(int columnIndex)
        {
            bool res = true;
            if (_API.Columns[columnIndex].HeaderText != PrivateKeyColumn)
            {
                _API.Columns.RemoveAt(columnIndex);
            }
            else
            {
                res = false;
            }
            return res;
        }
     
    }
}
