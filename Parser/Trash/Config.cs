using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Parser.Extensions;

namespace Parser
{
    // use Properties.Resources instead.
    static class Config
    {
       public const int ColumnInterval = 5;
       public const string UndefinedFieldText = "Undefined field";
       public const int ReductionRatio = 3;
       public const string Ellipsis = "...";
       public const string CheckedMenuItemPrefix = "[X] ";
       public const string UncheckedMenuItemPrefix = "[ ] ";
       public static readonly string [] PreambleStrings = { "Simple-JSON objects parser.", "UP/Down arrows - menu navigation.", "Space - Adding a field to the ones displayed in the table."};
       public const int TableMargin = 1;
    }
}
