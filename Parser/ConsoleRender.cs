using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    class Cell
    {
        private bool _IsFocused = false;
        public string Body { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public bool IsNeedRefresh { get; set; }
        public int Width { get; set; }
        public bool IsChecked { get; private set; } = false;
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
            IsChecked = (IsChecked) ? false : true;
            Console.SetCursorPosition(XPosition, YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("".PadLeft(Width));
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write((IsChecked) ? Properties.Resources.CheckedMenuItemPrefix + Body : Properties.Resources.UncheckedMenuItemPrefix + Body);           
            Console.BackgroundColor = ConsoleColor.Black;
        }       
        private void ChangeFocus()
        {
            Console.SetCursorPosition(XPosition, YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("".PadLeft(Width));
            Console.BackgroundColor = (_IsFocused) ? ConsoleColor.Blue: ConsoleColor.Black;           
            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write((IsChecked) ? Properties.Resources.CheckedMenuItemPrefix + Body : Properties.Resources.UncheckedMenuItemPrefix + Body);               
        }
        public void PrintToMenu()
        {
            Console.SetCursorPosition(XPosition, YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("".PadLeft(Width));
            Console.SetCursorPosition(XPosition, YPosition);
            Console.BackgroundColor = (_IsFocused) ? ConsoleColor.Blue : ConsoleColor.Black;
            Console.Write((IsChecked) ? Properties.Resources.CheckedMenuItemPrefix + Body : Properties.Resources.UncheckedMenuItemPrefix + Body);
        }
    }
    class ConsoleRender
    {      
        public int _TableCurrentWidth;
        List<string> _AggregatedObjectsFields;
        List<Cell> _MenuItems;
        int _FocusedMenuIndex;
        List<Dictionary<string, string>> _JSONObjects;
        List<string> _PrevMenuItems = new List<string>();
        bool _IsTableLess;
        List<string> _SelectedMenuItems;      
        public ConsoleRender(List<Dictionary<string, string>> p_parsedObjects)
        {
            _JSONObjects = p_parsedObjects;
            _AggregatedObjectsFields = _JSONObjects.SelectMany(j => j.Keys).Distinct().ToList();           
              _MenuItems = new List<Cell>(_AggregatedObjectsFields.Count);          
            for (int i = 0; i < _AggregatedObjectsFields.Count; i++)
            {
                _MenuItems.Add(new Cell());
                _MenuItems[i].Body = _AggregatedObjectsFields[i];
                _MenuItems[i].XPosition = 0;
                _MenuItems[i].YPosition = i + Convert.ToInt32(Properties.Resources.PreambleStringsCount);                        
               _PrevMenuItems.Add(String.Empty);
            }         
        } 
        public void RenderUI()
        {
            Console.CursorVisible = false;
            Console.BufferHeight = Console.WindowHeight + _JSONObjects.Count;
            Console.WriteLine(Properties.Resources.PreambleStrings);
            _MenuItems.ForEach(k => k.PrintToMenu());          
            _MenuItems[0].IsFocused = true;
            while (true)
            {
                bool isConsoleCleared = false;
                ConsoleKey pressedKeyWord = Console.ReadKey(true).Key;
                if (pressedKeyWord == ConsoleKey.DownArrow && _FocusedMenuIndex < _AggregatedObjectsFields.Count - 1)
                {
                    _MenuItems[_FocusedMenuIndex].IsFocused = false;
                    _MenuItems[_FocusedMenuIndex + 1].IsFocused = true;
                    _FocusedMenuIndex++;
                }
                if (pressedKeyWord == ConsoleKey.UpArrow && _FocusedMenuIndex > 0)
                {
                    _MenuItems[_FocusedMenuIndex].IsFocused = false;
                    _MenuItems[_FocusedMenuIndex - 1].IsFocused = true;
                    _FocusedMenuIndex--;
                }
                if (pressedKeyWord == ConsoleKey.Spacebar)
                {                   
                    _MenuItems[_FocusedMenuIndex].CheckingChange();
                     List<Cell>[] Cells = GenerateTable();               
                    foreach (var Column in Cells)
                    {
                        foreach (var Cell in Column)
                        {
                            if (Cell.IsNeedRefresh && !isConsoleCleared)
                            {
                                ClearToEndConsole(Cell.XPosition, Cell.YPosition);
                                isConsoleCleared = true;
                            }
                            Cell.PrintToTable();
                        }                        
                    }
                    if (_IsTableLess)
                    {
                        ClearToEndConsole(_TableCurrentWidth, _MenuItems.Count + Convert.ToInt32(Properties.Resources.PreambleStringsCount) + 1);
                    }                    
                }                         
            }
        }
      
        private void ClearToEndConsole(int xPosition, int yPosition)
        {            
            var startRowIndex = _MenuItems.Count + Convert.ToInt32(Properties.Resources.PreambleStringsCount) + 1;
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
        private List<Cell>[] GenerateTable()
        {
            _TableCurrentWidth = 0;
            _SelectedMenuItems = (from item in _MenuItems where item.IsChecked == true select item.Body).ToList<string>();         
            List<Cell>[] Cells = new List<Cell>[_SelectedMenuItems.Count];
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = new List<Cell>();
            }
           int index = 0;
            foreach (var column in Cells)
            {
               int YPositionCounter = _MenuItems.Count + Convert.ToInt32(Properties.Resources.TableMargin) + Convert.ToInt32(Properties.Resources.PreambleStringsCount);
                column.Add(new Cell
                {
                    IsNeedRefresh = (_PrevMenuItems[index] == _SelectedMenuItems[index]) ? false : true,
                    Body = _SelectedMenuItems[index],
                    YPosition = YPositionCounter,
                    XPosition = _TableCurrentWidth,
                });
                YPositionCounter++;
                foreach (var JSONObject in _JSONObjects)
                {
                    column.Add(new Cell
                    {
                        YPosition = ++YPositionCounter,
                        XPosition = _TableCurrentWidth,
                        Body = (JSONObject.ContainsKey(_SelectedMenuItems[index])) ? JSONObject[_SelectedMenuItems[index]] : Properties.Resources.UndefinedFieldText,
                        IsNeedRefresh = column[0].IsNeedRefresh,
                    });                
                }
                var cellsWithLongBody = column.Select(k => k).Where(k=>k.Body.Length > _SelectedMenuItems[index].Length * Convert.ToInt32(Properties.Resources.ReductionRatio)).ToList();
                cellsWithLongBody.ForEach(k=>k.Body = k.Body.Substring(0, _SelectedMenuItems[index].Length * Convert.ToInt32(Properties.Resources.ReductionRatio)) + Properties.Resources.Ellipsis);
                var MaxCellWidth = column.Max(k => k.Body.Length);
                column.Insert(1, new Cell
                {
                    YPosition = column[0].YPosition+1,
                    XPosition = _TableCurrentWidth,
                    IsNeedRefresh = column[0].IsNeedRefresh,
                    Body = "".PadLeft(MaxCellWidth + Convert.ToInt32(Properties.Resources.ColumnInterval), '-'),
                });
                column.ForEach(k => k.Width = MaxCellWidth);
                _TableCurrentWidth += MaxCellWidth + Convert.ToInt32(Properties.Resources.ColumnInterval);
                 index++;
            }
            int prevMenuItemsCount = _PrevMenuItems.Count(k => k.Length > 0);          
            _PrevMenuItems = new List<string>(_SelectedMenuItems);
            _IsTableLess = (_PrevMenuItems.Count < _SelectedMenuItems.Count) ? false : true;
            _PrevMenuItems.Add(String.Empty);
            if (_TableCurrentWidth > Console.WindowWidth)
            { Console.BufferWidth = _TableCurrentWidth + 2; }
            return Cells;
        }
      
    }
}
