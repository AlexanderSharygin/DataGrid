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
        List<Row> _Bufer = new List<Row>();
        int _RowHeight;
        int _LineWidth = 1;
        int _FirstPrintedRowIndex = 0;
        int _VerticalScrollValueRatio = 10;
        int _HorisontalScrollValueRatio = 1;
        int _CellMinMargin = 2;
        int _TotalRowsCount;
        int _ViewPortRowsCount;
        float _TableWidth;
        Brush _Brush;
        Pen _Pen;
        public MyDataGrid()
        {
            InitializeComponent();
            ResizeRedraw = true;
            components = new System.ComponentModel.Container();
            VerticalScrollBar.Minimum = 0;
            VerticalScrollBar.Value = 0;
            if (VerticalScrollBar.Visible)
            {
                HorisontalScrollBar.Width = this.ClientSize.Width - VerticalScrollBar.Width;
            }
            else
            {
                HorisontalScrollBar.Width = this.Width;
            }
            HorisontalScrollBar.Maximum = 0;
            HorisontalScrollBar.Minimum = 0;
            HorisontalScrollBar.Value = 0;
            _Brush = new SolidBrush(ForeColor);
            _Pen = new Pen(LineColor, _LineWidth);
        }
        public Color LineColor { get; set; }
        public List<List<string>> Source
        {
            get => _Source;
            set
            {
               
                _Source = value;
                if (Source.Count != 0)
                {
                    _Bufer = CreateBuffer(Source);
                    _TotalRowsCount = _Bufer.Count;                    
                    _ViewPortRowsCount = (this.Height) / (RowHeight) - 1;
                    if (_TableWidth > this.Width)
                    {
                        var hidenRowsCount = _RowHeight / HorisontalScrollBar.Height;
                        var remainder = _RowHeight % HorisontalScrollBar.Height;
                        if (remainder != 0)
                        {
                            hidenRowsCount++;
                        }
                        _ViewPortRowsCount = _ViewPortRowsCount - hidenRowsCount;
                    }
                    if (_TotalRowsCount > _ViewPortRowsCount)
                    {
                        VerticalScrollBar.Visible = true;
                    }
                    VerticalScrollBar.Maximum = ((_TotalRowsCount - _ViewPortRowsCount) * _VerticalScrollValueRatio)-1;
                    VerticalScrollBar.SmallChange = _VerticalScrollValueRatio;
                    VerticalScrollBar.LargeChange = _VerticalScrollValueRatio;
                    HorizontalScrollUpdater();
                    HorisontalScrollBar.Value = 0;
                    HorisontalScrollBar.SmallChange = _HorisontalScrollValueRatio;
                    HorisontalScrollBar.LargeChange = _HorisontalScrollValueRatio;
                    Invalidate();
                }
            }
        }       

        public int RowHeight
        {
            get => _RowHeight;
            set
            {
                _RowHeight = (_RowHeight < Font.Height) ? (Font.Height + 2 * _CellMinMargin + _LineWidth) : (_RowHeight + _LineWidth);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _Pen.Color = LineColor;
          
            DrawTable(e);
        }
        public void DrawOutsideFrame(PaintEventArgs e)
        {
            e.Graphics.DrawLine(_Pen, 0, 0, this.Width, 0);
            e.Graphics.DrawLine(_Pen, this.Width, 0, this.Width, this.Height);
            e.Graphics.DrawLine(_Pen, this.Width, this.Height, 0,  this.Height);
            e.Graphics.DrawLine(_Pen, 0, this.Height, 0, 0);

        }
        public void HorizontalScrollUpdater()
        {
            var viewportWidth = this.ClientSize.Width - (VerticalScrollBar.Visible ? VerticalScrollBar.Width : 0);
            if (_TableWidth >= viewportWidth)
            {
                HorisontalScrollBar.Visible = true;
                HorisontalScrollBar.Maximum = (int)(_TableWidth - viewportWidth + 1);
            }
            else
            {
                HorisontalScrollBar.Visible = false;
                HorisontalScrollBar.Maximum = 0;
            }
        }
        public void DrawHeader(PaintEventArgs e)
        {
            
            if (_Bufer.Count != 0)
            {       
                if (_ViewPortRowsCount > _Bufer.Count - 1)
                {
                    _ViewPortRowsCount = _Bufer.Count - 1;
                }
                int xCounterForLine = 0;             
                foreach (var haderCell in _Bufer.First().Cells)
                {
                     e.Graphics.DrawLine(_Pen,xCounterForLine - HorisontalScrollBar.Value, 0, xCounterForLine - HorisontalScrollBar.Value,  RowHeight * (_ViewPortRowsCount+1 ));
                    xCounterForLine +=_LineWidth + _CellMinMargin + haderCell.ColumnWidth*(int)this.Font.Size + _CellMinMargin;
                 } 
               e.Graphics.DrawLine(_Pen, _TableWidth- HorisontalScrollBar.Value,0, _TableWidth - HorisontalScrollBar.Value, 0 + RowHeight * (_ViewPortRowsCount + 1));
              e.Graphics.DrawLine(_Pen, 0 - HorisontalScrollBar.Value, RowHeight, _TableWidth - HorisontalScrollBar.Value, RowHeight);
                      
                Control HorizontalScroll = Controls[0];
                Control VerticalScroll = Controls[1];
                Controls.Clear();
                Controls.Add(HorizontalScroll);
                Controls.Add(VerticalScroll);
                int xCounter = 0;
                HeaderCell[] Header = new HeaderCell[_Bufer.First().Cells.Count];
                for (int i = 0; i < Header.Length; i++)
                {
                    Header[i] = new HeaderCell();                   
                    Header[i].Font = this.Font;
                    Header[i].Width = (int)(_LineWidth*2 + _CellMinMargin + _Bufer.First().Cells[i].ColumnWidth * (int)this.Font.Size + _CellMinMargin);
                    Header[i].Height = _RowHeight;
                    Header[i].Location = new Point(xCounter-HorisontalScrollBar.Value, 0);
                    Header[i].Text = _Bufer.First().Cells[i].Body;
                    Header[i].ColumnIndex = i;
                    if (SortProcessor.ColumnSortIndex == i)
                    {
                        Header[i].SortDirection = SortProcessor.SortDirection;
                    }
                    else
                    {
                        Header[i].SortDirection = 0;
                    }
                    xCounter += _LineWidth + _CellMinMargin + _Bufer.First().Cells[i].ColumnWidth * (int)this.Font.Size + _CellMinMargin;
                    Controls.Add(Header[i]);
                }          
            }
        }

        public void DrawTable(PaintEventArgs e)
        {
            DrawOutsideFrame(e);
            DrawHeader(e);          
            if (_Bufer.Count != 0)
            {              
                int bufferRowIndex = _FirstPrintedRowIndex + 1;
                int viewPortRowIndex = 1;
                for (int i = 0; i < _ViewPortRowsCount; i++)
                {
                    if (bufferRowIndex < _Bufer.Count)
                    {
                        int xCounterForText = _LineWidth + _CellMinMargin;
                        foreach (var Cell in _Bufer[bufferRowIndex].Cells)
                        {
                            e.Graphics.DrawString(Cell.Body, this.Font, _Brush, xCounterForText - HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex) + (RowHeight - FontHeight) / 2);
                            xCounterForText += Cell.ColumnWidth * (int)Font.Size + _CellMinMargin + _LineWidth + _CellMinMargin;
                        }
                        e.Graphics.DrawLine(_Pen,  -HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex + 1), _TableWidth - HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex + 1));
                        viewPortRowIndex++;
                        bufferRowIndex++;
                    }
                }
            }
        }
        private List<Row> CreateBuffer(List<List<string>> Source)
        {
            int currentWidth = Margin.Left + _VerticalScrollValueRatio;
            List<Row> tableRows = new List<Row>();
            for (int i = 0; i < Source.Count; i++)
            {
                tableRows.Add(new Row());
            }
            int rowIndex = 0;
            int cellIndex = 0;
            foreach (var Row in tableRows)
            {
                foreach (var item in _Source[rowIndex])
                {
                    Row.Cells.Add(new Cell(_Source[rowIndex][cellIndex]));
                    cellIndex++;
                }
                rowIndex++;
                cellIndex = 0;
            }
            for (int i = 0; i < tableRows.First().Cells.Count; i++)
            {
                var columnWidth = _Source.Select(k => k[i].Length).Max();
                var column = tableRows.Select(k => k.Cells[i]);
                foreach (var item in column)
                {                
                    item.ColumnWidth = columnWidth;
                }         
            }
            var firstRow = tableRows.First();
            var width = 0;
            foreach (var item in firstRow.Cells)
            {
                width += (_LineWidth + _CellMinMargin + item.ColumnWidth* (int)this.Font.Size + _CellMinMargin);
            }
            _TableWidth = width+ _LineWidth;       
            return tableRows;
        }
        class Cell
        {
            public string Body { get; set; }       
            public int ColumnWidth { get; set; }
            public Cell(string Body) 
            {
                this.Body = Body;        
            }
        }
        class Row
        {
            public List<Cell> Cells { get; set; } = new List<Cell>();
        }
        private void MyDataGrid_Resize(object sender, EventArgs e)
        {
            if (_Bufer.Count != 0)
            {
                _ViewPortRowsCount = this.Height / (RowHeight) - 1;
                if (_TableWidth > this.Width-VerticalScrollBar.Width)
                {
                    var hidenRowsCount = _RowHeight / HorisontalScrollBar.Height;
                    var remainder = _RowHeight % HorisontalScrollBar.Height;
                    if (remainder != 0)
                    {
                        hidenRowsCount++;
                    }
                    _ViewPortRowsCount = _ViewPortRowsCount - hidenRowsCount;
                }
                if (_TotalRowsCount > _ViewPortRowsCount + 1)
                {
                    VerticalScrollBar.Visible = true;
                }
                else
                {
                    VerticalScrollBar.Visible = false;
                }
                if (VerticalScrollBar.Value < 0)
                {
                    VerticalScrollBar.Minimum = 0;
                    VerticalScrollBar.Value = 0;
                }
                VerticalScrollBar.Maximum = ((_TotalRowsCount - _ViewPortRowsCount) * _VerticalScrollValueRatio - 1);
            }          
            HorizontalScrollUpdater();
            Invalidate();
        }
        private void VScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            _FirstPrintedRowIndex = VerticalScrollBar.Value / _VerticalScrollValueRatio;
            if (VerticalScrollBar.Value < 0)
            {
                _FirstPrintedRowIndex = 0;
            }
            Invalidate();
        }    
        private void VerticalScrollBar_VisibleChanged(object sender, EventArgs e)
        {
            if (!VerticalScrollBar.Visible)
            {
                HorisontalScrollBar.Width = this.Width;
            }
            else
            {
                HorisontalScrollBar.Width = this.Width - VerticalScrollBar.Width;
            }
        }

        private void HorisontalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }
    }
    static class SortProcessor
    {
        static int _SortDirection = 0;
        static int _ColumnSortIndex = 0;
        static bool _IsColumnChanged = false;
        public static int ColumnSortIndex
        { get =>_ColumnSortIndex;
            set
            {
                if (_ColumnSortIndex == value)
                {
                    _IsColumnChanged = false;
                }
                else
                {
                    _IsColumnChanged = true;
                }
                _ColumnSortIndex = value;
            }
        }
       public static int SortDirection { get => _SortDirection;}
       public static void ChangeSortDirection()
        {
            if (!_IsColumnChanged)
            {
                if (_SortDirection < 0)
                {
                    _SortDirection = 0;

                }

                else if (_SortDirection == 0)
                {
                    _SortDirection = 1;
                }
                else
                {
                    _SortDirection = -1;
                }
            }
        }
     
    }



    
}

