using System.IO;
using System;
using Parser.Extensions;


namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputTextFilePath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\", "\\Files\\");
            var inputText = File.ReadAllText(inputTextFilePath + "data.txt");
            MyList <JSONObject> JSONObjects = JSONParser.ParseSimpleJSON(inputText);
            ConsolePrinter Printer = new ConsolePrinter();
            Printer.PrintJSONObjectsPropertiesOnConsole(JSONObjects);

        }
    }

}
