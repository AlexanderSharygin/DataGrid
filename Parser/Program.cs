using System;
using System.IO;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            IOWork.GetSource();
            Parser Parsing = new Parser(IOWork.GetSource());
            Parsing.getObjects();
            Parsing.parseObjectstoStrings();
            Parsing.parseStringsOfObject();
            IOWork.SetResult(Parsing.ParsResult);                         
        }
    }
    // класс для работы с консолью (такой вот нонче фронтенд)
    static class IOWork
    {
        public static string GetSource()
        {
            // StreamReader FileReader = new StreamReader();
            // return FileReader.ReadToEnd();
            return  Properties.Resources.data;
        }
        public static void SetResult(ParsedObject[] obj)
        {
            var rowCounter = 0;
            var columnShift = 5;
            var FirstNameMaxLength = 9;
            var LastNameMaxLength = 9;
            for (var i = 0; i < obj.Length; i++)
            {
                var FirstNameLength = obj[i].FirstName.Length;
                if (FirstNameLength > FirstNameMaxLength)
                {
                    FirstNameMaxLength = FirstNameLength;
                }
                var LastNameLength = obj[i].LastName.Length;
                if (LastNameLength > LastNameMaxLength)
                {
                    {
                        LastNameMaxLength = LastNameLength;
                    }
                }
            }           
            Console.SetCursorPosition(0,rowCounter);
            Console.Write("FirstName");
            Console.SetCursorPosition(FirstNameMaxLength + columnShift,rowCounter);
            Console.Write("LastName");
            rowCounter++;
            Console.SetCursorPosition(0, rowCounter);
            for (var i = 0; i < FirstNameMaxLength + LastNameMaxLength + columnShift; i++)
            {
                Console.Write('-');
            }
            rowCounter++;
            Console.SetCursorPosition(0, rowCounter); ;
            for (var i = 0; i < obj.Length; i++)
            {
               
                Console.SetCursorPosition(0, rowCounter);
                Console.Write(obj[i].FirstName);
                Console.SetCursorPosition(FirstNameMaxLength + columnShift, rowCounter);
                Console.Write(obj[i].LastName);
                rowCounter++;
            }
            Console.ReadLine();
        }
    }
    //класс парсинга
    class Parser
    {
        public Parser(string sourceToParse)
        {
            _SourсeToParse = sourceToParse;
            _ObjectsToParse = new string[0];
            _ParsResults = new ParsedObject[0];
        }
        //Распарсеные данные в виде объектов
        private ParsedObject[] _ParsResults;
        public ParsedObject[] ParsResult
        {
            get
            {
                return _ParsResults;
            }
        }
        private string _SourсeToParse;
        //побитый на объекты JSON
        private string [] _ObjectsToParse;
        private int _CountOfObectsToParse;      
        //Разобъём весь JSON на отдельные объекты 
        public void getObjects()
        {
            var source = _SourсeToParse;
            var counter = 0;
            var character = ' ';
            var objectStartPositon = 0;
            var objectEndPosition = 0;
            while (counter <= source.Length - 1)
            {
                character = source[counter];
                //начало объекта - символ {
                if (character == '{')
                {
                    objectStartPositon = counter;
                    for (var i = counter; i < source.Length - 1; i++)
                    {
                        //если встречаем ковычки - ищём где они закрываются и пропускаем этот отрезок
                        character = source[i];
                        if (character == '"')
                        {
                            counter = ++i;
                            for (var j = counter; j < source.Length - 1; j++)
                            {
                                character = source[j];
                                if (character == '"')
                                {
                                    counter = j;
                                    counter++;
                                    i = counter;
                                    break;
                                }
                                else
                                {
                                    counter++;
                                }
                            }
                        }
                        // как только находим } не в ковычках - выделили объект
                        // выпиливаем его из общего текста и кидаем в массив 
                        if (character == '}')
                        {
                            objectEndPosition = i;
                            _CountOfObectsToParse++;
                            counter = i;
                            counter++;
                            Array.Resize(ref _ObjectsToParse, _ObjectsToParse.Length + 1);
                            _ObjectsToParse[_CountOfObectsToParse - 1] = source.Substring(objectStartPositon, objectEndPosition - objectStartPositon + 1);
                            break;
                        }
                    }
                }
                else
                {
                    counter++;
                }
            }
        }
        // распарсим каждый объектна отдельые строки в которых есть нужные нам данные
        public void parseObjectstoStrings()
        {
            // на этом этапе уже будем записывать сроки в готовые объекты
            
            for (int k = 0; k < _ObjectsToParse.Length; k++)
            {
                string[] result = new string[0];
                var countOfString= 0;
                var counter = 0;
                var stringStartPosition = 0;
                var stringEndPosition = 0;
                string temp = _ObjectsToParse[k];
                while (counter < temp.Length)
                {
                    //находим начало строки и ждём конец строки если он не в ковычках
                    if ((temp[counter] == '\r' && temp[counter + 1] == '\n')|| (temp[counter] == '\n') || (temp[counter] == '\r'))
                    {
                        stringStartPosition = counter;
                        for (int i = ++counter; i < temp.Length - 1; i++)
                        {
                            // если ковычки - пропускаем этот текст
                            if (temp[i] == '"')
                            {
                                counter = ++i;
                                for (int j = counter; j < temp.Length - 1; j++)
                                {
                                    if (temp[j] == '"')
                                    {
                                        counter = j;
                                        i = counter;
                                        break;
                                    }
                                    else
                                    {
                                        counter++;
                                    }
                                }
                            }
                            // находим конец строки и выпиливаем строку
                            if ((temp[counter] == '\r' && temp[counter + 1] == '\n')|| (temp[counter] == '\r' && temp[counter - 1] != '\n') || (temp[counter] == '\n' && temp[counter - 1] != '\r'))
                            {
                                stringEndPosition = i;
                                counter = i;
                                counter = counter - 2;
                                Array.Resize(ref result, result.Length + 1);
                                // так как в последней строке нет запятой пеенастраиваем выпиливание
                                if (temp[counter - 1] == ',')
                                {
                                    result[countOfString] = temp.Substring(stringStartPosition, stringEndPosition - stringStartPosition - 1);
                                }
                                result[countOfString] = temp.Substring(stringStartPosition, stringEndPosition - stringStartPosition);
                                countOfString++;
                                break;
                            }
                            else
                            {
                                counter++;
                            }
                        }

                    }
                    else
                    {
                        counter++;
                    }
                }
                // приходится делать так, потому что какой то Швайн запретил использовать List
                Array.Resize(ref _ParsResults, _ParsResults.Length + 1);
                _ParsResults[k] = new ParsedObject(result.Length);
                _ParsResults[k].StringsToParse = result;               
            }          
        }
        // ну и наконей парсим строки и получаем FirstName и LastName
        public void parseStringsOfObject()
        {           
            for (var i = 0; i < _ParsResults.Length; i++)
            {
                string[] stringsOfObject = new string[_ParsResults[i].StringsToParse.Length];
                stringsOfObject = _ParsResults[i].StringsToParse;
                for (var j = 0; j < stringsOfObject.Length; j++)
                {
                    var a = stringsOfObject[j].IndexOf(':');
                    var type = stringsOfObject[j].Substring(0, a);
                    var body = stringsOfObject[j].Substring(a + 1);
                    var trimStarIndex = type.IndexOf('"');
                    var trimEndIndex = type.LastIndexOf('"');              
                    if (trimStarIndex != -1)
                    {
                         type = type.Substring(trimStarIndex + 1, trimEndIndex - trimStarIndex - 1);
                    }
                    else
                    {
                        type = "Неразспознано";
                    }
                    trimStarIndex = body.IndexOf('"');
                    trimEndIndex = body.LastIndexOf('"');                   
                    if (trimStarIndex != -1)
                    {
                        body = body.Substring(trimStarIndex + 1, trimEndIndex - trimStarIndex - 1);
                    }
                    else
                    {
                        body = "Неразспознано";
                    }
                    if (type == "FirstName")
                    {
                        _ParsResults[i].FirstName = body;
                    }
                    if (type == "LastName")
                    {
                        _ParsResults[i].LastName = body;
                    }
                }
            }
        }
    }
    // представим выводимые данные в виде объекта
    class ParsedObject
    {
        private string _FirstName;
        private string _LastName;
        private string [] _StringToParse;
        public ParsedObject(int countStrings)
        {
          _FirstName = "";
          _LastName = "";
          _StringToParse = new string[countStrings];
        }
        public string [] StringsToParse
        {
            get
            {
                return _StringToParse;
            }
            set
            {
                _StringToParse = value;
            }
        }
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
            }
        }
        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = value;
            }
        }
    }   
}
