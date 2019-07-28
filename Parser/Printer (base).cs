using System;
using Parser.Extensions;

namespace Parser
{
    class Printer
    {            
       static int GetFieldValueLength(JSONObject p_item, string key)
        {
            if (p_item.Fields.KeyIndexOf(key) == -1)
            {
                return "Поле не найдено".Length;
            }
            else
            {
                return p_item.Fields[key].Length;
            }
        }       
        public static int MaxColuntLength(MyList<JSONObject> p_Objects, string p_key)
        {
            var maxColumnLength = p_Objects.MaxValue(p_key, GetFieldValueLength);
            var currentColumnLength = p_key.Length;
            if (maxColumnLength > currentColumnLength)
            {
                if (IsVeryLengthFieldForColumn(maxColumnLength, currentColumnLength))
                {
                    currentColumnLength = currentColumnLength * 3 + 3;
                }
                else
                {
                    currentColumnLength = maxColumnLength;
                }
            }
            return currentColumnLength;
        }
        public static bool IsVeryLengthFieldForColumn(int p_maxFieldLength, int p_columnLength)
        {
            if (p_maxFieldLength > 3 * p_columnLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
      
        public static bool IsInputKeyValid(int keyNumber, int maxKeyValue)
        {
            if (keyNumber <= maxKeyValue && keyNumber >= 0)
            {
                return true;
            }
           else
            {
                return false;
            }
        }    
    }
}
