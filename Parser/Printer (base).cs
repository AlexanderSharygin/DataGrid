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
            return item.LastName.Length;
        }
       
        public void PrintJSONObjectsPropertiesOnConsole(JSONObject[] parsedObjects)
        {
            var rowCounter = 0;
            var distanceBetweenColumns = 5; 
            #region comments                    
            // int FirstNameMaxLength = MyLinq.MaxValue(parsedObjects, SelectorFirstName);                                       
            // int FirstNameMaxLength = parsedObjects.MaxValue(n => { int f = n.FirstName.Length;    return f; });
            #endregion
            var ParsedObjectsFirstNameMaxLength = parsedObjects.MaxValue(n => n.FirstName.Length);
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
            for (var i = 0; i < parsedObjects.Length; i++)
            {
                Console.SetCursorPosition(0, rowCounter);
                Console.Write(parsedObjects[i].FirstName);
                Console.SetCursorPosition(ParsedObjectsFirstNameMaxLength + distanceBetweenColumns, rowCounter);
                Console.Write(parsedObjects[i].LastName);
                rowCounter++;
            }
            Console.ReadLine();
        }


    }
}
