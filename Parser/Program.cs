using System.IO;
using System.Collections.Generic;
using System;

namespace Parser
{
    static class ConsoleConfig
    {
        public static ConsoleColor SelectedFieldColor
        { get; set; } = ConsoleColor.Blue;
        public static void StarupConfig()
        {
            Console.Clear();
            Console.SetWindowSize(120, 35);
            Console.SetBufferSize(120, 35);
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);

        }
    }
    class Program
    {
        
        static void Main(string[] args)
        {

         var inputText = File.ReadAllText("Files\\Data.txt");
         List <Dictionary<string,string>> JSONObjects = SimpleJSONParser.ParseSimpleJSON(inputText);
         ConsoleConfig.StarupConfig();
         ConsoleRender m = new ConsoleRender(JSONObjects);
         m.RenderUI();  
                    
        }
       
}

}
