using System;
using System.IO;

namespace Parser
{      
    class Program
    {
        static void Main(string[] args)
        {
           
            var inputText = File.ReadAllText("data.txt");
            JSONObject [] JSONObjects = JSONParser.ParseSimpleJSON(inputText);
            ConsolePrinter Printer = new ConsolePrinter();
            Printer.PrintJSONOnConsole(JSONObjects);
        }
    }
  
  }
