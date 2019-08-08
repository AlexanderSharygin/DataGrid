using Parser.Extensions;
using System;

namespace Parser
{

    static class ConsolePrinter
    {
         private static void PrintFieldsAsTableByKeys(MyList<JSONObject> p_Objects, MyList<string> p_slectedKeys)
        {
            var firstRomNumber = Console.CursorTop;
            var rowCounter = firstRomNumber + 1;
            var columnStartX = 0;
            for (int i = 0; i < p_slectedKeys.Count; i++)
            {
                Console.SetCursorPosition(columnStartX, rowCounter);
                Console.WriteLine(p_slectedKeys[i]);
                rowCounter++;
                var maxColumnLength = MaxColumnLength(p_Objects, p_slectedKeys[i]);
                Console.SetCursorPosition(columnStartX, rowCounter);
                for (int j = 0; j < maxColumnLength + Constants.IntercolumnShift; j++)
                {
                   Console.Write("-");
                }                
                rowCounter++;
                Console.SetCursorPosition(columnStartX, rowCounter);
                for (int j = 0; j < p_Objects.Count; j++)
                {
                    Console.SetCursorPosition(columnStartX, rowCounter);
                    if (p_Objects[j].Fields.KeyIndexOf(p_slectedKeys[i]) == -1)
                    {
                        Console.WriteLine(Constants.TextForUndefinedField);
                    }
                    else
                    {
                        if (IsFieldForColumnToLong(p_Objects[j].Fields[p_slectedKeys[i]].Length, p_slectedKeys[i].Length))
                        {
                          
                            Console.WriteLine(p_Objects[j].Fields[p_slectedKeys[i]].Substring(0, p_slectedKeys[i].Length * Constants.FieldToLongСoefficient) + Constants.CuttingStringForTooLongField);
                        }
                        else
                        {
                            Console.WriteLine(p_Objects[j].Fields[p_slectedKeys[i]]);
                        }
                    }              
                    
                    rowCounter = Console.CursorTop;
                }
                columnStartX += maxColumnLength + Constants.IntercolumnShift;
               rowCounter = firstRomNumber + 1;
            }
        }
        private static void PrintCurrentMenuView(AgregatedKeyList p_AllMenuItems, MyList<int> p_CheckedMenuItems, int p_selectedFieldCounter)
        {
            for (int i = 0; i < p_AllMenuItems.Count; i++)
            {
                if (i == p_selectedFieldCounter)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;

                }
                if (p_CheckedMenuItems.IndexOf(i) == -1)
                {
                    Console.WriteLine(Constants.UncheckedFieldOnUIPrefix + p_AllMenuItems[i]);

                }
                else
                {
                    Console.WriteLine(Constants.CheckedFieldOnUIPrefix + p_AllMenuItems[i]);
                }
            }
        }

        private static MyList<string> GetSelectedKeysFromUI(AgregatedKeyList _AllKeys)
        {
            MyList<string> selectedKeys = new MyList<string>();
            MyList<int> CheckedItems = new MyList<int>();
            int selecteFieldCounter = 0;
            int cursorTopPosition = Console.CursorTop;
            while (true)
            {
                Console.SetCursorPosition(0, cursorTopPosition);
                PrintCurrentMenuView(_AllKeys, CheckedItems, selecteFieldCounter);
                Console.BackgroundColor = ConsoleColor.Black;
                ConsoleKey pressedKeyWord = Console.ReadKey().Key;
                if (pressedKeyWord == ConsoleKey.DownArrow)
                {
                    if (selecteFieldCounter < _AllKeys.Count - 1)
                    {
                        selecteFieldCounter++;
                    }
                    continue;
                }
                if (pressedKeyWord == ConsoleKey.UpArrow)
                {
                    if (selecteFieldCounter > 0)
                    {
                        selecteFieldCounter--;
                    }

                    continue;
                }

                if (pressedKeyWord == ConsoleKey.Spacebar)
                {
                    if (CheckedItems.IndexOf(selecteFieldCounter) == -1)
                    {
                        CheckedItems.Add(selecteFieldCounter);

                    }
                    else
                    {
                        CheckedItems.Remove(selecteFieldCounter);

                    }
                    continue;
                }
                if (pressedKeyWord == ConsoleKey.Enter)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, cursorTopPosition);

                    for (int i = 0; i < _AllKeys.Count; i++)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        if (CheckedItems.IndexOf(i) != -1)
                        {
                            Console.WriteLine("[X] {0}", _AllKeys[i]);
                        }
                        else
                        {
                            Console.WriteLine("[ ] {0}", _AllKeys[i]);
                        }
                    }
                    break;
                }

                else
                {
                    continue;
                }
            }
            for (int i = 0; i < CheckedItems.Count; i++)
            {
                selectedKeys.Add(_AllKeys[CheckedItems[i]]);
            }                     
            return selectedKeys;
            
        }
        public static void PrintJSONObjectsAsTable(MyList<JSONObject> parsedObjects)

        {
            AgregatedKeyList _AllKeys = new AgregatedKeyList(parsedObjects);
            while (true)
            {
                Console.CursorTop = 0;
                Console.WriteLine("Определите набор отображаемых полей - выберите поле в списке и нажмите пробел.");
                Console.WriteLine("Для вывода набора выбранных полей - нажмите Enter.");
                MyList<string> slectedKeys = GetSelectedKeysFromUI(_AllKeys);
                PrintFieldsAsTableByKeys(parsedObjects, slectedKeys);
             
            }
        }          

        public static int MaxColumnLength(MyList<JSONObject> p_Objects, string p_key)
        {
            var maxColumnLength = p_Objects.MaxValue(p_key, GetFieldValueLength);
            var currentColumnLength = p_key.Length;
            if (maxColumnLength > currentColumnLength)
            {
                if (IsFieldForColumnToLong(maxColumnLength, currentColumnLength))
                {
                    currentColumnLength = currentColumnLength * Constants.FieldToLongСoefficient + Constants.CuttingStringForTooLongField.Length;
                }
                else
                {
                    currentColumnLength = maxColumnLength;
                }
            }
            return currentColumnLength;
        }
        public static bool IsFieldForColumnToLong(int p_FieldLength, int p_columnLength)
        {
            if (p_FieldLength > Constants.FieldToLongСoefficient * p_columnLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static int GetFieldValueLength(JSONObject p_item, string key)
        {
            if (p_item.Fields.KeyIndexOf(key) == -1)
            {

                return Constants.TextForUndefinedField.Length;
            }
            else
            {
                return p_item.Fields[key].Length;
            }
        }
        // IsValid sounds better. It's more consistent with existing validation API.
        // excessive abstraction. C# is powerful enough to express this logic in a single expression at the place where it should be applied
        // https://trello.com/c/aWrnk1oi/10-c-and-net-platform-features
        public static bool IsInputKeyValid(int keyNumber, int maxKeyValue)
        {
            if (keyNumber <= maxKeyValue && keyNumber >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}