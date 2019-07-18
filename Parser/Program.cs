using System.IO;
using Parser.Extensions;


namespace Parser
{      
    class Program
    {
        static void Main(string[] args)
        {
            var inputText = File.ReadAllText("data.txt");
            MyList<JSONObject> JSONObjects = JSONParser.ParseSimpleJSON(inputText);                  
            ConsolePrinter Printer = new ConsolePrinter();
            Printer.PrintJSONOnConsole(JSONObjects);
        }
    }
  
  }
