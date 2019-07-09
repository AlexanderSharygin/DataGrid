using System;

namespace Parser
{      
    class Program
    {
        static void Main(string[] args)
        {
            var inputText = JSONParser.GetTextToParse(AppDomain.CurrentDomain.BaseDirectory + "data.txt");
            JSONParser.ParseSimpleJSON(inputText);
        }
    }
  
  }
