using System.IO;
using System.Collections.Generic;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {     
         var inputText = File.ReadAllText("Files\\Data.txt");
         List <Dictionary<string,string>> JSONObjects = JSONParser.ParseSimpleJSON(inputText);
         ConsoleRender m = new ConsoleRender(JSONObjects);
         m.RenderUI();           
           
        }
       
}

}
