using System;
using System.IO;

namespace Parser
{
    // nb.
    //renamed
    static class JSONParser
    {
        // nb. the method takes responsibility for something it is not aware of (the json format).
        //renamed
        public static string GetTextToParse(string pathToText)
        {
            StreamReader FileReader = new StreamReader(pathToText);
            return FileReader.ReadToEnd();
        }
        // nb.
        //renamed
        public static void ParseSimpleJSON(string inputText)
        {
            string[] JSONObjectsText = GetJSOONObjectsText(inputText);
            JSONObject[] JSONObjects = ParseJSONObjectsText(JSONObjectsText);
            ConsolePrinter Printer = new ConsolePrinter();
            Printer.PrintJSONOnConsole(JSONObjects);
        }
        // nb.
        //renamed
        private static string[] GetJSOONObjectsText(string inputText)
        {
            string[] JSONObjectsText = new string[0];
            var counter = 0;
            var objectNumber = 0;
            while (counter < inputText.Length)
            {
                var objectStartPositonInText = 0;
                var objectEndPositionInText = 0;
                if (inputText[counter] == '{')
                {
                    objectStartPositonInText = counter;
                    objectEndPositionInText = GetObjectEndPositionInText(inputText, counter);
                    // refactoring needed.
                    Array.Resize(ref JSONObjectsText, objectNumber + 1);
                    JSONObjectsText[objectNumber] = inputText.Substring(objectStartPositonInText, objectEndPositionInText - objectStartPositonInText + 1);
                    objectNumber++;
                    //
                    counter = objectEndPositionInText;
                }
                else
                {
                    counter++;
                }
            }
            return JSONObjectsText;
        }
        // nb. 'counter' should be more specific, whereas the former less.
        //renamed
        private static int GetObjectEndPositionInText(string simpleJSONText, int objectStartPosition)
        {
            int objectEndPosition = 0;
            for (var i = objectStartPosition; i < simpleJSONText.Length; i++)
            {
                if (simpleJSONText[i] == '"')
                {
                    i = GetClosingCommaPostion(simpleJSONText, ++i);
                }
                if (simpleJSONText[i] == '}')
                {
                    objectEndPosition = i;
                    break;
                }
            }
            return objectEndPosition;
        }
        private static int GetClosingCommaPostion(string simpleJSONText, int openCommaPosition)
        {
            int closingCommaPosition = openCommaPosition;
             
            for (var i = openCommaPosition; i < simpleJSONText.Length; i++)
            {
                 if (simpleJSONText[i] == '"')
                {
                    closingCommaPosition = i;
                    break;
                }
            }
            return closingCommaPosition+1;
        }
        private static int GetEndStringInJSOObject(string JSONObjectText, int startStringPosition)
        {
            int endStringPosition = startStringPosition;
            string temp = JSONObjectText.Substring(startStringPosition);
            for (int i = startStringPosition; i < JSONObjectText.Length ; i++)
            {
                if (JSONObjectText[i] == ',' || JSONObjectText[i] == '}')
                {
                    endStringPosition = i;
                    break;
                }
                if (JSONObjectText[i] == '"')
                {
                    i = GetClosingCommaPostion(JSONObjectText, ++i)-1;
                }
            }
            return endStringPosition;
        }
        private static JSONObject[] ParseJSONObjectsText(string[] JSONObjectsText)
        {
            JSONObject[] JSONObjects = new JSONObject[JSONObjectsText.Length];
            for (int i = 0; i < JSONObjectsText.Length; i++)
            {
                string[] JSONObjectStrings = new string[0];
                var countOfStringInJSONObject = 0;
                var counter = 0;
                var startStringPosition = 0;
                var endStringPosition = 0;
                string JSONObjectText = JSONObjectsText[i];
                while (counter < JSONObjectsText[i].Length)
                {
                    if (JSONObjectText[counter] == ',' || JSONObjectText[counter] == '{')
                    {
                        startStringPosition = counter;
                        endStringPosition = GetEndStringInJSOObject(JSONObjectText, counter + 1);
                        if (endStringPosition != 0)
                        {
                            Array.Resize(ref JSONObjectStrings, JSONObjectStrings.Length + 1);
                            JSONObjectStrings[countOfStringInJSONObject] = JSONObjectText.Substring(startStringPosition, endStringPosition - startStringPosition).Trim(',', '\n', '\r', ' ');
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
                JSONObjects[i] = new JSONObject();
                var objectNumber = i;
                ParseJSONObjectStrings(JSONObjectStrings, objectNumber, JSONObjects);
            }
            return JSONObjects;
        }
        private static void ParseJSONObjectStrings(string[] objectStrings, int objectNumber, JSONObject[] JSONObjects)
        {
            bool firstNameParsed = false;
            bool lastNameParsed = false;
            for (var i = 0; i < objectStrings.Length; i++)
            {                
                var spliter = objectStrings[i].IndexOf(':');
                if (spliter != -1)
                {
                    var type = objectStrings[i].Substring(0, spliter).Trim('{', '"', '\n', '\r', '\t', ' ');
                    var body = objectStrings[i].Substring(spliter + 2).Trim('{', '"', '\n', '\r', '\t', ' ');
                    if (type == "FirstName")
                    {
                        JSONObjects[objectNumber].FirstName = body;
                        firstNameParsed = true;
                    }
                    if (type == "LastName")
                    {
                        JSONObjects[objectNumber].LastName = body;
                        lastNameParsed = true;
                    }
                }
                if (firstNameParsed && lastNameParsed)
                {
                    firstNameParsed = false;
                    lastNameParsed = false;
                    break;
                }
                if (spliter == -1)
                {
                    JSONObjects[objectNumber].FirstName = "FirstName";
                    JSONObjects[objectNumber].LastName = "LastName";
                }
            }
        }
    }
}
