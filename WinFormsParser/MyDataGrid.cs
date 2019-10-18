using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser
{

    public partial class MyDataGrid : UserControl
    {
        List<List<string>> _Source = new List<List<string>>();
        List<List<Cell>> _Bufer = new List<List<Cell>>();
        int _ColumnHeight;
        int _LineWidth = 1;
        int _MaxX;
        int _MaxY;
        public MyDataGrid()
        {
            InitializeComponent();
            ResizeRedraw = true;
            components = new System.ComponentModel.Container();
            this.AutoScroll = true;
          
           
            AutoSize = false;


        }

        public List<List<string>> Source
        {
            get => _Source;
            set
            {
                _Source = value;
                if (Source.Count != 0)
                {
                    VerticalScroll.Value = 0;
                    _Bufer = CreateBufer(Source);
                    Invalidate();
                }


            }
        }
        public Color LineColor { get; set; }

        public int ColumnHeight
        {
            get => _ColumnHeight;
            set
            {
                if (_ColumnHeight < Font.Height)
                {
                    _ColumnHeight = Font.Height + 2;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {


            DrawFrame(e);





        }
        public void DrawFrame(PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(LineColor, _LineWidth), Margin.Left, Margin.Top, this.Width - Margin.Left - Margin.Right, Margin.Top);
            e.Graphics.DrawLine(new Pen(LineColor, _LineWidth), this.Width - Margin.Left - Margin.Right, Margin.Top, this.Width - Margin.Left - Margin.Right, this.Height - Margin.Top - Margin.Bottom);
            e.Graphics.DrawLine(new Pen(LineColor, _LineWidth), this.Width - Margin.Left - Margin.Right, this.Height - Margin.Top - Margin.Bottom, Margin.Left, this.Height - Margin.Top - Margin.Bottom);
            e.Graphics.DrawLine(new Pen(LineColor, _LineWidth), Margin.Left, this.Height - Margin.Top - Margin.Bottom, Margin.Left, Margin.Top);
            Brush brush = new SolidBrush(ForeColor);
            Pen p = new Pen(LineColor, _LineWidth);

            foreach (var Row in _Bufer)
            {

                var rowEndCounter = Row.Last().XPosition + Row.Last().ColumnWidth * Font.Size;
                foreach (var Cell in Row)
                {
                    e.Graphics.DrawString(Cell.Body, this.Font, brush, Cell.XPosition, Cell.YPosition);
                    e.Graphics.DrawLine(p, Cell.XPosition + Cell.ColumnWidth * Font.Size, Margin.Top, Cell.XPosition + Cell.ColumnWidth * Font.Size, Margin.Top + (ColumnHeight + 2) * _Bufer.Count);

                }

                e.Graphics.DrawLine(p, Margin.Left, Row[0].YPosition + ColumnHeight, rowEndCounter, Row[0].YPosition + ColumnHeight);
            }

            /*   if (_MaxY > this.Size.Height)
               {
                   vScrollBar1.Visible = true;
                 

               }*/

        }
        private new List<List<Cell>> CreateBufer(List<List<string>> Source)
        {
            int CurrentWidth = Margin.Left + 2;

            List<List<Cell>> TableRows = new List<List<Cell>>();
            for (int i = 0; i < Source.Count; i++)
            {
                TableRows.Add(new List<Cell>());
            }
            int RowIndex = 0;
            int CellIndex = 0;
            var yCounter = Margin.Top + 2;
            foreach (var Rows in TableRows)
            {
                foreach (var item in Source[RowIndex])
                {
                    TableRows[RowIndex].Add(new Cell(Source[RowIndex][CellIndex], 0, yCounter));
                    CellIndex++;
                }
                yCounter += ColumnHeight + 2;
                RowIndex++;
                CellIndex = 0;
            }
            for (int i = 0; i < TableRows[0].Count; i++)
            {
                var ColumnWidth = Source.Select(k => k[i].Length).Max();
                var Column = TableRows.Select(k => k[i]);
                foreach (var item in Column)
                {
                    item.XPosition = CurrentWidth;
                    item.ColumnWidth = ColumnWidth;
                }
                CurrentWidth += ColumnWidth * (int)this.Font.Size + 2 + _LineWidth;
            }

            _MaxY = TableRows.Last().First().YPosition + ColumnHeight + _LineWidth;
            _MaxX = TableRows.Last().Last().XPosition + TableRows.Last().Last().ColumnWidth * (int)Font.Size + 1 + _LineWidth;
          var Width = this.Width;
           var Height = this.Height;
            //  ClientSize = new Size(_MaxX, _MaxY);
            //   this.Height= Height;
            //   this.Width = Width;
            
            AutoScrollMinSize = new Size(_MaxX, _MaxY);
           
            VerticalScroll.Visible = true;
            VerticalScroll.Maximum = (_MaxY - this.Size.Height - ColumnHeight);
            
            VerticalScroll.SmallChange = ColumnHeight + _LineWidth;
            VerticalScroll.Value = 0;
            value = VerticalScroll.Value;
            Counter = 1;            
            return TableRows;
           
        }
        int Counter = 1;
        bool isCounterIncremented = false;
        bool isCounterDecremented = false;

        int value;


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
                                    Cell.YPosition -= ColumnHeight + 2;
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
                                    Cell.YPosition = _Bufer[0][0].YPosition + ColumnHeight + 2;

                                }
                                Counter--;
                                isCounterDecremented = true;
                            }
                            else
                            {
                                foreach (var Cell in _Bufer[i])

                                {
                                    Cell.YPosition += ColumnHeight + 2;
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
                        for (int i =1; i<_Bufer.Count; i++)
                        {
                            if (i == Counter && !isCounterIncremented)
                            {
                                foreach (var Cell in _Bufer[Counter])
                                {
                                    Cell.YPosition = i*-100;
                                   
                                }
                                Counter++;
                                isCounterIncremented = true;
                               
                            }
                            else
                            {
                                foreach (var Cell in _Bufer[i])
                                {
                                    Cell.YPosition -= ColumnHeight + 2;
                                }
                            }

                        }
                       
                        isCounterIncremented = false;
                    }
                    Invalidate();

                }
                if (e.Type == ScrollEventType.SmallDecrement)
                {

                    if (VerticalScroll.Value >= 0 && Counter>1)
                    {
                        
                            Counter--;
                           
                        for (int i = Counter; i < _Bufer.Count; i++)
                        {
                            if (i == Counter && isCounterDecremented==false)
                            {
                                foreach (var Cell in _Bufer[i])

                                {
                                    Cell.YPosition = _Bufer[0][0].YPosition+ColumnHeight+2;
                                    
                                }
                                Counter--;
                                isCounterDecremented = true;
                            }
                            else
                            {
                                foreach (var Cell in _Bufer[i])

                                {
                                    Cell.YPosition += ColumnHeight + 2;
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
        class Cell
        {

            public string Body { get; set; }

            public int XPosition { get; set; }
            public int YPosition { get; set; }
            public int ColumnWidth { get; set; }
            //   public bool NeedRefresh { get; }
            public Cell(string Body, int XPosition, int YPosition)
            {
                this.Body = Body;
                this.XPosition = XPosition;
                this.YPosition = YPosition;
                // this.NeedRefresh = NeedRefresh;
            }
        }
    }
}

