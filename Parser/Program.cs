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


     
            var inputText = File.ReadAllText("Files\\Data.txt");
            MyList <JSONObject> JSONObjects = JSONParser.ParseSimpleJSON(inputText);
            ConsoleRender m = new ConsoleRender(JSONObjects);
            m.RenderUI();           
           
        }
       
}

}
