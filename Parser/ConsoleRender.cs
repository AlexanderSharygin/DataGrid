using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    class Cell
    {
        bool _IsFocused;
        bool _IsChecked;
        public string Body { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public bool IsNeedRefresh { get; set; }
        public int Width { get; set; }
        public bool IsChecked
        {
            get => _IsChecked;
            set
            {
                _IsChecked = value;
                Console.SetCursorPosition(XPosition, YPosition);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("".PadLeft(Width));
                Console.BackgroundColor = ConsoleConfig.SelectedFieldColor;
                Console.SetCursorPosition(XPosition, YPosition);
                Console.Write((IsChecked) ? Properties.Resources.CheckedMenuItemPrefix + Body : Properties.Resources.UncheckedMenuItemPrefix + Body);
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
        public bool IsFocused
        {
            get => _IsFocused;
            set
            {
                _IsFocused = value;
                Console.SetCursorPosition(XPosition, YPosition);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("".PadLeft(Width));
                Console.BackgroundColor = (_IsFocused) ? ConsoleConfig.SelectedFieldColor : ConsoleColor.Black;
                Console.SetCursorPosition(XPosition, YPosition);
                Console.Write((IsChecked) ? Properties.Resources.CheckedMenuItemPrefix + Body : Properties.Resources.UncheckedMenuItemPrefix + Body);
            }
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
        public void PrintToMenu()
        {
            Console.SetCursorPosition(XPosition, YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("".PadLeft(Width));
            Console.SetCursorPosition(XPosition, YPosition);
            Console.BackgroundColor = (_IsFocused) ? ConsoleConfig.SelectedFieldColor : ConsoleColor.Black;
            Console.Write((IsChecked) ? Properties.Resources.CheckedMenuItemPrefix + Body : Properties.Resources.UncheckedMenuItemPrefix + Body);
        }
    }
    class ConsoleRender
    {
        int _TableCurrentWidth;
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
                    _MenuItems[_FocusedMenuIndex].IsChecked = !(_MenuItems[_FocusedMenuIndex].IsChecked);
                    Column[] Table = GenerateTable();
                    foreach (var Column in Table)
                    {
                        foreach (var Cell in Column.Cells)
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
        }
        class Column
        {
            public List<Cell> Cells { get; set; } = new List<Cell>();
        }

        private Column[] GenerateTable()
        {
            _TableCurrentWidth = 0;
            _SelectedMenuItems = (from item in _MenuItems where item.IsChecked select item.Body).ToList<string>();

           Column[] Table = new Column[_SelectedMenuItems.Count];
            for (int i = 0; i < Table.Length; i++)
            {
                Table[i] = new Column();
            }
            int index = 0;
            foreach (var column in Table)
            {
                int YPositionCounter = _MenuItems.Count + Convert.ToInt32(Properties.Resources.TableMargin) + Convert.ToInt32(Properties.Resources.PreambleStringsCount);
                column.Cells.Add(new Cell
                {
                    IsNeedRefresh = (_PrevMenuItems[index] == _SelectedMenuItems[index]) ? false : true,
                    Body = _SelectedMenuItems[index],
                    YPosition = YPositionCounter,
                    XPosition = _TableCurrentWidth,
                });
                YPositionCounter++;
                foreach (var JSONObject in _JSONObjects)
                {
                    column.Cells.Add(new Cell
                    {
                        YPosition = ++YPositionCounter,
                        XPosition = _TableCurrentWidth,
                        Body = (JSONObject.ContainsKey(_SelectedMenuItems[index])) ? JSONObject[_SelectedMenuItems[index]] : Properties.Resources.UndefinedFieldText,
                        IsNeedRefresh = column.Cells[0].IsNeedRefresh,
                    });
                }
                var cellsWithLongBody = column.Cells.Select(k => k).Where(k => k.Body.Length > _SelectedMenuItems[index].Length * Convert.ToInt32(Properties.Resources.ReductionRatio)).ToList();
                cellsWithLongBody.ForEach(k => k.Body = k.Body.Substring(0, _SelectedMenuItems[index].Length * Convert.ToInt32(Properties.Resources.ReductionRatio)) + Properties.Resources.Ellipsis);
                var MaxCellWidth = column.Cells.Max(k => k.Body.Length);
                column.Cells.Insert(1, new Cell
                {
                    YPosition = column.Cells[0].YPosition + 1,
                    XPosition = _TableCurrentWidth,
                    IsNeedRefresh = column.Cells[0].IsNeedRefresh,
                    Body = "".PadLeft(MaxCellWidth + Convert.ToInt32(Properties.Resources.ColumnInterval), '-'),
                });
                column.Cells.ForEach(k => k.Width = MaxCellWidth);
                _TableCurrentWidth += MaxCellWidth + Convert.ToInt32(Properties.Resources.ColumnInterval);
                index++;
            }
            int prevMenuItemsCount = _PrevMenuItems.Count(k => k.Length > 0);
            _PrevMenuItems = new List<string>(_SelectedMenuItems);
            _IsTableLess = (_PrevMenuItems.Count < _SelectedMenuItems.Count) ? false : true;
            _PrevMenuItems.Add(String.Empty);
            if (_TableCurrentWidth > Console.WindowWidth)
            { Console.BufferWidth = _TableCurrentWidth + 2; }
            return Table;
        }

    }
}
