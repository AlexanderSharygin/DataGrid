﻿using System;
using System.IO;
using Parser.Extensions;
using System.Collections.Generic;

namespace Parser
{
<<<<<<< HEAD
   
   static class JSONParser
    {
      
       public static JSONObject[] ParseSimpleJSON(string inputText)
=======
    // it's a good class. let's code a good API for it.
   static class JSONParser
    {
        // I assume that it shouldn't be public. In any case, it has to be removed, as .NET already provides a similar method. 
        // google: read text in c#
        public static string GetTextToParse(string pathToText)
>>>>>>> 9b98c4c4e4ad784d4e6be9c1510165baac25edf8
        {
            OwnList<string> JSONObjectsText = GetObjectsText(inputText);
            return ParseObjectsText(JSONObjectsText);
           
        }
<<<<<<< HEAD
        private static OwnList<string> GetObjectsText(string inputText)
        {
            OwnList<string> JSONObjectsText = new OwnList<string>();
=======
       // Should it be here? If so, should it print something?
       public static void ParseSimpleJSON(string inputText)
        {
            string[] JSONObjectsText = GetJSOONObjectsText(inputText);
            JSONObject[] JSONObjects = ParseJSONObjectsText(JSONObjectsText);
            ConsolePrinter Printer = new ConsolePrinter();
            Printer.PrintJSONOnConsole(JSONObjects);
        }
        // Stop repeating 'JSON' in every method since we are within the 'JSON' parser now. 
        // You likely won't lose anything if name it 'GetObjectsText', or more correct, 'GetObjectStrings' (not sure if 'Texts' possible)
        private static string[] GetJSOONObjectsText(string inputText)
        {
            string[] JSONObjectsText = new string[0];
>>>>>>> 9b98c4c4e4ad784d4e6be9c1510165baac25edf8
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
                    JSONObjectsText.AddLast(inputText.Substring(objectStartPositonInText, objectEndPositionInText - objectStartPositonInText + 1));

                    objectNumber++;
                    counter = objectEndPositionInText;
                }
                else
                {
                    counter++;
                }
            }
            return JSONObjectsText;
        }
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
        private static int GetEndStringInObject(string JSONObjectText, int startStringPosition)
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
        private static JSONObject[] ParseObjectsText(OwnList<string> JSONObjectsText)
        {
            JSONObject[] JSONObjects = new JSONObject[JSONObjectsText.Length];
            for (int i = 0; i < JSONObjectsText.Length; i++)
            {
                OwnList<string> JSONObjectStrings = new OwnList<string>();
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
                        endStringPosition = GetEndStringInObject(JSONObjectText, counter + 1);
                        if (endStringPosition != 0)
                        {
                            JSONObjectStrings.AddLast(JSONObjectText.Substring(startStringPosition, endStringPosition - startStringPosition).Trim(',', '\n', '\r', ' '));
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
                ParseObjectStrings(JSONObjectStrings, objectNumber, JSONObjects);
            }
            return JSONObjects;
        }
        private static void ParseObjectStrings(OwnList<string> objectStrings, int objectNumber, JSONObject[] JSONObjects)
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
