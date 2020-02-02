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
   [System.ComponentModel.DesignerCategory("Code")]
    public partial class MyDataGrid : UserControl
    {

        List<List<string>> _Source;
        List<Row> _Bufer;
        APICore _API;
        EditorSelector _Editor;
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
         List<HeaderCell> Header;
    
        public MyDataGrid()
        {

            AutoScaleMode = AutoScaleMode.None;
            InitializeComponent();
            components = new System.ComponentModel.Container();
            _Source = new List<List<string>>();
            _Bufer = new List<Row>();
            _API = new APICore();
            _API.PropertyChanged += APIPropertyChanged;
            Columns.CollectionChanged += ColumnsCollectionChanged;
            ResizeRedraw = true;
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
            MouseWheel += MyDataGrid_MouseWheel;
            Leave += MyDataGrid_LostFocus;
        }
        private void RemoveEditorFromControls(bool isDropChanges)
        {
            foreach (var item in Controls)
            {
                if (item.GetType() == _Editor?.GetComponentType())
                {
                    Controls.Remove((Control)item);
                    if (!isDropChanges)
                    {
                        _Editor.BuferCell.Body = _Editor.OriginalValue;
                    }                    
                    _Editor = null;
                    _API.IsEditorNedded = false;
                    _API.IsEditorOpened = false;

                }
            }
        
        }
        private void RemoveTypeSelectorFromControls()
        {
            foreach (var item in Controls)
            {

                if (item.GetType() == typeof(TypeSelector))
                {
                    Controls.Remove((Control)item);

                    _API.IsTypeSelectorOpened = false;
                }
            }
        }
        private void MyDataGrid_LostFocus(object sender, EventArgs e)
        {
            RemoveEditorFromControls(true);
            _Editor = null;
            CustomInvalidate();
        }

        private void MyDataGrid_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0 && VerticalScrollBar.Value+VerticalScrollBar.SmallChange<VerticalScrollBar.Maximum)
            {
                VerticalScrollBar.Value += VerticalScrollBar.SmallChange;
            }
            if(e.Delta>0 && VerticalScrollBar.Value > VerticalScrollBar.Minimum)
            {
                VerticalScrollBar.Value -= VerticalScrollBar.SmallChange;
            }    
        }

        //!why internal?
        internal ObservableCollection<Column> Columns
        {
            get
            {
                return _API.Columns;
            }
        }
        [DisplayName(@"Source")]
        public object Source
        {
            get => _Source;
            set
            {
                _Source = (List<List<string>>)value;
                if (ColumnsAutoGeneretion)
                {
                    Columns.Clear();
                    _Bufer.Clear();
                    List<string> AggregatedObjectsFields = _Source.Select(k => k.First()).ToList();
                    List<string> selectedItems = new List<string>();
                    foreach (var item in AggregatedObjectsFields)
                    {

                        Columns.Add(new Column(item, typeof(string)) { Visible = true });

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
                    UpdateScrolls();
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

                UpdateScrolls();
            }
        }
        public void RemoveFromBufer()
        {
            string deletedHeaderText = "";
            var temp = _Bufer.First().Cells;
            foreach (var item in temp)
            {
                var columnExistedHeaders = _API.Columns.Where(k => k.HeaderText == item.Body).Select(K => K.HeaderText).ToList();
                if (columnExistedHeaders.Count < 0)
                {
                    deletedHeaderText = item.Body;
                    break;
                }
            }
            int index = GetXIndexInBufer(deletedHeaderText);          
            if (index != -1)
            {
                for (int i = 0; i < _Bufer.Count; i++)
                {
                    _Bufer[i].Cells.RemoveAt(index);
                }
            }
        }
        public void AddToBufer(string headerText)
        {
            int max = _Source.Max(k => k.Count);

            int count = _Bufer.Count;
            if (_Bufer.Count < max)
            {
                for (int i = 0; i < max - count; i++)
                {
                    _Bufer.Add(new Row());
                }
            }

            int index = -1;
            for (int j = 0; j < _Source.Count; j++)
            {
                if (headerText == _Source[j].First())
                {
                    index = j;
                }
            }
            if (index != -1)
            {
                for (int k = 0; k < _Source[index].Count; k++)
                {
                    Cell temp = new Cell();
                    temp.Body = _Source[index][k];
                    temp.SourceXIndex = index;
                    temp.SourceYIndex = k;
                    _Bufer[k].Cells.Add(temp);
                }
            }
        }
        public void RemoveColumnByName(string columnName)
        {
            for (int i = 0; i < _API.Columns.Count; i++)
            {
                if (_API.Columns[i].HeaderText == columnName)
                {
                    _API.Columns.RemoveAt(i);
                    break;
                }
            }
        }
        private void UpdateScrolls()
        {

            if (_API.Columns.Count > 0)
            {

                _TotalRowsCount = _Bufer.Count;
                _ViewPortRowsCount = (this.Height) / (RowHeight) - 1;
                if (_TableWidth > this.Width)
                {
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
                CustomInvalidate();
                        
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
        private void APIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SortDirection")
            {
                
                CustomInvalidate();
            }
        }
        private void ColumnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Visible")
            {
                RemoveEditorFromControls(false);
              
                _Editor = null;
                CustomInvalidate();
            }
        }
        private void ColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            RemoveEditorFromControls(false);
            _Editor = null;
            if (e.Action == NotifyCollectionChangedAction.Add)
            {

                AddToBufer(Columns.Last().HeaderText);
                for (int i = 0; i < _API.Columns.Count; i++)
                {
                    var a = _Bufer.First().Cells.Select(k => k.Body).ToList();
                    if (a.IndexOf(_API.Columns[i].HeaderText) == -1)
                    {

                        throw new Exception("Невозможно добавить колонкe отсутствую в источнике данных source");

                    }
                }
                UpdateScrolls();
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
                RemoveFromBufer();
            }
        }


        public void ChangeSorting(string columnName, Sort sortDirection)
        {
            RemoveEditorFromControls(false);          
            var newSortedColumn = Columns.Select(k => k).Where(u => u.HeaderText == columnName).ToList();
            foreach (var item in newSortedColumn)
            {
                if (item.Visible)
                {
                    _API.SortedColumnIndex = item.Index;
                    _API.SortDirection = sortDirection;                 
                    break;
                }
            }
        }
        private void RemoveHeaderFromControls()
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                Control item = Controls[i];
                if (item.GetType() == typeof(HeaderCell))
                {
                    Controls.Remove((Control)item);
                    i--;
                }
            }
            Header.Clear();
        }
        private void CalculateTotalTableWidth()
        {
            _TableWidth = 0;
            foreach (var APIColumn in _API.Columns)
            {
                int index = GetXIndexInBufer(APIColumn.HeaderText);              
                if (index != -1)
                {
                    var columnItems = _Bufer.Select(k => k.Cells[index].Body).ToList();
                    int columnWidth = (columnItems.Max(k => k.Length) > APIColumn.HeaderText.Length) ? columnItems.Max(k => k.Length) : APIColumn.HeaderText.Length;
                    APIColumn.Width = columnWidth;
                    if (APIColumn.Visible)
                    {
                        _TableWidth += (_LineWidth + _CellMinMargin + APIColumn.Width * (int)this.Font.Size + _CellMinMargin);
                    }
                }
            }
        }

        private void CustomInvalidate()
        {
            if (_Editor != null)
            {
                _Editor.Visible = false;
                _API.IsEditorOpened = true;
            }
            
            CalculateTotalTableWidth();
            SortBuferRows();
            Header = new List<HeaderCell>();
            RemoveHeaderFromControls();
           
            for (int i = 0; i < _API.Columns.Count; i++)
            {
                var a = _Bufer.First().Cells.Select(k => k.Body).ToList();
                if (a.IndexOf(_API.Columns[i].HeaderText) != -1)
                {                   
                    if (_API.Columns[i].Visible)
                    {
                       
                        HeaderCell Cell = new HeaderCell(_API.Columns[i]);
                        Cell.Font = this.Font;
                        Cell.Width = (int)(_LineWidth * 2 + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin);
                        Cell.Height = _RowHeight;
                        Cell.Location = new Point(0, 0);
                        Cell._API = _API;                       
                        Header.Add(Cell);
                        Controls.Add(Cell);
                    }                   

                }
            }
            foreach (var headerCell in Header)
            {
                List<HeaderCell> temp = new List<HeaderCell>();
                var OtherCells = Header.Select(k => k).Where(k => k != headerCell).ToList();
                headerCell.NeighborCells.Clear();
                headerCell.NeighborCells.AddRange(OtherCells);
            }
            if (_Editor != null)
            {
                Controls.Add(_Editor.GetControl());
                _API.EditorComponentType = _Editor.GetComponentType();              
                _API.IsEditorNedded = true;
                _Editor.Visible = true;
                _Editor.SetFocus();               
                _Editor.GetControl().Leave += Editor_Leave;
            }
            Invalidate();
        }

        private void Editor_Leave(object sender, EventArgs e)
        {
            if (!_Editor.Dropchanges)
            {
                _Editor.BuferCell.Body = _Editor.Value;
            }
            else
            {
                _Editor.BuferCell.Body = _Editor.OriginalValue;               
            }          
            _API.IsEditorOpened = false;
            if (_Editor.Closed)
            {
                _API.IsEditorNedded = false;
            }
            UpdateColumnsPosition();
            UpdateHeaderWidth();          
            CalculateTotalTableWidth();
            if (_API.SortDirection != Sort.None)
            {
                SortBuferRows(); 
            }
            Invalidate();           
        }
        private void SortBuferRows()
        {
            if (_Bufer.Count > 0)
            {
                Row firstRowBufer = new Row();
                firstRowBufer = _Bufer.First();
                int sortedIndex = -1;
                if (_API.SortedColumnIndex != -1 && _API.Columns.Count > 0)
                {
                    sortedIndex = GetXIndexInBufer(_API.Columns[_API.SortedColumnIndex].HeaderText);

                }
                _Bufer.RemoveAt(0);
                if (_API.SortDirection != Sort.None)
                {
                    RowComparer u = (_API.SortDirection == Sort.ASC) ? new RowComparer(true, sortedIndex, _API.Columns[_API.SortedColumnIndex].Type) : new RowComparer(false, sortedIndex, _API.Columns[_API.SortedColumnIndex].Type);
                    _Bufer.Sort(u);
                }
                else if (_API.SortDirection == Sort.None)
                {
                    if (_Bufer.First().Cells.Count > 0)
                    {
                        _Bufer.Sort((a, b) => a.Cells.First().SourceYIndex.CompareTo(b.Cells.First().SourceYIndex));
                    }
                }
                _Bufer.Insert(0, firstRowBufer);

            }
        }
        private void UpdateHeaderWidth()
        {
            for (int i = 0; i < _API.Columns.Count; i++)
            {

                if (_API.Columns[i].Visible)
                {
                    var List = Header.Select(k => k).Where(k => k.ColumnData.Equals(_API.Columns[i])).ToList();
                    foreach (var item in List)
                    {
                        item.Width = (int)(_LineWidth * 2 + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin);
                    }
                }
            }
        }
        
        private int GetXIndexInBufer(string item)
        {
            int index = -1;
            for (int i = 0; i < _Bufer.First().Cells.Count; i++)
            {
                if (item == _Bufer.First().Cells[i].Body)
                {
                    index = i;                  
                }
            }
            return index;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            _Pen.Color = LineColor;           
            UpdateHorizontalScroll();         
            e.Graphics.DrawLine(_Pen, 0, 0, this.Width, 0);
            e.Graphics.DrawLine(_Pen, this.Width, 0, this.Width, this.Height);
            e.Graphics.DrawLine(_Pen, this.Width, this.Height, 0, this.Height);
            e.Graphics.DrawLine(_Pen, 0, this.Height, 0, 0);
            if (_Bufer.Count > 0)
            {
                if (_Editor != null)
                {
                    if (_API.IsEditorNedded == false)
                    {
                        Controls.Remove(_Editor.GetControl());
                        _Editor = null;
                    }
                }
                int xCounterForLine = 0;               
                
                if (_ViewPortRowsCount > _Bufer.Count - 1)
                {
                    _ViewPortRowsCount = _Bufer.Count - 1;
                }                                    
                int xCounter = 0;
                int xCounterForText = _LineWidth + _CellMinMargin;
                for (int i = 0; i < _API.Columns.Count; i++)
                {
                  
                    int bufferRowIndex = _FirstPrintedRowIndex + 1;
                    int viewPortRowIndex = 1;
                    if (_API.Columns[i].Visible)
                    {
                        var ColumnItems = _Bufer.First().Cells.Select(k => k.Body).ToList();
                        var HeaderCell = Header.Select(k => k).Where(u => u.ColumnData.Equals(_API.Columns[i])).Single();
                        HeaderCell.Location = new Point(xCounter - HorisontalScrollBar.Value, 0);
                        _API.Columns[i].XStartPosition = xCounter - HorisontalScrollBar.Value;
                        xCounter += _LineWidth + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin;
                        _API.Columns[i].XEndPosition = xCounter - HorisontalScrollBar.Value - 1;

                        for (int j = 0; j < _ViewPortRowsCount; j++)
                        {
                            if (bufferRowIndex < _Bufer.Count)
                            {
                               
                                int index = GetXIndexInBufer(_API.Columns[i].HeaderText);                             
                                Cell TempCell = _Bufer[bufferRowIndex].Cells[index];
                                e.Graphics.DrawString(TempCell.Body, this.Font, _Brush, xCounterForText - HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex) + (RowHeight - FontHeight) / 2);
                                e.Graphics.DrawLine(_Pen, -HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex + 1), _TableWidth - HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex + 1));
                                viewPortRowIndex++;
                                bufferRowIndex++;

                            }
                        }
                        e.Graphics.DrawLine(_Pen, xCounterForLine - HorisontalScrollBar.Value, 0, xCounterForLine - HorisontalScrollBar.Value, RowHeight * (_ViewPortRowsCount + 1));
                        xCounterForLine += _LineWidth + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin;
                        xCounterForText += _API.Columns[i].Width * (int)Font.Size + _CellMinMargin + _LineWidth + _CellMinMargin;
                    }
                }
                         
                e.Graphics.DrawLine(_Pen, _TableWidth - HorisontalScrollBar.Value, 0, _TableWidth - HorisontalScrollBar.Value, 0 + RowHeight * (_ViewPortRowsCount + 1));
                e.Graphics.DrawLine(_Pen, 0 - HorisontalScrollBar.Value, RowHeight, _TableWidth - HorisontalScrollBar.Value, RowHeight);
               
            }
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
        private void UpdateColumnsPosition()
        {
           
            int xCounter = 0;
            int xCounterForText = _LineWidth + _CellMinMargin;
            for (int i = 0; i < _API.Columns.Count; i++)
            {
                var a = _Bufer.First().Cells.Select(k => k.Body).ToList();
                if (a.IndexOf(_API.Columns[i].HeaderText) != -1)
                {
                    int index = GetXIndexInBufer(_API.Columns[i].HeaderText);                   
                    if (_API.Columns[i].Visible)
                    {
                        _API.Columns[i].XStartPosition = xCounter - HorisontalScrollBar.Value;
                        var columnItems = _Bufer.Select(k => k.Cells[index].Body).ToList();
                        int columnWidth = (columnItems.Max(k => k.Length) > _API.Columns[i].HeaderText.Length) ? columnItems.Max(k => k.Length) : _API.Columns[i].HeaderText.Length;
                        _API.Columns[i].Width = columnWidth;
                        xCounter += _LineWidth + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin;
                        _API.Columns[i].XEndPosition = xCounter - HorisontalScrollBar.Value - 1;
                    }
                }

            }
        }
        private void MyDataGrid_Resize(object sender, EventArgs e)
        {
            if (_Bufer.Count != 0)
            {
                _ViewPortRowsCount = this.Height / (RowHeight) - 1;
                if (_TableWidth > this.Width - VerticalScrollBar.Width)
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
            Invalidate();
        }
        private void VScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            _FirstPrintedRowIndex = VerticalScrollBar.Value / _VerticalScrollValueRatio;          
            if (VerticalScrollBar.Value < 0)
            {
                _FirstPrintedRowIndex = 0;
            }
            if (_Editor != null)
            {               
                _Editor.Location = new Point(_Editor.Location.X, _Editor.DefaultPosition.Y-_FirstPrintedRowIndex*RowHeight);
                _Editor.SetFocus();

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

            if (_Editor != null)
            {
                var item = _API.Columns.Select(k => k).Where(k => k.Index == _Editor.ColumnIndex).Single();
                int xstart = item.XStartPosition;
               int xend = item.XEndPosition;
                _Editor.Location = new Point(xstart + _LineWidth, _Editor.Location.Y);
                _Editor.Width = item.XEndPosition - item.XStartPosition;
                _Editor.SetFocus();
               
            }
            Invalidate();
        }
      
     
        private void MyDataGrid_MouseClick(object sender, MouseEventArgs e)
        {

            RemoveTypeSelectorFromControls();
            var X = e.Location.X;
            var Y = e.Location.Y;
            int YIndex ;
            int XIndex ;
         
            if (Y / RowHeight < _Bufer.Count)
            {
                int xend = 0;
                foreach (var item in _API.Columns)
                {
                    int xstart;
                    if (item.Visible)
                    {
                        xstart = item.XStartPosition;
                        xend = item.XEndPosition;
                        if (xstart < X && X < xend)
                        {
                            YIndex = Y / RowHeight + _FirstPrintedRowIndex;
                            XIndex = GetXIndexInBufer(item.HeaderText);
                            if (_Editor != null)
                            {
                                Controls.Remove(_Editor.GetControl());
                                _Editor = null;
                                UpdateColumnsPosition();
                            }                            
                            EditorSelector es = new EditorSelector(_Bufer[YIndex].Cells[XIndex], item.Type);                            
                            xstart = item.XStartPosition;
                            xend = item.XEndPosition;
                            es.CreateEditor();
                            es.OriginalValue= _Bufer[YIndex].Cells[XIndex].Body;
                            es.Font = this.Font;
                            es.Width = item.XEndPosition - item.XStartPosition;
                            es.DefaultPosition = new Point(xstart + _LineWidth, _RowHeight * YIndex + _LineWidth);
                            es.Location = new Point(xstart + _LineWidth, _RowHeight * YIndex + _LineWidth-_FirstPrintedRowIndex*RowHeight);
                            es.Height = RowHeight - _LineWidth;
                            es.ColumnIndex = item.Index;
                            _Editor = es;                          
                            break;
                        }
                    }
                    xstart = xend;
                }
                CustomInvalidate();
            }
            else
            {
                RemoveEditorFromControls(false);
                RemoveTypeSelectorFromControls();
            }

        }
    }
    class Cell
    {
        public string Body { get; set; }
        public int SourceXIndex { get; set; }
        public int SourceYIndex { get; set; }

    }
}








