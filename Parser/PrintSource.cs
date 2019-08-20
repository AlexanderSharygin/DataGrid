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

    }
    class MenuSource
    {
        public int GlobalCounter;
        AgregatedKeyList _AllKeys;
        List<Cell> _MenuItems;
        int _MaxMenuLength;
        int _SelectedFieldCounter = 0;
        public MenuSource(MyList<JSONObject> p_parsedObjects)
        {
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
                }
                else
                {
                    Console.Write(Constants.UncheckedFieldOnUIPrefix + _MenuItems[i].Body);
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

                }
                else
                {


                    continue;
                }
            }
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
