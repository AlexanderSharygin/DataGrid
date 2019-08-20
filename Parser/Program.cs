using System.IO;
using System;
using Parser.Extensions;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Collections.Generic;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {


            // No, you didn't read the trello card and didn't follow the suggested approach.
            // Read about System.IO.Path



          //  string inputTextFilePath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\", "\\Files\\");                   
          //  var inputText = File.ReadAllText(inputTextFilePath + "data.txt");
            var inputText = File.ReadAllText("Files\\Data.txt");
            MyList <JSONObject> JSONObjects = JSONParser.ParseSimpleJSON(inputText);
            // ConsolePrinter p = new ConsolePrinter(JSONObjects);

            MenuSource m = new MenuSource(JSONObjects);
            m.Run();
           
           // p.PrintJSONObjectsAsTable();
        }
       
}

}
