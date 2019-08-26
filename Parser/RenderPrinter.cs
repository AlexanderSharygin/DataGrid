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
        public string Body { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public bool IsChecked { get; set; }
        public bool IsFocused { get; set; }
        public bool IsForUpdate { get; set; }
        public int Width { get; set; }
     }
    class RenderPrinter
    {
        public int _MenuItemsCount;
        public int _TableWidth;
        AgregatedKeyList _AllKeys;
        List<Cell> _MenuItems;
        int _MenuWidth;
        int _FocusedFieldCounter = 0;
        MyList<JSONObject> _ParsedObjects;
        List<String> _PreviousSelectedItems = new List<string>();
        List<string> _SelectedItems;
        bool isTableLess;
        public RenderPrinter(MyList<JSONObject> p_parsedObjects)
        {
            _ParsedObjects = p_parsedObjects;
            _AllKeys = new AgregatedKeyList(p_parsedObjects);
            _MenuItems = new List<Cell>(_AllKeys.Count);
            for (int i = 0; i < _AllKeys.Count; i++)
            {
                _MenuItems.Add(new Cell());
                _MenuItems[i].Body = _AllKeys[i];
                _MenuItems[i].XPosition = 0;
                _MenuItems[i].YPosition = i;
                _MenuItems[i].IsChecked = false;
                if (i == 0)
                {
                    _MenuItems[i].IsFocused = true;
                }
                else
                {
                    _MenuItems[i].IsFocused = false;
                }
                _PreviousSelectedItems.Add("");
            }
            _MenuWidth = _MenuItems.Max(item => item.Body.Length);

        }
        public void ReWritehMenu()
        {
            _MenuItemsCount = 0;
            for (int i = 0; i < _MenuItems.Count; i++)
            {
                Console.SetCursorPosition(_MenuItems[i].XPosition, _MenuItems[i].YPosition);
                Console.BackgroundColor = ConsoleColor.Black;
                for (int j = 0; j < _MenuWidth; j++)
                {
                    Console.Write(" ");
                }
                Console.SetCursorPosition(_MenuItems[i].XPosition, _MenuItems[i].YPosition);
                if (_MenuItems[i].IsFocused)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                if (_MenuItems[i].IsChecked)
                {
                    Console.Write(Constants.CheckedFieldOnUIPrefix + _MenuItems[i].Body);
                    _MenuItemsCount++;
                }
                else
                {
                    Console.Write(Constants.UncheckedFieldOnUIPrefix + _MenuItems[i].Body);
                    _MenuItemsCount++;
                }
            }

        }
        public void Print()
        {
            Console.CursorVisible = false;
            ReWritehMenu();
       
            while (true)
            {
                bool isConsoleCleared = false;
                ConsoleKey pressedKeyWord = Console.ReadKey(true).Key;
                if (pressedKeyWord == ConsoleKey.DownArrow)
                {
                    if (_FocusedFieldCounter < _AllKeys.Count - 1)
                    {
                        _MenuItems[_FocusedFieldCounter].IsFocused = false;
                        _MenuItems[_FocusedFieldCounter + 1].IsFocused = true;
                        MoveToNextOrPrevMenuItem(_FocusedFieldCounter, true);
                    }
                    continue;
                }
                if (pressedKeyWord == ConsoleKey.UpArrow)
                {
                    if (_FocusedFieldCounter > 0)
                    {
                        _MenuItems[_FocusedFieldCounter].IsFocused = false;
                        _MenuItems[_FocusedFieldCounter - 1].IsFocused = true;
                        MoveToNextOrPrevMenuItem(_FocusedFieldCounter, false);

                    }
                    continue;
                }
                if (pressedKeyWord == ConsoleKey.Spacebar)
                {
                    if (_MenuItems[_FocusedFieldCounter].IsChecked == true)
                    {
                        _MenuItems[_FocusedFieldCounter].IsChecked = false;
                    }
                    else
                    {
                        _MenuItems[_FocusedFieldCounter].IsChecked = true;
                    }
                    ChecOrUncheckkMenuItem(_FocusedFieldCounter);
                    Cell[,] tableCells = GetTableCells();
                    for (int i = 0; i < _SelectedItems.Count; i++)
                    {
                        if (tableCells[i, 0].IsForUpdate == true)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            for (int j = 0; j < _ParsedObjects.Count; j++)
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
                        else
                        {
                            continue;
                        }
                      

                    }
                    if (isTableLess)
                     {
                        ClearToEndConsole(_TableWidth, _MenuItemsCount);
                    }
                    continue;

                }
                if (pressedKeyWord == ConsoleKey.Tab)
                {
                    ReWritehMenu();
                    continue;
                }
                else
                {


                    continue;
                }
            }
        }
        private void ClearToEndConsole(int xPosition, int yPosition)
        {
            int startRowCounter = _MenuItemsCount;
            int endRowCounter = startRowCounter + _ParsedObjects.Count;
            Console.SetCursorPosition(xPosition, yPosition);
            for (int i = startRowCounter; i <= endRowCounter; i++)
            {
                int a = xPosition;
                while (a<Console.BufferWidth)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                    a++;
                }               
                Console.SetCursorPosition(xPosition, i+1);
            }
            Console.BackgroundColor = ConsoleColor.Blue;
        }
        private void ClearConsoleRow(int xPosition, int yPosition)
        {

            Console.SetCursorPosition(xPosition, yPosition);
            for (int k = 0; k < Console.BufferWidth - Console.CursorLeft; k++)
            {
                Console.Write(" ");
            }
        }

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
        private Cell[,] GetTableCells()
        {
            int collumnWidth = 0;
            _TableWidth = 0;
            _SelectedItems = GetSelectedMenuItems();
            Cell[,] tableCells = new Cell[_SelectedItems.Count, _ParsedObjects.Count + 1];
            for (int i = 0; i < _SelectedItems.Count; i++)
            {
                tableCells[i, 0] = new Cell();
                if (_PreviousSelectedItems[i] == _SelectedItems[i])
                {
                    tableCells[i, 0].IsForUpdate = false;
                }
                else
                {
                    tableCells[i, 0].IsForUpdate = true;
                }
                tableCells[i, 0].Body = _SelectedItems[i];
                tableCells[i, 0].YPosition = _MenuItemsCount + 1;
                tableCells[i, 0].XPosition = _TableWidth;
                for (int j = 1; j < _ParsedObjects.Count; j++)
                {
                    tableCells[i, j] = new Cell();
                    tableCells[i, j].YPosition = tableCells[i, j - 1].YPosition + 1;
                    tableCells[i, j].XPosition = _TableWidth;

                    if (_ParsedObjects[j - 1].Fields.KeyIndexOf(_SelectedItems[i]) == -1)
                    {
                        tableCells[i, j].Body = Constants.TextForUndefinedField;

                    }
                    else
                    {
                        tableCells[i, j].Body = _ParsedObjects[j - 1].Fields[_SelectedItems[i]];
                        if (tableCells[i, j].Body.Length>tableCells[i,0].Body.Length*Constants.FieldToLongСoefficient)
                        {
                            tableCells[i, j].Body = tableCells[i, j].Body.Substring(0, tableCells[i, 0].Body.Length * Constants.FieldToLongСoefficient) + Constants.CuttingStringForTooLongField;
                        }
                    }
                    tableCells[i, j].IsForUpdate = tableCells[i, 0].IsForUpdate;
                }
                collumnWidth = GetFieldValueLength(tableCells, i, _ParsedObjects.Count);
                for (int j = 0; j < _SelectedItems.Count; j++)
                {
                    tableCells[i, j].Width = collumnWidth;
                }                
                _TableWidth += collumnWidth + Constants.IntercolumnShift;
                if (Console.BufferWidth - _TableWidth < collumnWidth)
                {
                    Console.BufferWidth = Console.BufferWidth * 2;
                }
               

            }
            int previousMenuItemsCount = 0;
            for (int i = 0; i < _PreviousSelectedItems.Count; i++)
            {
                if (_PreviousSelectedItems[i] != "")
                {
                    previousMenuItemsCount++;
                }
                _PreviousSelectedItems[i] = "";
            }
            if (previousMenuItemsCount < _SelectedItems.Count)
            {
                isTableLess = false;
            }
            else
            {
                isTableLess = true;
            }
            for (int i = 0; i < _SelectedItems.Count; i++)
            {
                _PreviousSelectedItems[i] = _SelectedItems[i];
            }
            return tableCells;

        }
        static int GetFieldValueLength(Cell[,] items, int FirstIndex, int SecondIndex)
        {
            int a=Int32.MinValue;
            for (int i = 0; i < SecondIndex; i++)
            {
                if (items[FirstIndex, i].Body.Length > a)
                {
                    a = items[FirstIndex, i].Body.Length;
                }
            }
            return a;

        }
        public void ChecOrUncheckkMenuItem(int FocusedItemIndex)
        {
            Console.SetCursorPosition(_MenuItems[FocusedItemIndex].XPosition, _MenuItems[FocusedItemIndex].YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = 0; i < _MenuWidth; i++)
            {
                Console.Write(" ");
            }
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(_MenuItems[FocusedItemIndex].XPosition, _MenuItems[FocusedItemIndex].YPosition);
            if (_MenuItems[FocusedItemIndex].IsChecked)
            {
                Console.Write(Constants.CheckedFieldOnUIPrefix + _MenuItems[FocusedItemIndex].Body);
            }
            else
            {
                Console.Write(Constants.UncheckedFieldOnUIPrefix + _MenuItems[FocusedItemIndex].Body);
            }
        }
        public void MoveToNextOrPrevMenuItem(int curentFocusedItemIndex, bool isNext)
        {               
            
            int CurrentIndex = curentFocusedItemIndex;
            int nextIndex;
            if (isNext)
            {
                nextIndex = ++curentFocusedItemIndex;
            }
            else
            {
                nextIndex = --curentFocusedItemIndex;
            }
            Console.SetCursorPosition(_MenuItems[CurrentIndex].XPosition, _MenuItems[CurrentIndex].YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i =0; i< _MenuWidth; i++)
            {
                Console.Write(" ");
               
            }
            Console.SetCursorPosition(_MenuItems[CurrentIndex].XPosition, _MenuItems[CurrentIndex].YPosition);
            if (_MenuItems[CurrentIndex].IsChecked)
            {
                Console.Write(Constants.CheckedFieldOnUIPrefix + _MenuItems[CurrentIndex].Body);
            }
            else
            {
                Console.Write(Constants.UncheckedFieldOnUIPrefix + _MenuItems[CurrentIndex].Body);
            }
            Console.SetCursorPosition(_MenuItems[nextIndex].XPosition, _MenuItems[nextIndex].YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = 0; i < _MenuWidth; i++)
            {
                Console.Write(" ");              
            }
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(_MenuItems[nextIndex].XPosition, _MenuItems[nextIndex].YPosition);
            if (_MenuItems[nextIndex].IsChecked)
            {
                Console.Write(Constants.CheckedFieldOnUIPrefix + _MenuItems[nextIndex].Body);
            }
            else
            {
                Console.Write(Constants.UncheckedFieldOnUIPrefix + _MenuItems[nextIndex].Body);
            }
            Console.SetCursorPosition(_MenuItems[nextIndex].XPosition, _MenuItems[nextIndex].YPosition);
            if (isNext)
            {
                _FocusedFieldCounter++;
            }
            else
                { _FocusedFieldCounter--; }
        }
        }
    }
