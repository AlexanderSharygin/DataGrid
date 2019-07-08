using System;
using System.IO;
using System.Linq;
using Parser.Extensions;

namespace Parser
{
    // excessive api
    public static class StringExtensionToSimpleJSONParsing
    {
        // named badly (nb). this method sounds like it returns something. this applies to the method below, too.
        public static void ParseSimpleJSON(this String str)
        {
            SimpleJSONParser.ParseSimpleJSON(str);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            OutpuAndInputDataLoader.GetTextOfSimpleJSON().ParseSimpleJSON();
        }
    }
    // named badly (nb). there is no clear understanding of what task this class solves.
    static class OutpuAndInputDataLoader
    {
        // nb. the method takes responsibility for something it is not aware of (the json format).
        public static string GetTextOfSimpleJSON()
        {
            StreamReader FileReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "data.txt");
            return FileReader.ReadToEnd();
        }
        // nb. this name doesn't reflect what method actually does.
        static int selectorFirstName(ParsedJSONObject item)
            { return item.FirstName.Length; }
        static int selectorLastName(ParsedJSONObject item) { return item.LastName.Length; }
        public static void ShowResult(ParsedJSONObject[] parsedObjects)
        {
            var rowCounter = 0;
            var columnShift = 5; // what does this constant mean?
                                 // inconsistently named variables. use code style tools to avoid mistakes.
           int a = MyLinq.MaxValue(parsedObjects, selectorFirstName);
           int b = parsedObjects.MaxValue(selectorFirstName);
            int c = parsedObjects.MaxValue(n => n.FirstName.Length);
            int d = parsedObjects.MaxValue(n => {
                int f = n.FirstName.Length;
                return f;
            });
            var FirstNameMaxLength = "FirstName".Length;
            var LastNameMaxLength = "LastName".Length;
            // refactoring needed. this is a common task that performs some operation over list items.
            for (var i = 0; i < parsedObjects.Length; i++)
            {
                var FirstNameLength = parsedObjects[i].FirstName.Length;
                if (FirstNameLength > FirstNameMaxLength)
                {
                    FirstNameMaxLength = FirstNameLength;
                }
                var LastNameLength = parsedObjects[i].LastName.Length;
                if (LastNameLength > LastNameMaxLength)
                {
                    {
                        LastNameMaxLength = LastNameLength;
                    }
                }
            }
            //
            Console.SetCursorPosition(0, rowCounter);
            Console.Write("FirstName");
            Console.SetCursorPosition(FirstNameMaxLength + columnShift, rowCounter);
            Console.Write("LastName");
            rowCounter++;
            Console.SetCursorPosition(0, rowCounter);
            for (var i = 0; i < FirstNameMaxLength + LastNameMaxLength + columnShift; i++)
            {
                Console.Write('-');
            }
            rowCounter++;
            Console.SetCursorPosition(0, rowCounter); ;
            for (var i = 0; i < parsedObjects.Length; i++)
            {
                Console.SetCursorPosition(0, rowCounter);
                Console.Write(parsedObjects[i].FirstName);
                Console.SetCursorPosition(FirstNameMaxLength + columnShift, rowCounter);
                Console.Write(parsedObjects[i].LastName);
                rowCounter++;
            }
            Console.ReadLine();
        }
    }
    // nb.
    static class SimpleJSONParser
    {
        // nb.
        public static void ParseSimpleJSON(string simpleJSONFileText)
        {
            string[] TextOfJSONObjects = GetTextOfJSOONObjects(simpleJSONFileText);
            ParsedJSONObject[] SimpleJSONObjects = ParseTextOfJSONObjects(TextOfJSONObjects);
            OutpuAndInputDataLoader.ShowResult(SimpleJSONObjects);
        }
        // nb.
        private static string[] GetTextOfJSOONObjects(string simpleJSONText)
        {
            string[] textOfJSONObjects = new string[0];
            var counter = 0;
            var objectStartPositonInText = 0;
            var objectEndPositionInText = 0;
            var objectNumber = 0;
            while (counter < simpleJSONText.Length)
            {
                if (simpleJSONText[counter] == '{')
                {
                    objectStartPositonInText = counter;
                    objectEndPositionInText = GetObjectEndPositionInText(simpleJSONText, counter);
                    // refactoring needed.
                    Array.Resize(ref textOfJSONObjects, objectNumber + 1);
                    textOfJSONObjects[objectNumber] = simpleJSONText.Substring(objectStartPositonInText, objectEndPositionInText - objectStartPositonInText + 1);
                    objectNumber++;
                    //
                    counter = objectEndPositionInText;
                }
                else
                {
                    counter++;
                }
            }
            return textOfJSONObjects;
        }
        // nb. 'counter' should be more specific, whereas the former less.
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
                if ((simpleJSONText[counter] == '\r' && simpleJSONText[counter + 1] == '\n')
                    || (simpleJSONText[counter] == '\r' && simpleJSONText[counter + 1] != '\n')
                    || (simpleJSONText[counter] == '\n' && simpleJSONText[counter - 1] != '\r'))
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
        private static ParsedJSONObject[] ParseTextOfJSONObjects(string[] textOfJSONObjects)
        {
            ParsedJSONObject[] JSONObjects = new ParsedJSONObject[textOfJSONObjects.Length];
            for (int i = 0; i < textOfJSONObjects.Length; i++)
            {
                string[] TextStringsOfJSONObject = new string[0];
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
                            Array.Resize(ref TextStringsOfJSONObject, TextStringsOfJSONObject.Length + 1);
                            TextStringsOfJSONObject[countOfStringInJSONObject] = textOfJSONObject.Substring(startStringPosition, endStringPosition - startStringPosition).Trim(',', '\n', '\r', ' ');
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
                JSONObjects[i] = new ParsedJSONObject();
                ParseStringsOfThisJSONObject(TextStringsOfJSONObject, i, JSONObjects);
            }
            return JSONObjects;
        }
        private static void ParseStringsOfThisJSONObject(string[] stringsOfObject, int number, ParsedJSONObject[] JSONObjects)
        {
            for (var j = 0; j < stringsOfObject.Length; j++)
            {
                var spliter = stringsOfObject[j].IndexOf(':');
                if (spliter != -1)
                {
                    var type = stringsOfObject[j].Substring(0, spliter).Trim('"');
                    var body = stringsOfObject[j].Substring(spliter + 2).Trim('"');
                    if (type == "FirstName")
                    {
                        JSONObjects[number].FirstName = body;
                    }
                    if (type == "LastName")
                    {
                        JSONObjects[number].LastName = body;
                    }
                }
                if (spliter == -1)
                {
                    JSONObjects[number].FirstName = "FirstName";
                    JSONObjects[number].LastName = "LastName";
                }
            }
        }
    }
    // declaration may be shorter.
    class ParsedJSONObject
    {
        string _FirstName;
        string _LastName;
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
    }
}
