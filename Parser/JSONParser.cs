using Parser.Extensions;


namespace Parser
{
    static class JSONParser
    {
        public static MyList<JSONObject> ParseSimpleJSON(string inputText)
        {
            MyList<string> ObjectsText = SplitTextToObjects(inputText);
            return ParseObjectsText(ObjectsText);
        }
        private static MyList<string> SplitTextToObjects(string inputText)
        {
            MyList<string> ObjectsText = new MyList<string>();
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
        private static MyList<JSONObject> ParseObjectsText(MyList<string> ObjectsText)
        {
            MyList<JSONObject> Objects = new MyList<JSONObject>();
            for (int i = 0; i < ObjectsText.Count; i++)
            {
                MyList<string> ObjectFieldsString = new MyList<string>();
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
        private static JSONObject ParseFieldsString(MyList<string> objectFieldsStrings)
        {
            JSONObject obj = new JSONObject();
            for (var i = 0; i < objectFieldsStrings.Count; i++)
            {
                
                var splitter = objectFieldsStrings[i].IndexOf(':');
                if (splitter != -1)
                {
                    var fieldKey = objectFieldsStrings[i].Substring(0, splitter).Trim('{', '"', '\n', '\r', '\t', ' ');
                    var fieldValue = objectFieldsStrings[i].Substring(splitter + 2).Trim('{', '"', '\n', '\r', '\t', ' ');
                    obj.Fields.Add(fieldKey, fieldValue);
                }
              
            }
            obj.MapObjectFields();
            return obj;
        }
    }
}

