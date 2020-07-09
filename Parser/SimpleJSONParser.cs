using System.Collections.Generic;

namespace Parser
{
   public static class SimpleJSONParser
    {
        public static List<Dictionary<string,string>> ParseSimpleJSON(string inputText)
        {
            List<string> ObjectsText = SplitTextToObjects(inputText);
            return ParseObjectsText(ObjectsText);
        }
        private static List<string> SplitTextToObjects(string inputText)
        {
            List<string> ObjectsText = new List<string>();
            int counter = 0;        
            while (counter < inputText.Length)
            {
                int objectStartPositonInText = 0;
                int objectEndPositionInText = 0;
                if (inputText[counter] == '{')
                {
                    objectStartPositonInText = counter;
                    objectEndPositionInText = GetObjectEndPositionInText(inputText, counter);
                    ObjectsText.Add(inputText.Substring(objectStartPositonInText, objectEndPositionInText - objectStartPositonInText + 1));
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
            for (var counter = StartPosition; counter < JSONTObjectsText.Length; counter++)
            {
                if (JSONTObjectsText[counter] == '"')
                {
                   counter = GetClosingCommaPostion(JSONTObjectsText, ++counter);
                   
                }
                if (JSONTObjectsText[counter] == '}')
                {
                    objectEndPosition = counter;
                    break;
                }
            }
            return objectEndPosition;
        }
        private static int GetClosingCommaPostion(string simpleJSONText, int openCommaPosition)
        {
            return openCommaPosition + simpleJSONText.Substring(openCommaPosition).IndexOf('"')+1;                    
        }
        private static int GetEndFieldPositonInObject(string objectText, int startStringPosition)
        {
            var endStringPosition = startStringPosition;         
            for (int counter = startStringPosition; counter < objectText.Length; counter++)
            {
                if (objectText[counter] == ',' || objectText[counter] == '}')
                {
                    endStringPosition = counter;
                    break;
                }
                if (objectText[counter] == '"')
                {
                    counter = GetClosingCommaPostion(objectText, ++counter) - 1;
                }
            }
            return endStringPosition;
        }
        private static List<Dictionary<string, string>> ParseObjectsText(List<string> ObjectsText)
        {
            List<Dictionary<string,string>> Objects = new List<Dictionary<string, string>>();
            foreach (var ObjectText in ObjectsText)
            {
                List<string> ObjectFieldsString = new List<string>(); 
                var counter = 0;
                var startFieldPosition=0;
                var endFieldPosition=0;              
                while (counter < ObjectText.Length)
                {
                    if (ObjectText[counter] == ',' || ObjectText[counter] == '{')
                    {
                        startFieldPosition = counter;
                        endFieldPosition = GetEndFieldPositonInObject(ObjectText, counter + 1);
                        if (endFieldPosition != 0)
                        {
                            ObjectFieldsString.Add(ObjectText.Substring(startFieldPosition, endFieldPosition - startFieldPosition).Trim(',', '\n', '\r', ' '));
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
            foreach (var objectFieldsString in objectFieldsStrings)
            {                
                var splitter = objectFieldsString.IndexOf(':');
                if (splitter != -1)
                {
                    var fieldKey = objectFieldsString.Substring(0, splitter).Trim('{', '"', '\n', '\r', '\t', ' ');
                    var fieldValue = objectFieldsString.Substring(splitter + 2).Trim('{', '"', '\n', '\r', '\t', ' ');
                    obj.Add(fieldKey, fieldValue);
                }              
            }
            return obj;
        }
    }
}

