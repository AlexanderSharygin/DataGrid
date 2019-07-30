﻿using System.IO;
using System;
using Parser.Extensions;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {


            // No, you didn't read the trello card and didn't follow the suggested approach.
            // Read about System.IO.Path
            string inputTextFilePath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\", "\\Files\\");
           var inputText = File.ReadAllText(inputTextFilePath + "data.txt");
            MyList <JSONObject> JSONObjects = JSONParser.ParseSimpleJSON(inputText);
            ConsolePrinter Printer = new ConsolePrinter();
            Printer.PrintJSONObjectsPropertiesOnConsole(JSONObjects);

        }
    }

}
