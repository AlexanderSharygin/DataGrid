using Parser.Extensions;


namespace Parser
{


    static class JSONParser
    {

        public static MyList<JSONObject> ParseSimpleJSON(string inputText)
        {
            MyList<string> ObjectsText = GetObjectsText(inputText);
            return ParseObjectsText(ObjectsText);

        }

        private static MyList<string> GetObjectsText(string inputText)
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
        private static MyList<JSONObject> ParseObjectsText(MyList<string> ObjectsText)
        {
            //  JSONObject[] Objects = new JSONObject[ObjectsText.Count];
            MyList<JSONObject> Objects = new MyList<JSONObject>();
            for (int i = 0; i < ObjectsText.Count; i++)
            {
                MyList<string> ObjectStrings = new MyList<string>();
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
                            ObjectStrings.Add(ObjectText.Substring(startStringPosition, endStringPosition - startStringPosition).Trim(',', '\n', '\r', ' '));
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
                Objects.Add(new JSONObject());
                var objectNumber = i;
                ParseObjectStrings(ObjectStrings, objectNumber, Objects);
            }
            return Objects;
        }
        private static void ParseObjectStrings(MyList<string> objectStrings, int objectNumber, MyList<JSONObject> Objects)
        {
            for (var i = 0; i < objectStrings.Count; i++)
            {
                var splitter = objectStrings[i].IndexOf(':');
                if (splitter != -1)
                {
                    var fieldKey = objectStrings[i].Substring(0, splitter).Trim('{', '"', '\n', '\r', '\t', ' ');
                    var fieldValue = objectStrings[i].Substring(splitter + 2).Trim('{', '"', '\n', '\r', '\t', ' ');
                    Objects[objectNumber].Fields.Add(fieldKey, fieldValue);
                   if (splitter == -1)
                    {

                        Objects[objectNumber].Fields.Add(("Error key " + i), ("Error value" + i));
                    }
                }
            }
        }
    }
}

