using System;
using Parser.Extensions;

namespace Parser
{
    class ConsolePrinter: Printer
    {

        public void PrintJSONOnConsole(JSONObject[] JSOObjects)
        {
            PrintJSONObjectsPropertiesOnConsole(JSOObjects);
        }
    }
}
