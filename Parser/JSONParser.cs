using Parser.Extensions;
using System.Collections.Generic;

namespace Parser
{
    static class JSONParser
    {
        public static List<Dictionary<string,string>> ParseSimpleJSON(string inputText)
        {
            List<string> ObjectsText = SplitTextToObjects(inputText);
            return ParseObjectsText(ObjectsText);
        }
        private static List<string> SplitTextToObjects(string inputText)
        {
            List<string> ObjectsText = new List<string>();
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
                    ObjectsText.Add(inputText.Substring(objectStartPositonInText, objectEndPositionInText - objectStartPositonInText + 1));
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
        private static int GetObjectEndPositionInText(string JSONTObjectsText, int StartPosition)
        {
            int objectEndPosition = 0;
            for (var i = StartPosition; i < JSONTObjectsText.Length; i++)
            {
                if (JSONTObjectsText[i] == '"')
                {
                    i = GetClosingCommaPostion(JSONTObjectsText, ++i);
                }
                if (JSONTObjectsText[i] == '}')
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
        private static int GetEndFieldPositonInObject(string objectText, int startStringPosition)
        {
            int endStringPosition = startStringPosition;
            string temp = objectText.Substring(startStringPosition);
            for (int i = startStringPosition; i < objectText.Length; i++)
            {
                if (objectText[i] == ',' || objectText[i] == '}')
                {
                    endStringPosition = i;
                    break;
                }
                if (objectText[i] == '"')
                {
                    i = GetClosingCommaPostion(objectText, ++i) - 1;
                }
            }
            return endStringPosition;
        }
        private static List<Dictionary<string, string>> ParseObjectsText(List<string> ObjectsText)
        {
            List<Dictionary<string,string>> Objects = new List<Dictionary<string, string>>();
            for (int i = 0; i < ObjectsText.Count; i++)
            {
                List<string> ObjectFieldsString = new List<string>();
                var countOfFieldsInObject = 0;
                var counter = 0;
                var startFieldPosition=0;
                var endFieldPosition=0;
                string ObjectText = ObjectsText[i];
                while (counter < ObjectsText[i].Length)
                {
                    if (ObjectText[counter] == ',' || ObjectText[counter] == '{')
                    {
                        startFieldPosition = counter;
                        endFieldPosition = GetEndFieldPositonInObject(ObjectText, counter + 1);
                        if (endFieldPosition != 0)
                        {
                            ObjectFieldsString.Add(ObjectText.Substring(startFieldPosition, endFieldPosition - startFieldPosition).Trim(',', '\n', '\r', ' '));
                            countOfFieldsInObject++;
                            counter = endFieldPosition;
                        }
                        if (endFieldPosition == 0)
                        {
                            counter++;
                        }
                    }
                    else
                    {
                        counter++;
                    }
                }
                Objects.Add(ParseFieldsString(ObjectFieldsString));
              
            }
            return Objects;
        }
        private static Dictionary<string,string> ParseFieldsString(List<string> objectFieldsStrings)
        {
            Dictionary<string, string> obj = new Dictionary<string, string>();
            for (var i = 0; i < objectFieldsStrings.Count; i++)
            {
                
                var splitter = objectFieldsStrings[i].IndexOf(':');
                if (splitter != -1)
                {
                    var fieldKey = objectFieldsStrings[i].Substring(0, splitter).Trim('{', '"', '\n', '\r', '\t', ' ');
                    var fieldValue = objectFieldsStrings[i].Substring(splitter + 2).Trim('{', '"', '\n', '\r', '\t', ' ');
                    obj.Add(fieldKey, fieldValue);
                }
              
            }
            return obj;
        }
    }
}

