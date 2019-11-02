using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Trash
{
    class Class1
    {
        private void MyDataGrid_Scroll_1(object sender, ScrollEventArgs e)
        {


            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (e.Type == ScrollEventType.ThumbTrack)
                {
                    int k = (e.NewValue - value) / VerticalScroll.SmallChange;
                    if (k >= 1)
                    {
                        e.NewValue = value + VerticalScroll.SmallChange;
                        value = e.NewValue;
                        VerticalScroll.Value = value;

                        for (int i = 1; i < _Bufer.Count; i++)
                        {
                            if (i == Counter && !isCounterIncremented)
                            {
                                foreach (var Cell in _Bufer[Counter])
                                {
                                    Cell.YPosition = i * -100;

                                }
                                Counter++;
                                isCounterIncremented = true;

                            }
                            else
                            {
                                foreach (var Cell in _Bufer[i])
                                {
                                    Cell.YPosition -= RowHeight + 2;
                                }
                            }

                        }

                        isCounterIncremented = false;

                    }
                    if (k == -1)
                    {
                        Counter--;
                        e.NewValue = value - VerticalScroll.SmallChange;
                        value = e.NewValue;
                        for (int i = Counter; i < _Bufer.Count; i++)
                        {
                            if (i == Counter && isCounterDecremented == false)
                            {
                                foreach (var Cell in _Bufer[i])

                                {
                                    Cell.YPosition = _Bufer[0][0].YPosition + RowHeight + 2;

                                }
                                Counter--;
                                isCounterDecremented = true;
                            }
                            else
                            {
                                foreach (var Cell in _Bufer[i])

                                {
                                    Cell.YPosition += RowHeight + 2;
                                }
                            }
                        }
                        Counter++;

                        isCounterDecremented = false;
                    }

                    Invalidate();

                }
                if (e.Type == ScrollEventType.ThumbPosition)
                {


                }
                if (e.Type == ScrollEventType.SmallIncrement)
                {

                    if (VerticalScroll.Value < VerticalScroll.Maximum - Height)
                    {
                        for (int i = 1; i < _Bufer.Count; i++)
                        {
                            if (i == Counter && !isCounterIncremented)
                            {
                                foreach (var Cell in _Bufer[Counter])
                                {
                                    Cell.YPosition = i * -100;

                                }
                                Counter++;
                                isCounterIncremented = true;

                            }
                            else
                            {
                                foreach (var Cell in _Bufer[i])
                                {
                                    Cell.YPosition -= RowHeight + 2;
                                }
                            }

                        }

                        isCounterIncremented = false;
                    }
                    Invalidate();

                }
                if (e.Type == ScrollEventType.SmallDecrement)
                {

                    if (VerticalScroll.Value >= 0 && Counter > 1)
                    {

                        Counter--;

                        for (int i = Counter; i < _Bufer.Count; i++)
                        {
                            if (i == Counter && isCounterDecremented == false)
                            {
                                foreach (var Cell in _Bufer[i])

                                {
                                    Cell.YPosition = _Bufer[0][0].YPosition + RowHeight + 2;

                                }
                                Counter--;
                                isCounterDecremented = true;
                            }
                            else
                            {
                                foreach (var Cell in _Bufer[i])

                                {
                                    Cell.YPosition += RowHeight + 2;
                                }
                            }
                        }
                        Counter++;

                        isCounterDecremented = false;
                    }
                    Invalidate();

                }

            }
        }
    }
}
