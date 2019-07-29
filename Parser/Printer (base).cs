using System;
using Parser.Extensions;

namespace Parser
{
    // Printer means that it prints something. But it looks more like utility methods for a specific task.
    // Besides, it's probably an excessive abstraction.
    class Printer
    {            
       static int GetFieldValueLength(JSONObject p_item, string key)
        {
            if (p_item.Fields.KeyIndexOf(key) == -1)
            {
                // OK, I see that you care about code readability.
                // But what your programmer should think if he/she gets 15 when sends an incorrect key value? He or she might not have your code at all.
                // You should keep the function output consistent. If it returns Length of something, I will expect 0 or in worst case,  -1, but not fucking 15!
                return "Поле не найдено".Length;
            }
            else
            {
                return p_item.Fields[key].Length;
            }
        }       
        // what?
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
        // google translate 'слишком длинный'
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
        // IsValid sounds better. It's more consistent with existing validation API.
        // excessive abstraction. C# is powerful enough to express this logic in a single expression at the place where it should be applied
        // https://trello.com/c/aWrnk1oi/10-c-and-net-platform-features
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