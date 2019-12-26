using System;
using System.Collections.Generic;
using Parser.Properties;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Parser
{
    public partial class MyDataGrid : UserControl
    {
        List<List<string>> _ColumnsData = new List<List<string>>();

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

        internal ObservableCollection<Column> Columns
        {
            get
            {
                return _API.Columns;

            }
            set
            {
                _API.Columns = value;
                Invalidate();
            }

        }

        public MyDataGrid()
        {
            InitializeComponent();
          
          
            _API = new APICore();
           Columns.CollectionChanged +=Columns_CollectionChanged;
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
        public List<List<string>> ColumnsData
        {

            get => _ColumnsData;
            set
            {
                _ColumnsData = value;

                /*if (ColumnsData.Count != 0)
                {
                    //  _API.UpdateColumns(ColumnsData);
                   
                    _Bufer = CreateBuffer(ColumnsData);
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
                    VerticalScrollBar.Maximum = ((_TotalRowsCount - _ViewPortRowsCount) * _VerticalScrollValueRatio) - 1;
                    VerticalScrollBar.SmallChange = _VerticalScrollValueRatio;
                    VerticalScrollBar.LargeChange = _VerticalScrollValueRatio;
                    UpdateHorizontalScroll();
                    HorisontalScrollBar.Value = 0;
                    HorisontalScrollBar.SmallChange = _HorisontalScrollValueRatio;
                    HorisontalScrollBar.LargeChange = _HorisontalScrollValueRatio;
                    Invalidate();
                    for (int i = 0; i < Controls.Count; i++)
                    {
                        Type Type = Controls[i].GetType();

                        if (Type == typeof(TypeSelector))
                        {
                            Controls[i].Visible = false;
                        }
                    }
                }*/
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

        public void UpdateColumns()
        {
           
            if (_API.Columns.Count != 0)
            {
                ColumnsData.Clear();
                foreach (var item in _API.Columns)
                {
                    if (ColumnsData.Count == 0)
                    {
                        ColumnsData.Add(new List<string>());
                    }

                    ColumnsData.First().Add(item.HeaderText);
                    for (int i = 0; i < item.Items.Count; i++)
                    {
                        if (ColumnsData.Count < i + 2)
                        {
                            ColumnsData.Add(new List<string>());
                        }
                        ColumnsData[i + 1].Add(item.Items[i]);
                    }
                }
                _Bufer = CreateBuffer(ColumnsData);
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
                VerticalScrollBar.Maximum = ((_TotalRowsCount - _ViewPortRowsCount) * _VerticalScrollValueRatio) - 1;
                VerticalScrollBar.SmallChange = _VerticalScrollValueRatio;
                VerticalScrollBar.LargeChange = _VerticalScrollValueRatio;
                UpdateHorizontalScroll();
                HorisontalScrollBar.Value = 0;
                HorisontalScrollBar.SmallChange = _HorisontalScrollValueRatio;
                HorisontalScrollBar.LargeChange = _HorisontalScrollValueRatio;
                Invalidate();
                for (int i = 0; i < Controls.Count; i++)
                {
                    Type Type = Controls[i].GetType();

                    if (Type == typeof(TypeSelector))
                    {
                        Controls[i].Visible = false;
                    }
                }
            }
            
           
        }
        protected override void OnPaint(PaintEventArgs e)
        {
          
            _Pen.Color = LineColor;
            _TableWidth = 0;
            foreach (var item in _API.Columns)
            {
                if (item.Visible)
                {
                    _TableWidth += (_LineWidth + _CellMinMargin + item.Width * (int)this.Font.Size + _CellMinMargin);
                }
            }
            UpdateHorizontalScroll();
            DrawTable(e);

        }      
   
      
       private void Columns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
       {

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                UpdateColumns();
            }
         //   Column item1 = (Column)e.NewItems;
            foreach (var item in Columns)
           {
                if (!item.isSigned)
                {
                    item.PropertyChanged += OnPropertyChanged;
                    item.isSigned = true;
                }
           }
         

        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Visible")
            {
               Invalidate();

            }
            if (e.PropertyName == "SortDirection")
            {
                Column a = (Column)sender;               
                _API.ChangeSortedColumn(a.Index);               
               
            }
            Invalidate();

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
                    if (_API.Columns[i].Visible)
                    {
                        e.Graphics.DrawLine(_Pen, xCounterForLine - HorisontalScrollBar.Value, 0, xCounterForLine - HorisontalScrollBar.Value, RowHeight * (_ViewPortRowsCount + 1));
                        xCounterForLine += _LineWidth + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin;
                    }
                }             
               e.Graphics.DrawLine(_Pen, _TableWidth- HorisontalScrollBar.Value,0, _TableWidth - HorisontalScrollBar.Value, 0 + RowHeight * (_ViewPortRowsCount + 1));
              e.Graphics.DrawLine(_Pen, 0 - HorisontalScrollBar.Value, RowHeight, _TableWidth - HorisontalScrollBar.Value, RowHeight);
                      
                Control HorizontalScroll = Controls[0];
                Control VerticalScroll = Controls[1];
                Controls.Clear();
                Controls.Add(HorizontalScroll);
                Controls.Add(VerticalScroll);
                int xCounter = 0;
             //   _API.SortColumns();
                
                List<HeaderCell> Header = new List<HeaderCell>();               
                for (int i = 0; i < _API.Columns.Count; i++)
                {
                    if (_API.Columns[i].Visible)
                    {
                        HeaderCell Cell = new HeaderCell(_API.Columns[i]);
                        Cell.Font = this.Font;
                       Cell.Width = (int)(_LineWidth * 2 + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin);
                        Cell.Height = _RowHeight;
                        Cell.Location = new Point(xCounter - HorisontalScrollBar.Value, 0);
                        xCounter += _LineWidth + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin;
                        Header.Add(Cell);
                        Controls.Add(Cell);
                    }
                }
                foreach (var headerCell in Header)
                {
                    
                    List<HeaderCell> temp = new List<HeaderCell>();                   
                    var OtherCells = Header.Select(k => k).Where(k => k!=headerCell).ToList();                   
                       headerCell.NeighborCells.Clear();
                        headerCell.NeighborCells.AddRange(OtherCells);
                  
                   
                }
                
            }
        }
        public void SortBuferColumns()
        {
            foreach (var Cell in _Bufer.First().Cells)
            {
                Cell.ColumnNumber = _API.Columns.Where(u => Cell.Body == u.HeaderText).Select(u => u.Index).Single();
            }        
            for (int i = 1; i < _Bufer.Count; i++)
            {                              
                    for (int j = 0; j <_Bufer.First().Cells.Count; j++)
                    {
                        var Column = _Bufer.Select(k => k.Cells[j]).ToList();
                        Column.ForEach(k => k.ColumnNumber = Column.First().ColumnNumber);
                    }                       
            }                  
             _Bufer.ForEach(k => k.Cells = k.Cells.OrderBy(f => f.ColumnNumber).ToList());       
        }
        public void DrawTable(PaintEventArgs e)
        {
            DrawOutsideFrame(e);
            
            if (_Bufer.Count != 0 && _Bufer.First().Cells.Count != 0)
            {
                SortBuferColumns();
               _API.SortColumns();
           
               
           
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
                    if (_API.Columns[ColumnIndex].SortDirection!=Sort.None)
                    {
                        RowComparer u = (_API.Columns[ColumnIndex].SortDirection == Sort.ASC) ? new RowComparer(true, ColumnIndex, _API.Columns[ColumnIndex].Type) : new RowComparer(false, ColumnIndex, _API.Columns[ColumnIndex].Type);
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
                        for (int j = 0; j < SortedBufer[bufferRowIndex].Cells.Count; j++)
                        {
                            if (_API.Columns[j].Visible)
                            {
                                Cell Cell = SortedBufer[bufferRowIndex].Cells[j];
                                e.Graphics.DrawString(Cell.Body, this.Font, _Brush, xCounterForText - HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex) + (RowHeight - FontHeight) / 2);
                                xCounterForText += _API.Columns[j].Width * (int)Font.Size + _CellMinMargin + _LineWidth + _CellMinMargin;
                                index++;
                            }
                        }
                        e.Graphics.DrawLine(_Pen,  -HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex + 1), _TableWidth - HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex + 1));
                        viewPortRowIndex++;
                        bufferRowIndex++;
                    }
                }
            }
            DrawHeader(e);

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
                for (int i = 0; i < Source.First().Count; i++)
                {
                   // if (Source.First().Count - Source[rowIndex].Count != 0)
                  //  {
                  //      int delta = Source.First().Count - Source[rowIndex].Count;
                  //      for (int j = 0; j < delta; j++)
                   //     {
                   //         Source[rowIndex].Add("");
                   //     }

                  //  }
                
                    Row.Cells.Add(new Cell(Source[rowIndex][i]));
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
                if (item.Visible)
                {
                    width += (_LineWidth + _CellMinMargin + item.Width * (int)this.Font.Size + _CellMinMargin);
                }
            }
            _TableWidth = width+ _LineWidth;       
            return tableRows;
        }
        class Cell
        {
            public string Body { get; set; }     
          
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
            Type _Type;
            public RowComparer(bool direction, int index, Type t)
            {
                _Direction = direction;
                _ColumnIndex = index;
                _Type = t;
            }
            public int Compare(Row x, Row y)
            {
                object xValue=null;
                object yValue=null;
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

                int dir = (_Direction)?1:-1;
              
               
               
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

