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
using System.Runtime.CompilerServices;

namespace WinFormsParser
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
        List<List<TableCell>> TableCells = new List<List<TableCell>>();
        List<Row> SortedBufer = new List<Row>();

        internal ObservableCollection<Column> Columns
        {
            get
            {
                return _API.Columns;
            }

        }
        [DisplayName(@"DataSource"), Description("Используйте таблицу данных в формте List<List<string>> (ColumnsAutoGeneretion должен быть true)")]
        public object Source
        {
            get => _Source;
            set
            {
                if (ColumnsAutoGeneretion)
                {
                    try
                    {
                        _Source = (List<List<string>>)value;

                        _API.Columns.Clear();
                        var index = 0;
                        foreach (var item in _Source)
                        {
                            _API.Columns.Add(new Column(item.First(), index, typeof(string), item.GetRange(1, item.Count - 1)));
                        }
                        UpdateControl();
                        UpdateTableCells(true);
                      
                    }
                    catch 
                    {
                        throw new Exception("Source должен иметь формат List<List<string>>");
                    }

                }
            }
        }
        [DisplayName(@"ColumnsAutoGeneretion"), Description("Если value=true - колонки генерируются автоматически из коллекции Source"), DefaultValue(false)]
        public bool ColumnsAutoGeneretion { get; set; } = false;
        public Color LineColor { get; set; }
       
        public int RowHeight
        {
            get => _RowHeight;
            set
            {
                if (value > Font.Height + 2 * _CellMinMargin + _LineWidth)
                {
                    _RowHeight = value;

                    UpdateControl();
                    UpdateTableCells(true);
                }
            }
        }
      
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                _RowHeight = (_RowHeight < Font.Height + 2 * _CellMinMargin + _LineWidth) ? (Font.Height + 2 * _CellMinMargin + _LineWidth) : _RowHeight;
                UpdateControl();
                UpdateTableCells(true);


            }
        }
        public MyDataGrid()
        {
            //base.AutoScaleMode = AutoScaleMode.None;
            
            InitializeComponent();
            components = new System.ComponentModel.Container();
            _API = new APICore();
            _API.PropertyChanged += APIPropertyChanged;
            Columns.CollectionChanged += ColumnsCollectionChanged;
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
        private void APIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SortDirection")
            {
                Invalidate();
            }
        }
        private void ColumnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Visible")
            {
                UpdateTableCells(true);
                Invalidate();
            }
            if (e.PropertyName == "Type")
            {
                
                Invalidate();
             
            }
        }
        private void ColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
               
                UpdateControl();
                UpdateTableCells(false);
                foreach (var item in Columns)
                {
                    if (!item.IsSignedToPropertyChange)
                    {
                        item.PropertyChanged += ColumnPropertyChanged;
                        item.IsSignedToPropertyChange = true;
                    }
                }

            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                
                UpdateControl();
                UpdateTableCells(false);
            }
          
        }
        public void ChangeSorting(string columnName, Sort sortDirection)
        {
            var newSortedColumn = Columns.Select(k => k).Where(u => u.HeaderText == columnName).Single();
            if (newSortedColumn.Visible)
            {
                _API.SortDirection = sortDirection;
                _API.SortedColumnIndex = newSortedColumn.Index;
            }
        }
        private void UpdateControl()
        {           
            if (_API.Columns.Count != 0)
            {               
                _Bufer = CreateBuffer();
                _TotalRowsCount = _Bufer.Count;
                _ViewPortRowsCount = (this.Height) / (RowHeight)-1;
                if (_TableWidth > this.Width)
                {
                 
                     var hidenRowsCount = _RowHeight / HorisontalScrollBar.Height;
                     var remainder = _RowHeight % HorisontalScrollBar.Height;
                      if (remainder != 0)
                     {
                       _ViewPortRowsCount--;
                    }                  
                }
                if (_TotalRowsCount > _ViewPortRowsCount)
                {
                    VerticalScrollBar.Visible = true;
                }
                VerticalScrollBar.Minimum = 0;
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
            else
            {
                Invalidate();
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
            foreach (var item in Controls)
            {
                if (item.GetType() == typeof(HeaderCell))
                {
                    var a = (HeaderCell)(item);
                    if (a.IsMoved)
                    {
                        UpdateTableCells(true);
                        a.IsMoved = false;
                        break;

                    }
                }
            }
            DrawHeader(e);
        }


        private void DrawOutsideFrame(PaintEventArgs e)
        {
            e.Graphics.DrawLine(_Pen, 0, 0, this.Width, 0);
            e.Graphics.DrawLine(_Pen, this.Width, 0, this.Width, this.Height);
            e.Graphics.DrawLine(_Pen, this.Width, this.Height, 0, this.Height);
            e.Graphics.DrawLine(_Pen, 0, this.Height, 0, 0);
        }
        private void UpdateTableCells(bool SortColumns)
        {
            if (_Bufer.Count != 0 && _Bufer.First().Cells.Count != 0 && Columns.Count > 0)
            {
                SortBuferColumns();
                if (SortColumns)
                {
                    _API.SortColumns();
                }
                SortedBufer.Clear();
                SortedBufer.AddRange(_Bufer);
                SortedBufer.RemoveAt(0);
                for (int i = 0; i < _API.Columns.Count; i++)
                {
                    _API.Columns[i].Items.Clear();
                    foreach (var item in SortedBufer)
                    {
                        foreach (var Cell in item.Cells)
                        {
                            if (Cell.ColumnNumber == _API.Columns[i].Index)
                            {
                                _API.Columns[i].Items.Add(Cell.Body);
                            }
                        }
                    }
                }           
                if (_ViewPortRowsCount > _Bufer.Count - 1)
                {
                    _ViewPortRowsCount = _Bufer.Count - 1;
                }
                Control HorizontalScroll = Controls[0];
                Control VerticalScroll = Controls[1];
                Controls.Clear();
                Controls.Add(HorizontalScroll);
                Controls.Add(VerticalScroll);
                int xCounter = 0;
                List<HeaderCell> Header = new List<HeaderCell>();
                for (int i = 0; i < _API.Columns.Count; i++)
                {
                    int bufferRowIndex = _FirstPrintedRowIndex;
                    int viewPortRowIndex = 1;
                    if (_API.Columns[i].Visible)
                    {
                        HeaderCell Cell = new HeaderCell(_API.Columns[i]);
                        Cell.Font = this.Font;
                        Cell.Width = (int)(_LineWidth * 2 + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin);
                        Cell.Height = _RowHeight;
                        Cell.Location = new Point(xCounter - HorisontalScrollBar.Value, 0);
                        Cell.StartX = Cell.Location.X;
                        Cell._API = _API;
                        Header.Add(Cell);
                        Controls.Add(Cell);
                        for (int j = 0; j < _API.Columns[i].Items.Count; j++)
                        {
                            if (bufferRowIndex < _API.Columns[i].Items.Count)
                            {
                                if (viewPortRowIndex < _ViewPortRowsCount + 1)
                                {
                                    TableCell TC = new TableCell();
                                    TC.Type = _API.Columns[i].Type;
                                    TC.ColumnIndex = _API.Columns[i].Index;
                                    TC.LineWidth = _LineWidth;
                                    TC.LineColor = LineColor;
                                    TC.Width = Cell.Width; 
                                    TC.Height = _RowHeight;
                                    TC.Location = new Point(xCounter - HorisontalScrollBar.Value, (RowHeight - _LineWidth) * viewPortRowIndex);
                                    TC.StartX = TC.Location.X;
                                    TC.Font = this.Font;
                                    Controls.Add(TC);                                  
                                    
                                    viewPortRowIndex++;
                                    bufferRowIndex++;
                                }

                            }

                        }
                        xCounter += _LineWidth + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin;

                    }
                }
                foreach (var headerCell in Header)
                {
                    List<HeaderCell> temp = new List<HeaderCell>();
                    var OtherCells = Header.Select(k => k).Where(k => k != headerCell).ToList();
                    headerCell.NeighborCells.Clear();
                    headerCell.NeighborCells.AddRange(OtherCells);
                }

            }
        }

        private void DrawHeader(PaintEventArgs e)
        {
            DrawOutsideFrame(e);           
            if (_Bufer.Count != 0 && _Bufer.First().Cells.Count != 0 && Columns.Count > 0)
            {
                SortedBufer.Clear();
                SortedBufer.AddRange(_Bufer);
                SortedBufer.RemoveAt(0);
                if (_API.SortedColumnIndex != -1)
                {
                    if (_API.SortDirection != Sort.None)
                    {
                        RowComparer u = (_API.SortDirection == Sort.ASC) ? new RowComparer(true, _API.SortedColumnIndex, _API.Columns[_API.SortedColumnIndex].Type) : new RowComparer(false, _API.SortedColumnIndex, _API.Columns[_API.SortedColumnIndex].Type);
                        SortedBufer.Sort(u);
                    }
                }
                for (int i = 0; i < _API.Columns.Count; i++)
                {
                    _API.Columns[i].Items.Clear();
                    foreach (var item in SortedBufer)
                    {
                        foreach (var Cell in item.Cells)
                        {
                            if (Cell.ColumnNumber == _API.Columns[i].Index)
                            {
                                _API.Columns[i].Items.Add(Cell.Body);
                            }
                        }
                    }
                }              
                for (int i = 0; i < _API.Columns.Count; i++)
                {
                    int bufferRowIndex = _FirstPrintedRowIndex;              
                    if (_API.Columns[i].Visible)
                    {
                        List<TableCell> temp = new List<TableCell>();
                        foreach (var item in Controls)
                        {
                            if (item.GetType() == typeof(TableCell))
                            {
                                var Cell = (TableCell)item;
                                if (Cell.ColumnIndex == _API.Columns[i].Index)
                                {
                                    temp.Add(Cell);
                                }
                            }
                        }

                        for (int j = 0; j < temp.Count; j++)
                        {
                            if (temp[j].Value != _API.Columns[i].Items[bufferRowIndex])
                            {
                                temp[j].Value = _API.Columns[i].Items[bufferRowIndex];
                                temp[j].Invalidate();
                            }
                            if (temp[j].Type != _API.Columns[i].Type)
                            {
                                temp[j].Type = _API.Columns[i].Type;
                                temp[j].Invalidate();
                            }                                               
                            bufferRowIndex++;
                        }
                        foreach (var item in Controls)
                        {
                            if (item.GetType() == typeof(HeaderCell))
                            {
                                var Cell = (HeaderCell)item;
                                Cell.Invalidate();
                                
                            }
                        }

                    }
                }
               
              
            }
        }       
        private void SortBuferColumns()
        {
            foreach (var Cell in _Bufer.First().Cells)
            {
                Cell.ColumnNumber = _API.Columns.Where(u => Cell.Body == u.HeaderText).Select(u => u.Index).Single();
            }
            for (int i = 1; i < _Bufer.Count; i++)
            {
                for (int j = 0; j < _Bufer.First().Cells.Count; j++)
                {
                    var Column = _Bufer.Select(k => k.Cells[j]).ToList();
                    Column.ForEach(k => k.ColumnNumber = Column.First().ColumnNumber);
                }
            }
            _Bufer.ForEach(k => k.Cells = k.Cells.OrderBy(f => f.ColumnNumber).ToList());
        }

        private void UpdateHorizontalScroll()
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
        private List<Row> CreateBuffer()
        {

            List<Row> tableRows = new List<Row>();
            for (int i = 0; i < _API.Columns.First().Items.Count + 1; i++)
            {
                tableRows.Add(new Row());
            }
            for (int i = 0; i < tableRows.Count; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < _API.Columns.Count; j++)
                    {
                        tableRows[i].Cells.Add(new Cell(_API.Columns[j].HeaderText));
                    }
                }
                else
                {
                    for (int j = 0; j < _API.Columns.Count; j++)
                    {
                        tableRows[i].Cells.Add(new Cell(_API.Columns[j].Items[i - 1]));
                    }
                }
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
            _TableWidth = width + _LineWidth;
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
                        _ViewPortRowsCount--;
                    }
                
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
            UpdateTableCells(true);
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
            foreach (var item in Controls)
            {
                if (item.GetType() == typeof(HeaderCell))
                {
                    var Cell = (HeaderCell)item;
                    Cell.Location = new Point(Cell.StartX - HorisontalScrollBar.Value, Cell.Location.Y);

                }
                if (item.GetType() == typeof(TableCell))
                {
                    var Cell = (TableCell)item;
                    Cell.Location = new Point(Cell.StartX - HorisontalScrollBar.Value, Cell.Location.Y);


                }
            }
            Invalidate();
        }
    } 
  
}

