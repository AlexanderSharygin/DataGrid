using System;
using System.Collections.Generic;

using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace Parser
{
    public partial class MyDataGrid : UserControl
    {  
        List<List<string>> _Source = new List<List<string>>();
        List<Row> _Bufer = new List<Row>();
        APICore _API;      
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
            _API = new APICore();
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
                    _API.UpdateColumns(Source);
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
                    UpdateHorizontalScroll();
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
       
        public void DrawHeader(PaintEventArgs e)
        {
            
            if (_Bufer.Count != 0)
            {
                           
                if (_ViewPortRowsCount > _Bufer.Count - 1)
                {
                    _ViewPortRowsCount = _Bufer.Count - 1;
                }
                int xCounterForLine = 0;

                for (int i = 0; i < _API.Columns.Count; i++)                           
                {               
                   e.Graphics.DrawLine(_Pen, xCounterForLine - HorisontalScrollBar.Value, 0, xCounterForLine - HorisontalScrollBar.Value, RowHeight * (_ViewPortRowsCount + 1));
                   xCounterForLine += _LineWidth + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin;

                }             
               e.Graphics.DrawLine(_Pen, _TableWidth- HorisontalScrollBar.Value,0, _TableWidth - HorisontalScrollBar.Value, 0 + RowHeight * (_ViewPortRowsCount + 1));
              e.Graphics.DrawLine(_Pen, 0 - HorisontalScrollBar.Value, RowHeight, _TableWidth - HorisontalScrollBar.Value, RowHeight);
                      
                Control HorizontalScroll = Controls[0];
                Control VerticalScroll = Controls[1];
                Controls.Clear();
                Controls.Add(HorizontalScroll);
                Controls.Add(VerticalScroll);
                int xCounter = 0;
                _API.SortColumns();
                HeaderCell[] Header = new HeaderCell[_API.Columns.Count];               
                for (int i = 0; i < Header.Length; i++)
                {
                    Header[i] = new HeaderCell(_API.Columns[i]);                   
                    Header[i].Font = this.Font;
                    Header[i].Width = (int)(_LineWidth*2 + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin);
                    Header[i].Height = _RowHeight;
                    Header[i].Location = new Point(xCounter-HorisontalScrollBar.Value, 0);                                      
                    xCounter += _LineWidth + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin;
                    Controls.Add(Header[i]);
                }
                foreach (var headerCell in Header)
                {

                    List<HeaderCell> temp = new List<HeaderCell>();                   
                    var a = Header.Select(k => k).Where(k => k!=headerCell).ToList();                   
                       headerCell.NeighborCells.Clear();
                        headerCell.NeighborCells.AddRange(a);                  
                }
                
            }
        }
        public void SortBufer()
        {
            for (int i = 0; i < _Bufer.Count; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < _Bufer[i].Cells.Count; j++)
                    {
                        for (int k = 0; k < _API.Columns.Count; k++)
                        {
                            if (_Bufer[i].Cells[j].Body == _API.Columns[k].HeaderText)
                            {
                                _Bufer[i].Cells[j].ColumnNumber = _API.Columns[k].Index;

                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < _Bufer[i].Cells.Count; j++)
                    {

                        _Bufer[i].Cells[j].ColumnNumber = _Bufer[0].Cells[j].ColumnNumber;


                    }
                }


            }
            for (int i = 0; i < _Bufer.Count; i++)
            {
                _Bufer[i].Cells = _Bufer[i].Cells.OrderBy(k => k.ColumnNumber).ToList();
            }
        }
        public void DrawTable(PaintEventArgs e)
        {
            DrawOutsideFrame(e);           
            if (_Bufer.Count != 0 && _Bufer.First().Cells.Count!=0)
            {               
                SortBufer();
                _API.SortColumns();
                DrawHeader(e);
                int ColumnIndex = -1;
                foreach (var item in _API.Columns)
                {
                    if (item.IsSortedBy)
                    {
                        ColumnIndex = item.Index;
                    }
                }
                List<Row> SortedBufer = new List<Row>();
                SortedBufer.AddRange(_Bufer);
                SortedBufer.RemoveAt(0);
                if (ColumnIndex > -1)
                {
                    if (_API.Columns[ColumnIndex].SortDirecion!=Sort.None)
                    {
                        RowComparer u = (_API.Columns[ColumnIndex].SortDirecion == Sort.ASC) ? new RowComparer(true, ColumnIndex) : new RowComparer(false, ColumnIndex);
                        SortedBufer.Sort(u);
                    }
                }
                int bufferRowIndex = _FirstPrintedRowIndex;
                int viewPortRowIndex = 1;
                for (int i = 0; i < _ViewPortRowsCount; i++)
                {
                    if (bufferRowIndex < SortedBufer.Count)
                    {
                        int xCounterForText = _LineWidth + _CellMinMargin;
                        int index = 0;
                        foreach (var Cell in SortedBufer[bufferRowIndex].Cells)
                        {
                            
                            e.Graphics.DrawString(Cell.Body, this.Font, _Brush, xCounterForText - HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex) + (RowHeight - FontHeight) / 2);
                            xCounterForText += _API.Columns[index].Width * (int)Font.Size + _CellMinMargin + _LineWidth + _CellMinMargin;
                            index++;
                        }
                        e.Graphics.DrawLine(_Pen,  -HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex + 1), _TableWidth - HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex + 1));
                        viewPortRowIndex++;
                        bufferRowIndex++;
                    }
                }
            }
            
        }
        public void UpdateHorizontalScroll()
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
        private List<Row> CreateBuffer(List<List<string>> Source)
        {        
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
            foreach (var item in tableRows)
            {
                for (int i = 0; i < item.Cells.Count; i++)
                {
                    item.Cells[i].ColumnNumber = i;
                }
            }      
            var width = 0;
            foreach (var item in _API.Columns)
            {
                width += (_LineWidth + _CellMinMargin + item.Width* (int)this.Font.Size + _CellMinMargin);
            }
            _TableWidth = width+ _LineWidth;       
            return tableRows;
        }
        class Cell
        {
            public string Body { get; }     
          
            public int ColumnNumber { get; set; }
            public Cell(string Body) 
            {
                this.Body = Body;        
            }
        }
        class Row
        {
            public List<Cell> Cells { get; set; } = new List<Cell>();

        }
        class RowComparer : IComparer<Row>
        {
            bool _Direction;
            int _ColumnIndex;
            public RowComparer(bool direction, int index)
            {
                _Direction = direction;
                _ColumnIndex = index;
            }
            public int Compare(Row x, Row y)
            {
                int dir = (_Direction)?1:-1;

                
                int yDigit;
                int xDigit;
                if (int.TryParse(x.Cells[_ColumnIndex].Body, out xDigit) && int.TryParse(y.Cells[_ColumnIndex].Body, out yDigit))
                {
                    if (xDigit < yDigit)
                    {
                        return -1*dir;
                    }
                    if (xDigit > yDigit)
                    {
                        return 1*dir;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return (_Direction)? x.Cells[_ColumnIndex].Body.CompareTo(y.Cells[_ColumnIndex].Body): y.Cells[_ColumnIndex].Body.CompareTo(x.Cells[_ColumnIndex].Body);
                }
            }
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
            UpdateHorizontalScroll();
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
 
     
   



    
}

