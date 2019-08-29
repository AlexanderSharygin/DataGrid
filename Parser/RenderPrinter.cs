﻿using System;
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
    class ConsoleRender
    {
        public int _MenuItemsCount;
        public int _TableWidth;
        AgregatedKeyList _AllObjectsFields;
        List<Cell> _MenuItems;
        int _MenuWidth;
        int _FocusedMenuItemCounter = 0;
        MyList<JSONObject> _JSONObjects;
        List<String> _PrevSelectedMenuItems = new List<string>();
        List<string> _SelectedMenuItems;
        bool isTableLess;
        public ConsoleRender(MyList<JSONObject> p_parsedObjects)
        {
            _JSONObjects = p_parsedObjects;
            _AllObjectsFields = new AgregatedKeyList(p_parsedObjects);
            _MenuItems = new List<Cell>(_AllObjectsFields.Count);
            for (int i = 0; i < _AllObjectsFields.Count; i++)
            {
                _MenuItems.Add(new Cell());
                _MenuItems[i].Body = _AllObjectsFields[i];
                _MenuItems[i].XPosition = 0;
                _MenuItems[i].YPosition = i + Constants.PreambleStrings.Length;
                _MenuItems[i].IsChecked = false;
                _PrevSelectedMenuItems.Add("");
            }
            _MenuItems[0].IsFocused = true;
            _MenuWidth = _MenuItems.Max(item => item.Body.Length);
        }
        public void ReWritehMenu()
        {
            _MenuItemsCount = 0;
            for (int i = 0; i < _MenuItems.Count; i++)
            {
                Console.SetCursorPosition(_MenuItems[i].XPosition, _MenuItems[i].YPosition);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("".PadLeft(_MenuWidth));
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

        public void RendreUI()
        {
            Console.CursorVisible = false;
            Console.BufferWidth = Constants.MinWidthConsoleBufer;
            for (int i = 0; i < Constants.PreambleStrings.Length; i++)
            {
                Console.WriteLine(Constants.PreambleStrings[i]);
            }
            ReWritehMenu();
            while (true)
            {
                bool isConsoleCleared = false;
                ConsoleKey pressedKeyWord = Console.ReadKey(true).Key;
                if (pressedKeyWord == ConsoleKey.DownArrow)
                {
                    if (_FocusedMenuItemCounter < _AllObjectsFields.Count - 1)
                    {
                        _MenuItems[_FocusedMenuItemCounter].IsFocused = false;
                        _MenuItems[_FocusedMenuItemCounter + 1].IsFocused = true;
                        MoveToNextOrPrevMenuItem(_FocusedMenuItemCounter, true);
                    }
                    continue;
                }
                if (pressedKeyWord == ConsoleKey.UpArrow)
                {
                    if (_FocusedMenuItemCounter > 0)
                    {
                        _MenuItems[_FocusedMenuItemCounter].IsFocused = false;
                        _MenuItems[_FocusedMenuItemCounter - 1].IsFocused = true;
                        MoveToNextOrPrevMenuItem(_FocusedMenuItemCounter, false);
                    }
                    continue;
                }
                if (pressedKeyWord == ConsoleKey.Spacebar)
                {
                    if (_MenuItems[_FocusedMenuItemCounter].IsChecked == true)
                    {
                        _MenuItems[_FocusedMenuItemCounter].IsChecked = false;
                    }
                    else
                    {
                        _MenuItems[_FocusedMenuItemCounter].IsChecked = true;
                    }
                    ChecOrUncheckkMenuItem(_FocusedMenuItemCounter);
                    Cell[,] tableCells = GetTableCells();
                    for (int i = 0; i < _SelectedMenuItems.Count; i++)
                    {
                        if (tableCells[i, 0].IsForUpdate == true)
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
                        else
                        {
                            continue;
                        }
                    }
                    if (isTableLess)
                    {
                        ClearToEndConsole(_TableWidth, _MenuItemsCount + Constants.PreambleStrings.Length + 1);
                    }
                    if (_TableWidth > Console.WindowWidth)
                    {
                        Console.BufferWidth = _TableWidth + 2;
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
            int startRowCounter = _MenuItemsCount + Constants.PreambleStrings.Length + 1;
            int endRowCounter = startRowCounter + _JSONObjects.Count + 1;
            Console.SetCursorPosition(xPosition, yPosition);
            for (int i = startRowCounter; i <= endRowCounter; i++)
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
            int currentCollumnWidth = 0;
            _TableWidth = 0;
            _SelectedMenuItems = GetSelectedMenuItems();
            Cell[,] tableCells = new Cell[_SelectedMenuItems.Count, _JSONObjects.Count + 2];
            for (int i = 0; i < _SelectedMenuItems.Count; i++)
            {
                tableCells[i, 0] = new Cell();
                if (_PrevSelectedMenuItems[i] == _SelectedMenuItems[i])
                {
                    tableCells[i, 0].IsForUpdate = false;
                }
                else
                {
                    tableCells[i, 0].IsForUpdate = true;
                }
                tableCells[i, 0].Body = _SelectedMenuItems[i];
                tableCells[i, 0].YPosition = _MenuItemsCount + Constants.CountEmptyStringsBetweenMenuAndItable + Constants.PreambleStrings.Length;
                tableCells[i, 0].XPosition = _TableWidth;
                tableCells[i, 1] = new Cell();
                tableCells[i, 1].YPosition = tableCells[i, 0].YPosition + 1;
                tableCells[i, 1].XPosition = _TableWidth;
                tableCells[i, 1].IsForUpdate = tableCells[i, 0].IsForUpdate;
                tableCells[i, 1].Body = "";
                for (int j = 2; j < _JSONObjects.Count + 2; j++)
                {
                    tableCells[i, j] = new Cell();
                    tableCells[i, j].YPosition = tableCells[i, j - 1].YPosition + 1;
                    tableCells[i, j].XPosition = _TableWidth;
                    if (_JSONObjects[j - 2].Fields.KeyIndexOf(_SelectedMenuItems[i]) == -1)
                    {
                        tableCells[i, j].Body = Constants.TextForUndefinedField;
                    }
                    else
                    {
                        tableCells[i, j].Body = _JSONObjects[j - 2].Fields[_SelectedMenuItems[i]];
                        if (tableCells[i, j].Body.Length > tableCells[i, 0].Body.Length * Constants.FieldToLongСoefficient)
                        {
                            tableCells[i, j].Body = tableCells[i, j].Body.Substring(0, tableCells[i, 0].Body.Length * Constants.FieldToLongСoefficient) + Constants.CuttingStringForTooLongField;
                        }
                    }
                    tableCells[i, j].IsForUpdate = tableCells[i, 0].IsForUpdate;
                }
                currentCollumnWidth = GetFieldValueLength(tableCells, i, _JSONObjects.Count);
                for (int j = 0; j < _SelectedMenuItems.Count + 1; j++)
                {
                    tableCells[i, j].Width = currentCollumnWidth;
                }
                tableCells[i, 1].Body = "".PadLeft(tableCells[i, 1].Width + Constants.IntercolumnShift, '-');
                _TableWidth += currentCollumnWidth + Constants.IntercolumnShift;
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
        static int GetFieldValueLength(Cell[,] items, int FirstIndex, int SecondIndex)
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
        public void ChecOrUncheckkMenuItem(int FocusedItemIndex)
        {
            Console.SetCursorPosition(_MenuItems[FocusedItemIndex].XPosition, _MenuItems[FocusedItemIndex].YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("".PadLeft(_MenuWidth));
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
        public void MoveToNextOrPrevMenuItem(int curentFocusedItemIndex, bool isMoveToNext)
        {
            int CurrentIndex = curentFocusedItemIndex;
            int nextIndex;
            if (isMoveToNext)
            {
                nextIndex = ++curentFocusedItemIndex;
            }
            else
            {
                nextIndex = --curentFocusedItemIndex;
            }
            Console.SetCursorPosition(_MenuItems[CurrentIndex].XPosition, _MenuItems[CurrentIndex].YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("".PadLeft(_MenuWidth));
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
            Console.Write("".PadLeft(_MenuWidth));
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
            if (isMoveToNext)
            {
                _FocusedMenuItemCounter++;
            }
            else
            {
                _FocusedMenuItemCounter--;
            }
        }
    }
}