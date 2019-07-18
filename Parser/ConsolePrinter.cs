using System;
using Parser.Extensions;

namespace Parser
{
    class ConsolePrinter: Printer
    {

        public void PrintJSONOnConsole(MyList<JSONObject> JSOObjects)
        {
            PrintJSONObjectsPropertiesOnConsole(JSOObjects);
        }
    }
}
