using System;
using System.IO;

namespace Parser
{
    public static class StringExtension
    {
        public static void ParseSimpleJSON(this String str)
        {
            SimpleJSONParser.ParseSimpleJSON(str);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
           OutpuAndInputData.GetTextOfSimpleJSON().ParseSimpleJSON();          
        }
    }
    static class OutpuAndInputData
    {
        public static string GetTextOfSimpleJSON()
        {
            StreamReader FileReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "data.txt" );
            return FileReader.ReadToEnd();          
        }
        public static void ShowResult(ParsedJSONObject[] parsedObject)
        {
            var rowCounter = 0;
            var columnShift = 5;
            var FirstNameMaxLength = "FirstName".Length;
            var LastNameMaxLength = "LastName".Length;
            for (var i = 0; i < parsedObject.Length; i++)
            {
                var FirstNameLength = parsedObject[i].FirstName.Length;
                if (FirstNameLength > FirstNameMaxLength)
                {
                    FirstNameMaxLength = FirstNameLength;
                }
                var LastNameLength = parsedObject[i].LastName.Length;
                if (LastNameLength > LastNameMaxLength)
                {
                    {
                        LastNameMaxLength = LastNameLength;
                    }
                }
            }           
            Console.SetCursorPosition(0,rowCounter);
            Console.Write("FirstName");
            Console.SetCursorPosition(FirstNameMaxLength + columnShift,rowCounter);
            Console.Write("LastName");
            rowCounter++;
            Console.SetCursorPosition(0, rowCounter);
            for (var i = 0; i < FirstNameMaxLength + LastNameMaxLength + columnShift; i++)
            {
                Console.Write('-');
            }
            rowCounter++;
            Console.SetCursorPosition(0, rowCounter); ;
            for (var i = 0; i < parsedObject.Length; i++)
            {
               
                Console.SetCursorPosition(0, rowCounter);
                Console.Write(parsedObject[i].FirstName);
                Console.SetCursorPosition(FirstNameMaxLength + columnShift, rowCounter);
                Console.Write(parsedObject[i].LastName);
                rowCounter++;
            }
            Console.ReadLine();
        }
    }
    static class SimpleJSONParser
    {
        public static void ParseSimpleJSON(string simpleJSONFileText)
        {
            string[] TextOfJSONObjects= GetTextOfJSOONObjects(simpleJSONFileText);
            ParsedJSONObject[] SimpleJSONObjects = ParseObjectstoStrings(TextOfJSONObjects);
            ParseStringsOfEachJSONObject(SimpleJSONObjects);
            OutpuAndInputData.ShowResult(SimpleJSONObjects);
        }
        private static string[] GetTextOfJSOONObjects(string simpleJSONText)
        {
            string[] textOfJSONObjects = new string[0];
            var counter = 0;
            var objectStartPositonInText = 0;
            var objectEndPositionInText = 0;
            var objectNumber = 0;
            while (counter < simpleJSONText.Length )
            {
                if (simpleJSONText[counter] == '{')
                {
                    objectStartPositonInText = counter;
                    objectEndPositionInText = GetObjectEndPositionInText(simpleJSONText, counter);
                    Array.Resize(ref textOfJSONObjects, objectNumber + 1);
                    textOfJSONObjects[objectNumber] = simpleJSONText.Substring(objectStartPositonInText, objectEndPositionInText - objectStartPositonInText + 1);
                    objectNumber++;
                    counter = objectEndPositionInText;
                }
                else
                {
                    counter++;
                }
            }
            return textOfJSONObjects;
        }
        private static int GetObjectEndPositionInText(string simpleJSONText, int counter)
        {
            int endObjectPosition = 0;
            for (var i = counter; i < simpleJSONText.Length; i++)
            {
                if (simpleJSONText[i] == '"')
                {
                    counter = GetPositionOfClosingComma(simpleJSONText, ++i);
                }
                if (simpleJSONText[i] == '}')
                {
                    endObjectPosition = i;
                    break;
                }
            }
            return endObjectPosition - 1;
        }
        private static int GetPositionOfClosingComma(string simpleJSONText, int counter)
        {
            int positionOfClosingComma = 0;
            for (var i = counter; i < simpleJSONText.Length; i++)
            {
                if (simpleJSONText[i] == '"')
                {
                    counter = i;
                    counter++;
                    positionOfClosingComma = counter;
                    break;
                }
                else
                {
                    counter++;
                }
            }
            return positionOfClosingComma;
        }
        private static int GetEndOfStringInJSOObject(string simpleJSONText, int counter)
        {
            int endStringPosition = 0;
            for (int i = ++counter; i < simpleJSONText.Length - 1; i++)
            {
                if ((simpleJSONText[counter] == '\r' && simpleJSONText[counter + 1] == '\n') || (simpleJSONText[counter] == '\r' && simpleJSONText[counter + 1] != '\n') || (simpleJSONText[counter] == '\n' && simpleJSONText[counter - 1] != '\r'))
                {
                    endStringPosition = counter;
                    break;
                }
                if (simpleJSONText[counter] == '"')
                {
                    counter = GetPositionOfClosingComma(simpleJSONText, ++counter);
                }
                else
                {
                    counter++;
                }
            }
            return endStringPosition;
        }
        private static ParsedJSONObject[] ParseObjectstoStrings(string[] textOfJSONObjects)
        {
            ParsedJSONObject[] ParsedJSONObjects = new ParsedJSONObject[textOfJSONObjects.Length];
            for (int i = 0; i < textOfJSONObjects.Length; i++)
            {
                string[] JSONObjectString = new string[0];
                var countOfStringInJSONObject = 0;
                var counter = 0;
                var startStringPosition = 0;
                var endStringPosition = 0;
                string textOfJSONObject = textOfJSONObjects[i];
                while (counter < textOfJSONObjects[i].Length)
                {
                    if ((textOfJSONObject[counter] == '\r' && textOfJSONObject[counter + 1] == '\n') || (textOfJSONObject[counter] == '\r' && textOfJSONObject[counter + 1] != '\n') || (textOfJSONObject[counter] == '\n' && textOfJSONObject[counter - 1] != '\r'))
                    {
                        startStringPosition = counter;
                        endStringPosition = GetEndOfStringInJSOObject(textOfJSONObject, counter + 1);
                        if (endStringPosition != 0)
                        {
                            Array.Resize(ref JSONObjectString, JSONObjectString.Length + 1);                                                  
                            JSONObjectString[countOfStringInJSONObject] = textOfJSONObject.Substring(startStringPosition, endStringPosition - startStringPosition).Trim(',','\n','\r', Convert.ToChar(" "));
                            countOfStringInJSONObject++;
                            counter = endStringPosition;
                        }
                        if (endStringPosition == 0)
                        {
                            counter++;
                        }                    
                    }
                    else
                    {
                        counter++;
                    }
                }
                ParsedJSONObjects[i] = new ParsedJSONObject(JSONObjectString.Length);
                ParsedJSONObjects[i].StringsToParse = JSONObjectString;             
            }
            return ParsedJSONObjects;
        }
        private static void ParseStringsOfEachJSONObject(ParsedJSONObject [] JSONObjects)
        {           
            for (var i = 0; i < JSONObjects.Length; i++)
            {
                string[] stringsOfObject = JSONObjects[i].StringsToParse;
                for (var j = 0; j < stringsOfObject.Length; j++)
                {
                    var spliter = stringsOfObject[j].IndexOf(':');
                    if (spliter!=-1)
                    {
                        var type = stringsOfObject[j].Substring(0, spliter).Trim('"');
                        var body = stringsOfObject[j].Substring(spliter + 2).Trim('"');
                        if (type == "FirstName")
                        {
                            JSONObjects[i].FirstName = body;
                        }
                        if (type == "LastName")
                        {
                            JSONObjects[i].LastName = body;
                        }
                    }
                    if (spliter == -1)
                    {
                        JSONObjects[i].FirstName = "FirstName";
                        JSONObjects[i].LastName = "LastName";
                    }
                }
            }
        }
    }
    class ParsedJSONObject
    {
        string _FirstName;
        string _LastName;
        string [] _StringsToParse;
        public ParsedJSONObject(int countStrings)
        {
            _FirstName = "";
            _LastName = "";
            _StringsToParse = new string[countStrings];
        }
        public string FirstName
        {
            get => _FirstName;
            set => _FirstName = value;
        }
        public string LastName
        {
            get => _LastName;
            set => _LastName = value;
        }
        public string [] StringsToParse
        {
            get => _StringsToParse;
            set => _StringsToParse = value;
        }
    }   
}
