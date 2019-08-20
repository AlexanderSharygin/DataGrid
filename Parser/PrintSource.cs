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
        public int Width { get; set; }

    }
    class MenuSource
    {
        public int GlobalCounter;
        AgregatedKeyList _AllKeys;
        List<Cell> _MenuItems;
        List<List<Cell>> _AllTableItems;
        int _MaxMenuLength;
        int _SelectedFieldCounter = 0;
        MyList<JSONObject> _ParsedObjects;
        public MenuSource(MyList<JSONObject> p_parsedObjects)
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
              
            }
            
            _MaxMenuLength = _MenuItems.Max(item => item.Body.Length);
           
        }
        public void RefreshMenu()
        {
            GlobalCounter = 0;
            for (int i = 0; i < _MenuItems.Count; i++)
            {
                Console.SetCursorPosition(_MenuItems[i].XPosition, _MenuItems[i].YPosition);
                Console.BackgroundColor = ConsoleColor.Black;
                int widthCounter = 0;
                while (widthCounter < _MaxMenuLength)
                {
                    Console.Write(" ");
                    widthCounter++;
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
                    GlobalCounter++;
                }
                else
                {
                    Console.Write(Constants.UncheckedFieldOnUIPrefix + _MenuItems[i].Body);
                    GlobalCounter++;
                }
            }

        }
        public void Run()
        {
            Console.CursorVisible = false;
            RefreshMenu();
            while (true)
            {

               
                ConsoleKey pressedKeyWord = Console.ReadKey(true).Key;
                
                if (pressedKeyWord == ConsoleKey.DownArrow)
                {
                    if (_SelectedFieldCounter < _AllKeys.Count - 1)
                    {
                        _MenuItems[_SelectedFieldCounter].IsFocused = false;
                        _MenuItems[_SelectedFieldCounter + 1].IsFocused = true;
                        MoveToOtherMenuItem(_SelectedFieldCounter, true);
                     

                    }
                    continue;
                }
                if (pressedKeyWord == ConsoleKey.UpArrow)
                {
                    if (_SelectedFieldCounter > 0)
                    {
                        _MenuItems[_SelectedFieldCounter].IsFocused = false;
                        _MenuItems[_SelectedFieldCounter - 1].IsFocused = true;
                        MoveToOtherMenuItem(_SelectedFieldCounter, false);
                      
                    }
                    continue;
                }
                if (pressedKeyWord == ConsoleKey.Spacebar)
                {
                    if (_MenuItems[_SelectedFieldCounter].IsChecked == true)
                    {
                        _MenuItems[_SelectedFieldCounter].IsChecked = false;
                    }
                    else
                    {
                        _MenuItems[_SelectedFieldCounter].IsChecked = true;
                    }
                    ChecOrUncheckkMenuItem(_SelectedFieldCounter);
                    Cell[,] tableCells = GetTableCells();
                    continue;

                }
                if (pressedKeyWord == ConsoleKey.Tab)
                {
                    RefreshMenu();
                    continue;
                }
                else
                {


                    continue;
                }
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
            int ColumnXPosition = 0;
            List<string> SelectedItems = GetSelectedMenuItems();
            Cell[,]tableCells = new Cell[SelectedItems.Count, _ParsedObjects.Count+1];
            for (int i = 0; i < SelectedItems.Count; i++)
            {
                tableCells[i, 0] = new Cell();
                tableCells[i, 0].Body = SelectedItems[i];
                tableCells[i, 0].XPosition = GlobalCounter+1;
                tableCells[i, 0].XPosition = ColumnXPosition;
                for (int j = 0; j < _ParsedObjects.Count; j++)
                {
                    tableCells[i, j+1] = new Cell();
                    tableCells[i, j + 1].YPosition = tableCells[i, j].XPosition + 1;
                    tableCells[i, j + 1].XPosition = ColumnXPosition;
                    if (_ParsedObjects[j].Fields.KeyIndexOf(SelectedItems[i]) == -1)
                    {
                        tableCells[i, j + 1].Body = Constants.TextForUndefinedField;
                       
                    }
                    else
                    {
                        tableCells[i, j + 1].Body = _ParsedObjects[j].Fields[SelectedItems[i]];
                    }
                }
                ColumnXPosition +=  GetFieldValueLength(tableCells, i, _ParsedObjects.Count);
                for (int j = 0; j < SelectedItems.Count; j++)
                {
                    tableCells[i, j].Width = ColumnXPosition;
                }
            }
           
            ColumnXPosition +=  Constants.IntercolumnShift;
            return tableCells;

        }
        static int GetFieldValueLength(Cell[,] items, int FirstIndex, int SecondIndex)
        {
            int a=0;
            for (int i = 0; i < SecondIndex; i++)
            {
               a = items[FirstIndex, i].Body.Length;
            }
            return a;

        }
        public void ChecOrUncheckkMenuItem(int i)
        {
            Console.SetCursorPosition(_MenuItems[i].XPosition, _MenuItems[i].YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            int widthCounter = 0;
            while (widthCounter < _MaxMenuLength)
            {
                Console.Write(" ");
                widthCounter++;
            }
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(_MenuItems[i].XPosition, _MenuItems[i].YPosition);
            if (_MenuItems[i].IsChecked)
            {
                Console.Write(Constants.CheckedFieldOnUIPrefix + _MenuItems[i].Body);
            }
            else
            {
                Console.Write(Constants.UncheckedFieldOnUIPrefix + _MenuItems[i].Body);
            }
        }
        public void MoveToOtherMenuItem(int curentItemIndex, bool isNext)
        {
    
           
            int widthCounter = 0;
            int CurrentIndex = curentItemIndex;
            int nextIndex;
            if (isNext)
            {
                nextIndex = ++curentItemIndex;
            }
            else
            {
                nextIndex = --curentItemIndex;
            }
            Console.SetCursorPosition(_MenuItems[CurrentIndex].XPosition, _MenuItems[CurrentIndex].YPosition);
            Console.BackgroundColor = ConsoleColor.Black;
            while (widthCounter < _MaxMenuLength)
            {

                Console.Write(" ");
                widthCounter++;
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
            widthCounter = 0;
            while (widthCounter < _MaxMenuLength)
            {
              
                Console.Write(" ");
                widthCounter++;
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
                _SelectedFieldCounter++;
            }
            else
                { _SelectedFieldCounter--; }
        }
        }
    }
