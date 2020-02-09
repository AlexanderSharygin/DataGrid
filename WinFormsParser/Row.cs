using Parser.Properties;
using System;
using System.Collections.Generic;


namespace Parser
{
    class Row
    {
        public List<Cell> Cells { get; set; } = new List<Cell>();

    }
    class RowComparer : IComparer<Row>
    {
        bool _Direction;
        int _ColumnIndex;
        Type _Type;
        public RowComparer(bool direction, int index, Type t)
        {
            _Direction = direction;
            _ColumnIndex = index;
            _Type = t;
        }
        public int Compare(Row x, Row y)
        {
            object xValue = null;
            object yValue = null;
            if (x.Cells[_ColumnIndex].Body != Resources.UndefinedFieldText)
            {
                try
                {
                    xValue = Convert.ChangeType(x.Cells[_ColumnIndex].Body, _Type);
                }
                catch
                {
                    xValue = null;
                }
            }
            if (y.Cells[_ColumnIndex].Body != Resources.UndefinedFieldText)
            {
                try
                {
                    yValue = Convert.ChangeType(y.Cells[_ColumnIndex].Body, _Type);
                }
                catch
                {
                    yValue = null;
                }
            }

            int dir = (_Direction) ? 1 : -1;

            if (xValue != null && xValue is IComparable)
            {

                return (xValue as IComparable).CompareTo(yValue) * ((_Direction) ? 1 : -1);
            }
            if (yValue != null && xValue is IComparable)
            {
                return (yValue as IComparable).CompareTo(xValue) * ((_Direction) ? 1 : -1);
            }
            else
            {
                return (x.Cells[_ColumnIndex].Body as string).CompareTo(y.Cells[_ColumnIndex].Body as string) * ((_Direction) ? 1 : -1);
            }

        }
    }
}
