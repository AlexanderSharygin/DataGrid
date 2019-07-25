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
        static int GetLastNameLength(JSONObject item)

        {
            return item.Fields["LastName"].Length;
        }
        public void GetDataForPrint(MyList<JSONObject> p_Objects)
        {
            MyList<string> dataKeys = GetKeys();
            var rowCounter = 20;
            var distanceBetweenColumns = 5;
            var columnDefWide = 0;//править
            Console.SetCursorPosition(0, rowCounter);
            for (int i = 0; i < dataKeys.Count; i++)
            {
                Console.SetCursorPosition(columnDefWide, rowCounter);
                Console.WriteLine(dataKeys[i]);
                rowCounter++;
                for (int j = 0; j < p_Objects.Count; j++)
                {
                    Console.SetCursorPosition(columnDefWide, rowCounter);
                    Console.WriteLine( p_Objects[j].Fields[dataKeys[i]]);
                    rowCounter++;
                }
                columnDefWide = columnDefWide + 30;
                rowCounter = 20;
                ;
            }
        }
        public MyList<string> GetKeys()
        {
            MyList<string> selectedKeys = new MyList<string>();
            while (true)
            {
                try
                {
                    Console.Write("Введите значение:");
                    var a = Convert.ToInt32(Console.ReadLine());
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
                catch
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
            Console.WriteLine("Затем добавьте к набору другие поля или перейдите к выводу выбранного поля/набора полей.");
            Console.WriteLine("Для вывода набора выбранных полей - введите 0 и нажмите Enter.");
            GetDataForPrint(parsedObjects);
            // int index = 0;
           /* foreach (var item in AllKeys.GetKeys)
           {               
               Console.WriteLine("{0} - {1}", ++index, item);
            }*/
          

                 
            
         
            var rowCounter = AllKeys.Count+4;
            var distanceBetweenColumns = 5;
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
