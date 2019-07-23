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




        static int GetLastNameLength(JSONObject item)

        {

            return item.Fields["LastName"].Length;
        }
       
        public void PrintJSONObjectsPropertiesOnConsole(MyList<JSONObject> parsedObjects)
        {

            parsedObjects.TrimExcessObjects();
            AgregatedKeyList AllKeys = new AgregatedKeyList(parsedObjects);
            AllKeys.GetKeys.TrimExcessObjects();
            Console.WriteLine("Выберите набор отображаемых полей - введите номер поля и нажмите Enter.");
            Console.WriteLine("Затем добавьте к набору другие поля или перейдите к выводу выбранного поля/набора полей.");
            Console.WriteLine("Для вывода набора выбранных полей - введите 0 и нажмите Enter.");
            int index = 0;
            foreach (var item in AllKeys.GetKeys)
           {
               
               Console.WriteLine("{0} - {1}", ++index, item);
            }
         
            var rowCounter = AllKeys.Count+4;
            var distanceBetweenColumns = 5;
            #region comments                    
            // int FirstNameMaxLength = MyLinq.MaxValue(parsedObjects, SelectorFirstName);                                       
            // int FirstNameMaxLength = parsedObjects.MaxValue(n => { int f = n.FirstName.Length;    return f; });
            #endregion

            var ParsedObjectsFirstNameMaxLength = parsedObjects.MaxValue(n => n.Fields["FirstName"].Length);
            var ParsedObjectsLastNameMaxLength = parsedObjects.MaxValue(GetLastNameLength);
            if (ParsedObjectsFirstNameMaxLength < "FirstName".Length)
            {
                ParsedObjectsFirstNameMaxLength = "FirstName".Length;
            }
            if (ParsedObjectsLastNameMaxLength < "LastName".Length)
            {
                ParsedObjectsLastNameMaxLength = "LastName".Length;
            }
            //
            Console.SetCursorPosition(0, rowCounter);
            Console.Write("FirstName");
            Console.SetCursorPosition(ParsedObjectsFirstNameMaxLength + distanceBetweenColumns, rowCounter);
            Console.Write("LastName");
            rowCounter++;
            Console.SetCursorPosition(0, rowCounter);
            for (var i = 0; i < ParsedObjectsFirstNameMaxLength + ParsedObjectsLastNameMaxLength + distanceBetweenColumns; i++)
            {
                Console.Write('-');
            }
            rowCounter++;
            Console.SetCursorPosition(0, rowCounter); ;
            for (var i = 0; i < parsedObjects.Count; i++)
            {
                Console.SetCursorPosition(0, rowCounter);
                Console.Write(parsedObjects[i].Fields["FirstName"]);
                Console.SetCursorPosition(ParsedObjectsFirstNameMaxLength + distanceBetweenColumns, rowCounter);
                Console.Write(parsedObjects[i].Fields["LastName"]);
                rowCounter++;
            }
            Console.ReadLine();
        }


    }
}
