using System;
using Parser.Extensions;

namespace Parser
{
    class ConsolePrinter:Printer
    {

        AgregatedKeyList _AllKeys;
        public void PrintFieldsTableByKeys(MyList<JSONObject> p_Objects, MyList<string> p_slectedKeys)
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
                var maxColumnLength = MaxColuntLength(p_Objects, p_slectedKeys[i]);
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
                    if (p_Objects[j].Fields[p_slectedKeys[i]].Length > 3 * p_slectedKeys[i].Length)
                    {
                        Console.WriteLine(p_Objects[j].Fields[p_slectedKeys[i]].Substring(0, p_slectedKeys[i].Length * 3) + "...");
                    }
                    else
                    {
                        int a = p_Objects[j].Fields.ValueIndexOf(p_slectedKeys[i]);
                        if (a != -1)
                        {
                            Console.WriteLine(p_Objects[j].Fields[p_slectedKeys[i]]);
                        }
                        else
                        {
                            Console.WriteLine("Не найдено");
                        }

                    }
                    rowCounter = Console.CursorTop;
                }
                columnStartX += maxColumnLength + intercolumnShift;
                rowCounter = firstRomNumber + 1;
            }
        }   
     
        public MyList<string> GetSelectedKeys()
        {
            MyList<string> selectedKeys = new MyList<string>();
            while (true)
            {
                Console.Write("Введите значение:");
                var keyIndex = 0;
                bool isInputKeyNumberParsed = Int32.TryParse(Console.ReadLine(), out keyIndex);
                if (isInputKeyNumberParsed)
                {
                    if (IsInputKeyValid(keyIndex, _AllKeys.Count))
                    {
                        if (keyIndex == 0)
                        {
                            break;
                        }
                        if (selectedKeys.AddisUniqueItem(_AllKeys[keyIndex - 1]) == false)
                        {
                            Console.WriteLine("Это значение уже выбрано. Выберите другое");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Введите число в диапазоне от 1 до {0}, или введите 0 для перехода к выводу данных.", _AllKeys.Count);
                    }
                }
                if (!isInputKeyNumberParsed)
                {
                    Console.WriteLine("Введено не допустимое значение. Повторите ввод");
                }
            }
           return selectedKeys;
        }
       
        public void PrintJSONObjectsPropertiesOnConsole(MyList<JSONObject> parsedObjects)

        {         
            _AllKeys = new AgregatedKeyList(parsedObjects);
            Console.WriteLine("Выберите набор отображаемых полей - введите номер поля и нажмите Enter.");
            Console.WriteLine("Затем добавьте к набору другие поля или перейдите к выводу выбранного поля/набора полей.");
            Console.WriteLine("Для вывода набора выбранных полей - введите 0 и нажмите Enter.");
            for (int i = 0; i < _AllKeys.GetKeys.Count; i++)
            {
                Console.WriteLine("{0} - {1}", i + 1, _AllKeys.GetKeys[i]);
            }
            MyList<string> slectedKeys = GetSelectedKeys();
            PrintFieldsTableByKeys(parsedObjects, slectedKeys);
            Console.ReadLine();
        }
    }
}

