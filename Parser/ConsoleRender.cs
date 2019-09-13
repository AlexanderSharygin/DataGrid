using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Extensions;

namespace Parser
{
    class Cell
    {
        private bool _IsFocused = false;
        private bool _IsChecked = false;
        public string Body { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public bool IsNeedRefresh { get; set; }
        public int Width { get; set; }
        public bool IsChecked
        {
            get => _IsChecked;
        }
        public bool IsFocused
        {
            get => _IsFocused;
            set {_IsFocused = value; ChangeFocus(); }           
        } 
        public void PrintToTable()
        {
            if (IsNeedRefresh == true)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(XPosition, YPosition);
                Console.Write(Body);
            }
        }
        public void CheckingChange()
        {
            _IsChecked = (_IsChecked) ? false : true;
            Console.SetCursorPosition(XPosition, YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("".PadLeft(Width));
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write((_IsChecked) ? Properties.Resources.CheckedMenuItemPrefix + Body : Properties.Resources.UncheckedMenuItemPrefix + Body);           
            Console.BackgroundColor = ConsoleColor.Black;
        }       
        private void ChangeFocus()
        {
            Console.SetCursorPosition(XPosition, YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("".PadLeft(Width));
            Console.BackgroundColor = (_IsFocused) ? ConsoleColor.Blue: ConsoleColor.Black;           
            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write((_IsChecked) ? Properties.Resources.CheckedMenuItemPrefix + Body : Properties.Resources.UncheckedMenuItemPrefix + Body);               
        }
        public void PrintToMenu()
        {
            Console.SetCursorPosition(XPosition, YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("".PadLeft(Width));
            Console.SetCursorPosition(XPosition, YPosition);
            Console.BackgroundColor = (_IsFocused) ? ConsoleColor.Blue : ConsoleColor.Black;
            Console.Write((_IsChecked) ? Properties.Resources.CheckedMenuItemPrefix + Body : Properties.Resources.UncheckedMenuItemPrefix + Body);
        }
    }
    class ConsoleRender
    {
        public int _MenuItemsCount;
        public int _TableWidth;
        List<string> _AllObjectsFields;
        List<Cell> _MenuItems;
        int _FocusedMenuIndex;
        List<Dictionary<string, string>> _JSONObjects;
        List<String> _PrevSelectedMenuItems = new List<string>();
        List<string> _SelectedMenuItems;
        bool _IsTableLess;
        public ConsoleRender(List<Dictionary<string, string>> p_parsedObjects)
        {
            _JSONObjects = p_parsedObjects;
            _AllObjectsFields = GetAllObjectsFields();
            _MenuItems = new List<Cell>(_AllObjectsFields.Count);           
            for (var i = 0; i < _AllObjectsFields.Count; i++)
            {
                _MenuItems.Add(new Cell());
                _MenuItems[i].Body = _AllObjectsFields[i];
                _MenuItems[i].XPosition = 0;
                _MenuItems[i].YPosition = i + Convert.ToInt32(Properties.Resources.PreambleStringsCount);                        
               _PrevSelectedMenuItems.Add(String.Empty);
            }         
        }    
        public void RefreshMenu()
        {
            _MenuItemsCount = 0;
            foreach (var MenuItem in _MenuItems)
            {
                MenuItem.PrintToMenu();
                _MenuItemsCount++;
            }
        } 
        public void RenderUI()
        {
            Console.CursorVisible = false;
            Console.BufferHeight = Console.WindowHeight + _JSONObjects.Count;
            Console.WriteLine(Properties.Resources.PreambleStrings);
            RefreshMenu();
            _MenuItems[0].IsFocused = true;
            while (true)
            {
                var isConsoleCleared = false;
                ConsoleKey pressedKeyWord = Console.ReadKey(true).Key;
                if (pressedKeyWord == ConsoleKey.DownArrow)
                {
                    if (_FocusedMenuIndex < _AllObjectsFields.Count - 1)
                    {
                        _MenuItems[_FocusedMenuIndex].IsFocused = false;
                        _MenuItems[_FocusedMenuIndex + 1].IsFocused = true;
                        _FocusedMenuIndex++;                     
                    }                   
                }
                if (pressedKeyWord == ConsoleKey.UpArrow)
                {
                    if (_FocusedMenuIndex > 0)
                    {
                        _MenuItems[_FocusedMenuIndex].IsFocused = false;
                        _MenuItems[_FocusedMenuIndex - 1].IsFocused = true;
                        _FocusedMenuIndex--;                    
                    }                    
                }
                if (pressedKeyWord == ConsoleKey.Spacebar)
                {                   
                    _MenuItems[_FocusedMenuIndex].CheckingChange();                  
                    Cell[,] tableCells = GenerateTable();                  
                    foreach (var tableCell in tableCells)
                    {
                        if (tableCell.IsNeedRefresh && !isConsoleCleared)
                        {
                            ClearToEndConsole(tableCell.XPosition, tableCell.YPosition);
                            isConsoleCleared = true;
                        }
                        tableCell.PrintToTable();
                    }                  
                    if (_IsTableLess)
                    {
                        ClearToEndConsole(_TableWidth, _MenuItemsCount + Convert.ToInt32(Properties.Resources.PreambleStringsCount) + 1);
                    }
                    if (_TableWidth > Console.WindowWidth)
                    {
                        Console.BufferWidth = _TableWidth + 2;
                    }              
                }                         
            }
        }
        private void ClearToEndConsole(int xPosition, int yPosition)
        {            
            var startRowIndex = _MenuItemsCount + Convert.ToInt32(Properties.Resources.PreambleStringsCount) + 1;
            var endRowIndex = startRowIndex + _JSONObjects.Count + 1;
            Console.SetCursorPosition(xPosition, yPosition);
            for (int i = startRowIndex; i <= endRowIndex; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("".PadLeft(Console.BufferWidth - xPosition));
                Console.SetCursorPosition(xPosition, i + 1);
            }
            Console.BackgroundColor = ConsoleColor.Blue;
        }     
        private List<string> GetSelectedMenuItems()
        {
          //  var t = _MenuItems.Select(k => new { Item = k, k.Body }).Where(k => k.Item.IsChecked == true).Select(k=>k.Body).ToList<string>();
            var checkedMenuItems = (from item in _MenuItems where item.IsChecked==true select item.Body).ToList<string>();
            return checkedMenuItems;
        }     
        private Cell[,] GenerateTable()
        {
          
            var currentColumnWidth = 0;
            _TableWidth = 0;
            _SelectedMenuItems = GetSelectedMenuItems();
            Cell[,] tableCells = new Cell[_SelectedMenuItems.Count, _JSONObjects.Count + 2];
           
            for (int i = 0; i < _SelectedMenuItems.Count; i++)
            {
                tableCells[i, 0] = new Cell();
                tableCells[i,0].IsNeedRefresh = (_PrevSelectedMenuItems[i] == _SelectedMenuItems[i]) ? false: true; 
          
            // to refactoring. looks rather complex.
            
                tableCells[i, 0].Body = _SelectedMenuItems[i];
                tableCells[i, 0].YPosition = _MenuItemsCount + Convert.ToInt32(Properties.Resources.TableMargin) + Convert.ToInt32(Properties.Resources.PreambleStringsCount);
                tableCells[i, 0].XPosition = _TableWidth;
                tableCells[i, 1] = new Cell();
                tableCells[i, 1].YPosition = tableCells[i, 0].YPosition + 1;
                tableCells[i, 1].XPosition = _TableWidth;
                tableCells[i, 1].IsNeedRefresh = tableCells[i, 0].IsNeedRefresh;
                tableCells[i, 1].Body = String.Empty;
                var list = (from item in _JSONObjects select item[_SelectedMenuItems[i]].Length).Max();
                var max = (list > _SelectedMenuItems[i].Length) ? list : _SelectedMenuItems[i].Length;
                for (int j = 2; j < _JSONObjects.Count + 2; j++)
                { 
                     var a = _JSONObjects[j - 2][_SelectedMenuItems[i]];
                    tableCells[i, j] = new Cell();
                    tableCells[i, j].YPosition = tableCells[i, j - 1].YPosition + 1;
                    tableCells[i, j].XPosition = _TableWidth;
                    if (!(_JSONObjects[j - 2].ContainsKey(_SelectedMenuItems[i])))
                    {
                        tableCells[i, j].Body = Properties.Resources.UndefinedFieldText;
                    }
                    else
                    {
                        tableCells[i, j].Body = _JSONObjects[j - 2][_SelectedMenuItems[i]];
                        if (tableCells[i, j].Body.Length > tableCells[i, 0].Body.Length * Convert.ToInt32(Properties.Resources.ReductionRatio))
                        {
                            tableCells[i, j].Body = tableCells[i, j].Body.Substring(0, tableCells[i, 0].Body.Length * Convert.ToInt32(Properties.Resources.ReductionRatio)) + Properties.Resources.Ellipsis;
                        }
                    }
                    tableCells[i, j].IsNeedRefresh = tableCells[i, 0].IsNeedRefresh;
                }
                currentColumnWidth = GetColumnMaxWidth(tableCells, i, _JSONObjects.Count);
                for (int j = 0; j < _SelectedMenuItems.Count + 1; j++)
                { 
                    tableCells[i, j].Width = currentColumnWidth;
                }
                tableCells[i, 1].Body = "".PadLeft(tableCells[i, 1].Width + Convert.ToInt32(Properties.Resources.ColumnInterval), '-');
                _TableWidth += currentColumnWidth + Convert.ToInt32(Properties.Resources.ColumnInterval);
            }
            int previousMenuItemsCount = _PrevSelectedMenuItems.IndexOf("");
            _IsTableLess = (previousMenuItemsCount < _SelectedMenuItems.Count) ? false : true;
            _PrevSelectedMenuItems = new List<string>(_SelectedMenuItems);
            _PrevSelectedMenuItems.Add(String.Empty);        
            if  (_TableWidth > Console.BufferWidth)
            {
                 Console.BufferWidth = _TableWidth + 1;
            }
            return tableCells;
        }
        public List<string> GetAllObjectsFields()
        {
            List<string> AllObjectsFields = new List<string>();
            for (int i = 0; i < _JSONObjects.Count; i++)
            {
                foreach (var item in _JSONObjects[i].Keys)
                {
                    int index = AllObjectsFields.IndexOf(item);
                    if (index == -1)
                    {
                        AllObjectsFields.Add(item);
                    }
                }
            }
            return AllObjectsFields;
        }
        static int GetColumnMaxWidth(Cell[,] items, int FirstIndex, int SecondIndex)
        {
            int a = Int32.MinValue;
            for (int i = 0; i < SecondIndex; i++)
            {
                if (items[FirstIndex, i].Body.Length > a)
                {
                    a = items[FirstIndex, i].Body.Length;
                }
            }
            return a;
        }            
    }
}
