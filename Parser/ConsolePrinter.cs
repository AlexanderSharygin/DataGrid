using System;
using System.Collections.Generic;
using Parser.Extensions;

namespace Parser
{
   
   static class ConsolePrinter
    {
         private static void PrintFieldsTableByKeys(MyList<JSONObject> p_Objects, MyList<string> p_slectedKeys)
        {
            var firstRomNumber = Console.CursorTop;
            var rowCounter = firstRomNumber + 1;
            var intercolumnShift = 5;
            var columnStartX = 0;
            for (int i = 0; i < p_slectedKeys.Count; i++)
            {
                Console.SetCursorPosition(columnStartX, rowCounter);
                Console.WriteLine(p_slectedKeys[i]);
                rowCounter++;
                var maxColumnLength = PrintUtilities.MaxColuntLength(p_Objects, p_slectedKeys[i]);
                Console.SetCursorPosition(columnStartX, rowCounter);
                for (int j = 0; j < maxColumnLength + intercolumnShift; j++)
                {
                   Console.Write("-");
                }                
                rowCounter++;
                Console.SetCursorPosition(columnStartX, rowCounter);
                for (int j = 0; j < p_Objects.Count; j++)
                {
                    Console.SetCursorPosition(columnStartX, rowCounter);
                    if (p_Objects[j].Fields.KeyIndexOf(p_slectedKeys[i]) != -1)
                    {

                        if (p_Objects[j].Fields[p_slectedKeys[i]].Length > 3 * p_slectedKeys[i].Length)
                        {
                            Console.WriteLine(p_Objects[j].Fields[p_slectedKeys[i]].Substring(0, p_slectedKeys[i].Length * 3) + "...");
                        }
                        else
                        {


                            Console.WriteLine(p_Objects[j].Fields[p_slectedKeys[i]]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Поле не найдено");
                    }

                    
                    rowCounter = Console.CursorTop;
                }
                columnStartX += maxColumnLength + intercolumnShift;
                rowCounter = firstRomNumber + 1;
            }
        }   
      
        private static MyList<string> GetSelectedKeys(AgregatedKeyList _AllKeys)
        {
            MyList<string> selectedKeys = new MyList<string>();
            MyList<int> CheckedItems = new MyList<int>();
            int b = 0;
            int cursorPosition = Console.CursorTop;
            while (true)
            {
                Console.SetCursorPosition(0, cursorPosition);

                for (int i = 0; i < _AllKeys.Count; i++)
                {
                    if (i == b)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;

                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;

                    }
                    if (CheckedItems.IndexOf(i) != -1)
                    {
                        Console.WriteLine("[X] {0}", _AllKeys[i]);
                    }
                    else
                    {
                        Console.WriteLine("[ ] {0}", _AllKeys[i]);
                    }
                }
                Console.BackgroundColor = ConsoleColor.Black;
                ConsoleKey fa = Console.ReadKey().Key;

                if (fa == ConsoleKey.DownArrow)
                {
                    if (b < _AllKeys.Count - 1)
                    {
                        b++;
                    }
                    continue;
                }
                if (fa == ConsoleKey.UpArrow)
                {
                    if (b > 0)
                    {
                        b--;
                    }

                    continue;
                }

                if (fa == ConsoleKey.Spacebar)
                {
                    if (CheckedItems.IndexOf(b) == -1)
                    {
                        CheckedItems.Add(b);

                    }
                    else
                    {
                        CheckedItems.Remove(b);

                    }
                    continue;
                }
                if (fa == ConsoleKey.Enter)
                {
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
                { continue; }
            }
            for (int i = 0; i < CheckedItems.Count; i++)
            {
                var ind = CheckedItems[i];
                selectedKeys.Add(_AllKeys[ind]);
            }
            return selectedKeys;
        }
       public static void PrintJSONObjectsAsTable(MyList<JSONObject> parsedObjects)

        {
            bool foolCycle = true;
            AgregatedKeyList _AllKeys = new AgregatedKeyList(parsedObjects);
            while (foolCycle)
            {
             Console.WriteLine("Выберите набор отображаемых полей - выберите поле в списке и нажмите пробел");
            Console.WriteLine("Для вывода набора выбранных полей - нажмите Enter.");
                MyList<string> slectedKeys = GetSelectedKeys(_AllKeys);
                PrintFieldsTableByKeys(parsedObjects, slectedKeys);
                Console.CursorTop++;
                Console.WriteLine("Повторить процедуру? (Y - возврат к выбору полейб, N - выход");
                while (true)
                {
                    ConsoleKey fa = Console.ReadKey().Key;
                    if (fa == ConsoleKey.N)
                    {
                        foolCycle = false;
                        break;
                    }
                    if (fa == ConsoleKey.Y)
                    {
                        foolCycle = true;
                        Console.Clear();
                        break;
                    }
                }

            }
       
        }
    }
}