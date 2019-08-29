using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Extensions;

namespace Parser
{
    // use Properties.Resources instead.
    static class Constants
    {
        // bad name. use word 'treshold'
        public const int MinLengthMyListThenTrimExcessObjects = 100000;
        public const int IntercolumnShift = 5;
        // undefinedFieldText
        public const string TextForUndefinedField = "Undefined field";
        // toLong?
        public const int FieldToLongСoefficient = 3;
        // ellipsis
        public const string CuttingStringForTooLongField = "...";
        public const string CheckedFieldOnUIPrefix = "[X] ";
        public const string UncheckedFieldOnUIPrefix = "[ ] ";
        public static readonly string [] PreambleStrings = { "Simple-JSON objects parser.", "UP/Down arrows - menu navigation.", "Space - Adding a field to the ones displayed in the table."};
        // bad design. The buffer width depends on OS settings. You should not consider it constant. Otherwise, it may cause unexpected behavior on different machines.
        public static int MinWidthConsoleBufer = Console.BufferWidth;
        // tableMargin
        public const int CountEmptyStringsBetweenMenuAndItable = 1;
    }
}
