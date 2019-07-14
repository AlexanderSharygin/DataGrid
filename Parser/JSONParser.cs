using System;
using System.IO;
using Parser.Extensions;
using System.Collections.Generic;

namespace Parser
{


    static class JSONParser
    {

        public static JSONObject[] ParseSimpleJSON(string inputText)
        {
            OwnList<string> ObjectsText = GetObjectsText(inputText);
            return ParseObjectsText(ObjectsText);

        }

        private static OwnList<string> GetObjectsText(string inputText)
        {
            OwnList<string> ObjectsText = new OwnList<string>();

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
                    ObjectsText.AddLast(inputText.Substring(objectStartPositonInText, objectEndPositionInText - objectStartPositonInText + 1));

                    objectNumber++;
                    counter = objectEndPositionInText;
                }
                else
                {
                    counter++;
                }
            }
            return ObjectsText;
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
            return closingCommaPosition + 1;
        }
        private static int GetEndStringInObject(string ObjectText, int startStringPosition)
        {
            int endStringPosition = startStringPosition;
            string temp = ObjectText.Substring(startStringPosition);
            for (int i = startStringPosition; i < ObjectText.Length; i++)
            {
                if (ObjectText[i] == ',' || ObjectText[i] == '}')
                {
                    endStringPosition = i;
                    break;
                }
                if (ObjectText[i] == '"')
                {
                    i = GetClosingCommaPostion(ObjectText, ++i) - 1;
                }
            }
            return endStringPosition;
        }
        private static JSONObject[] ParseObjectsText(OwnList<string> ObjectsText)
        {
            JSONObject[] Objects = new JSONObject[ObjectsText.Length];
            for (int i = 0; i < ObjectsText.Length; i++)
            {
                OwnList<string> ObjectStrings = new OwnList<string>();
                var countOfStringInObject = 0;
                var counter = 0;
                var startStringPosition = 0;
                var endStringPosition = 0;
                string ObjectText = ObjectsText[i];
                while (counter < ObjectsText[i].Length)
                {
                    if (ObjectText[counter] == ',' || ObjectText[counter] == '{')
                    {
                        startStringPosition = counter;
                        endStringPosition = GetEndStringInObject(ObjectText, counter + 1);
                        if (endStringPosition != 0)
                        {
                            ObjectStrings.AddLast(ObjectText.Substring(startStringPosition, endStringPosition - startStringPosition).Trim(',', '\n', '\r', ' '));
                            countOfStringInObject++;
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
                Objects[i] = new JSONObject();
                var objectNumber = i;
                ParseObjectStrings(ObjectStrings, objectNumber, Objects);
            }
            return Objects;
        }
        private static void ParseObjectStrings(OwnList<string> objectStrings, int objectNumber, JSONObject[] Objects)
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
                        Objects[objectNumber].FirstName = body;
                        firstNameParsed = true;
                    }
                    if (type == "LastName")
                    {
                        Objects[objectNumber].LastName = body;
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
                    Objects[objectNumber].FirstName = "FirstName";
                    Objects[objectNumber].LastName = "LastName";
                }
            }
        }
    }
}

