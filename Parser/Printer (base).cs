using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Extensions;

namespace Parser
{
    class Printer
    {
      
        AgregatedKeyList AllKeys;
        private int rowCount = 0;
        static int GetLastNameLength(JSONObject p_item, string key)

        {
            return p_item.Fields[key].Length;
        }
        public void GetDataForPrint(MyList<JSONObject> p_Objects)
        {
            MyList<string> dataKeys = GetKeys();
            var startRomNumber = Console.CursorTop;
            var rowCounter = startRomNumber;
            var columnsXShift = 5;
            var columnStartX = 0;//править
           
           Console.SetCursorPosition(0, rowCounter);
            for (int i = 0; i < dataKeys.Count; i++)
            {
                Console.SetCursorPosition(columnStartX, rowCounter);
                Console.WriteLine(dataKeys[i]);
                rowCounter++;
                for (int j = 0; j < p_Objects.Count; j++)
                {
                    Console.SetCursorPosition(columnStartX, rowCounter);
                    if (p_Objects[j].Fields[dataKeys[i]].Length > 3 * dataKeys[i].Length)
                    {
                        Console.WriteLine(p_Objects[j].Fields[dataKeys[i]].Substring(0, dataKeys[i].Length*3) + "...");
                    }
                    else
                    {
                        Console.WriteLine(p_Objects[j].Fields[dataKeys[i]]);
                        
                    }
                    rowCounter = Console.CursorTop;
                }
                var ObjectsDataKeyFieldMaxLength = p_Objects.MaxValue(dataKeys[i], GetLastNameLength);
             
                int def= dataKeys[i].Length;
                if (ObjectsDataKeyFieldMaxLength > def)
                {
                    if (ObjectsDataKeyFieldMaxLength > 3 * def)
                    {
                        def = def * 3 + 3;
                    }
                    else
                    {
                        def = ObjectsDataKeyFieldMaxLength;
                    }
                }
                
                columnStartX += def;
                columnStartX = columnStartX+columnsXShift;
                
                rowCounter = startRomNumber;
               
                
            }
        }
        public MyList<string> GetKeys()
        {
            MyList<string> selectedKeys = new MyList<string>();
            while (true)
            {
                //  try


                Console.Write("Введите значение:");
                int a = 0;
                bool bb = Int32.TryParse(Console.ReadLine(), out a);
                if (bb)
                {
                    if (a <= AllKeys.Count && a > 0)
                    {
                        bool b = selectedKeys.AddUnical(AllKeys[a - 1]);
                        if (b == false)
                        {
                            Console.WriteLine("Это значение уже выбрано. Выберите другое");
                        }
                    }
                    if (a == 0)
                    {
                        break;
                    }
                    if (a > AllKeys.Count + 1 || a <= 0)
                    {
                        Console.WriteLine("Введите число в диапазоне от 1 до {0}, или введите 0 для перехода к выводу данных.", AllKeys.Count);
                    }
                }
              if(!bb)
               {
                    Console.WriteLine("Введено не допустимое значение. Повторите ввод");
               }
            }
           selectedKeys.TrimExcessObjects();
            return selectedKeys;
        }


        public void PrintJSONObjectsPropertiesOnConsole(MyList<JSONObject> parsedObjects)

        {

            parsedObjects.TrimExcessObjects();
            AllKeys = new AgregatedKeyList(parsedObjects);
            
            AllKeys.GetKeys.TrimExcessObjects();
            Console.WriteLine("Выберите набор отображаемых полей - введите номер поля и нажмите Enter.");
            rowCount++;
            Console.WriteLine("Затем добавьте к набору другие поля или перейдите к выводу выбранного поля/набора полей.");
            rowCount++;
            Console.WriteLine("Для вывода набора выбранных полей - введите 0 и нажмите Enter.");
            rowCount++;
            int index = 0;
          foreach (var item in AllKeys.GetKeys)
            {
                Console.WriteLine("{0} - {1}", ++index, item);
            }
            GetDataForPrint(parsedObjects);
           
            
            
          

                 
            
         
          //  var rowCounter = AllKeys.Count+4;
           // var distanceBetweenColumns = 5;
            #region comments                    
            // int FirstNameMaxLength = MyLinq.MaxValue(parsedObjects, SelectorFirstName);                                       
            // int FirstNameMaxLength = parsedObjects.MaxValue(n => { int f = n.FirstName.Length;    return f; });
            #endregion
            /*
            var ParsedObjectsFirstNameMaxLength = parsedObjects.MaxValue(n => n.Fields["FirstName"].Length);
            var ParsedObjectsLastNameMaxLength = parsedObjects.MaxValue(GetLastNameLength);
            if (ParsedObjectsFirstNameMaxLength < "FirstName".Length)
            {
                ParsedObjectsFirstNameMaxLength = "FirstName".Length;
            }
            if (ParsedObjectsLastNameMaxLength < "LastName".Length)
            {
                ParsedObjectsLastNameMaxLength = "LastName".Length;
            }*/
            //
         /*   Console.SetCursorPosition(0, rowCounter);
            Console.Write("FirstName");
            Console.SetCursorPosition(ParsedObjectsFirstNameMaxLength + distanceBetweenColumns, rowCounter);
            Console.Write("LastName");
            rowCounter++;
            Console.SetCursorPosition(0, rowCounter);
            for (var i = 0; i < ParsedObjectsFirstNameMaxLength + ParsedObjectsLastNameMaxLength + distanceBetweenColumns; i++)
            {
                Console.Write('-');
            }
            rowCounter++;*/
           /* Console.SetCursorPosition(0, rowCounter); ;
            for (var i = 0; i < parsedObjects.Count; i++)
            {
                Console.SetCursorPosition(0, rowCounter);
                Console.Write(parsedObjects[i].Fields["FirstName"]);
                Console.SetCursorPosition(ParsedObjectsFirstNameMaxLength + distanceBetweenColumns, rowCounter);
                Console.Write(parsedObjects[i].Fields["LastName"]);
                rowCounter++;
            }*/
            Console.ReadLine();
        }


    }
}
