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
        bool _IsFocused = false;
        public bool _IsChecked = false;     
        public string Body { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public bool IsChecked
        {
            get
            { return _IsChecked; }
        }
        public bool IsFocused
        {
            get
            { return _IsFocused; }
            set
            {
                _IsFocused = value;
                ChangeFocus();
            }
        }

        public bool IsNeedRefresh { get; set; }
      
        public void CheckingChange()
        {
            if (_IsChecked)
            {
                _IsChecked=false;
            }
            else
            {
                _IsChecked = true;
            }
            Console.SetCursorPosition(XPosition, YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("".PadLeft(Width));
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(XPosition, YPosition);
            if (_IsChecked)
            {
                Console.Write(Properties.Resources.CheckedMenuItemPrefix + Body);
            }
            else
            {
                Console.Write(Properties.Resources.UncheckedMenuItemPrefix +Body);
            }
        }
        public int Width { get; set; }
       
        public void ChangeFocus()
        {
            Console.SetCursorPosition(XPosition, YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("".PadLeft(Width));
            if (_IsFocused)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
            }
            Console.SetCursorPosition(XPosition, YPosition);
            if (_IsChecked)
            {
                Console.Write(Properties.Resources.CheckedMenuItemPrefix + Body);
            }
            else
            {
                Console.Write(Properties.Resources.UncheckedMenuItemPrefix + Body);
            }
                    
        }
        public void PrintMenuItem()
        {
            Console.SetCursorPosition(XPosition, YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("".PadLeft(Width));
            Console.SetCursorPosition(XPosition, YPosition);
            if (IsFocused)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
            if (IsChecked)
            {
                Console.Write(Properties.Resources.CheckedMenuItemPrefix + Body);
               
            }
            else
            {
                Console.Write(Properties.Resources.UncheckedMenuItemPrefix + Body);
               
            }
        }

    }
    class ConsoleRender
    {
        public int _MenuItemsCount;
        public int _TableWidth;
       List<string> _AllObjectsFields;
        List<Cell> _MenuItems;
       int _MenuWidth;
        int _FocusedMenuInbdex = 0;
        List<Dictionary<string, string>> _JSONObjects;
        List<String> _PrevSelectedMenuItems = new List<string>();
        List<string> _SelectedMenuItems;
        bool isTableLess;
        public ConsoleRender(List<Dictionary<string, string>> p_parsedObjects)
        {
            _JSONObjects = p_parsedObjects;
            _AllObjectsFields = GetAllObjectsFields();
            _MenuItems = new List<Cell>(_AllObjectsFields.Count);
            for (int i = 0; i < _AllObjectsFields.Count; i++)
            {
                _MenuItems.Add(new Cell());
                _MenuItems[i].Body = _AllObjectsFields[i];
                _MenuItems[i].XPosition = 0;
                _MenuItems[i].YPosition = i + Convert.ToInt32(Properties.Resources.PreambleStringsCount);
              
               
               _PrevSelectedMenuItems.Add("");
            }
         
            _MenuWidth = _MenuItems.Max(item => item.Body.Length);
        }
       

        public void RefreshMenu()
        {
            _MenuItemsCount = 0;
            for (int i = 0; i < _MenuItems.Count; i++)
            {
                _MenuItems[i].PrintMenuItem();
                _MenuItemsCount++;              
            }
        } 
        public void RenderUI()
        {
            Console.CursorVisible = false;
            Console.BufferWidth = Console.BufferWidth;
            Console.WriteLine(Properties.Resources.PreambleStrings);
            RefreshMenu();

            _MenuItems[0].IsFocused = true;
            while (true)
            {
                bool isConsoleCleared = false;
                ConsoleKey pressedKeyWord = Console.ReadKey(true).Key;
                if (pressedKeyWord == ConsoleKey.DownArrow)
                {
                    if (_FocusedMenuInbdex < _AllObjectsFields.Count - 1)
                    {
                        _MenuItems[_FocusedMenuInbdex].IsFocused = false;
                        _MenuItems[_FocusedMenuInbdex + 1].IsFocused = true;
                        _FocusedMenuInbdex++;                     
                    }
                    continue;
                }
                if (pressedKeyWord == ConsoleKey.UpArrow)
                {
                    if (_FocusedMenuInbdex > 0)
                    {
                        _MenuItems[_FocusedMenuInbdex].IsFocused = false;
                        _MenuItems[_FocusedMenuInbdex - 1].IsFocused = true;
                        _FocusedMenuInbdex--;                    
                    }
                    continue;
                }
                if (pressedKeyWord == ConsoleKey.Spacebar)
                {
                    _MenuItems[_FocusedMenuInbdex].CheckingChange();                  
                    Cell[,] tableCells = GenerateTable();
                    for (int i = 0; i < _SelectedMenuItems.Count; i++)
                    {
                        if (tableCells[i, 0].IsNeedRefresh == true)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            for (int j = 0; j < _JSONObjects.Count + 2; j++)
                            {
                                if (!isConsoleCleared)
                                {
                                    ClearConsoleRow(tableCells[i, j].XPosition, tableCells[i, j].YPosition);
                                }
                                Console.SetCursorPosition(tableCells[i, j].XPosition, tableCells[i, j].YPosition);
                                Console.Write(tableCells[i, j].Body);
                            }
                            isConsoleCleared = true;
                        }                      
                    }
                    if (isTableLess)
                    {
                        ClearToEndConsole(_TableWidth, _MenuItemsCount + Convert.ToInt32(Properties.Resources.PreambleStringsCount) + 1);
                    }
                    if (_TableWidth > Console.WindowWidth)
                    {
                        Console.BufferWidth = _TableWidth + 2;
                    }
                    continue;
                }
                if (pressedKeyWord == ConsoleKey.Tab)
                {
                    RefreshMenu();
                    continue;
                }               
            }
        }
        private void ClearToEndConsole(int xPosition, int yPosition)
        {
            
            int startRowIndex = _MenuItemsCount + Convert.ToInt32(Properties.Resources.PreambleStringsCount) + 1;
            int endRowIndex = startRowIndex + _JSONObjects.Count + 1;
            Console.SetCursorPosition(xPosition, yPosition);
            for (int i = startRowIndex; i <= endRowIndex; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("".PadLeft(Console.BufferWidth - xPosition));
                Console.SetCursorPosition(xPosition, i + 1);
            }
            Console.BackgroundColor = ConsoleColor.Blue;
        }
        
        private void ClearConsoleRow(int xPosition, int yPosition)
        {
            Console.SetCursorPosition(xPosition, yPosition);
            Console.Write("".PadLeft(Console.BufferWidth - xPosition));
        }
        // to refactoring.
        private List<string> GetSelectedMenuItems()
        {
            List<string> checkedMenuItems = new List<string>();
            for (int i = 0; i < _MenuItems.Count; i++)
            {
                if (_MenuItems[i].IsChecked == true)
                {
                    checkedMenuItems.Add(_MenuItems[i].Body);
                }
            }
            return checkedMenuItems;
        }
        private Cell[,] GenerateTable()
        {
          
            int currentColumnWidth = 0;
            _TableWidth = 0;
            _SelectedMenuItems = GetSelectedMenuItems();
            Cell[,] tableCells = new Cell[_SelectedMenuItems.Count, _JSONObjects.Count + 2];
            for (int i = 0; i < _SelectedMenuItems.Count; i++)
            {
                tableCells[i, 0] = new Cell();
                if (_PrevSelectedMenuItems[i] == _SelectedMenuItems[i])
                {
                    tableCells[i, 0].IsNeedRefresh = false;
                }
                else
                {
                    tableCells[i, 0].IsNeedRefresh = true;
                }
                // to refactoring. looks rather complex.
                tableCells[i, 0].Body = _SelectedMenuItems[i];
                tableCells[i, 0].YPosition = _MenuItemsCount + Convert.ToInt32(Properties.Resources.TableMargin) + Convert.ToInt32(Properties.Resources.PreambleStringsCount);
                tableCells[i, 0].XPosition = _TableWidth;
                tableCells[i, 1] = new Cell();
                tableCells[i, 1].YPosition = tableCells[i, 0].YPosition + 1;
                tableCells[i, 1].XPosition = _TableWidth;
                tableCells[i, 1].IsNeedRefresh = tableCells[i, 0].IsNeedRefresh;
                tableCells[i, 1].Body = "";
                for (int j = 2; j < _JSONObjects.Count + 2; j++)
                {
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
            if (previousMenuItemsCount < _SelectedMenuItems.Count)
            {
                isTableLess = false;
            }
            else
            {
                isTableLess = true;
            }
            for (int i = 0; i < _PrevSelectedMenuItems.Count; i++)
            {
                _PrevSelectedMenuItems[i] = "";
            }
            for (int i = 0; i < _SelectedMenuItems.Count; i++)
            {
                _PrevSelectedMenuItems[i] = _SelectedMenuItems[i];
            }
            if (_TableWidth > Console.BufferWidth)
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
