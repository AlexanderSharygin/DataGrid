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
        List<Row> _Buffer;
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
        int _TableWidth;
        Brush _Brush;
        Pen _Pen;
        List<HeaderCell> Header;    
        public MyDataGrid()
        {

            AutoScaleMode = AutoScaleMode.None;
            InitializeComponent();           
            _Source = new List<List<string>>();
            _Buffer = new List<Row>();
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
            var editorControl = _Editor?.GetControl();
            if (editorControl != null)
            {
                Controls.Remove((Control)editorControl);
                if (!isDropChanges)
                {
                    _Editor.BufferCell.Body = _Editor.OriginalValue;
                }
                _Editor = null;
                _API.IsEditorUsed = false;
                _API.IsEditorOpened = false;
            }                    
        }
        private void RemoveTypeSelectorFromControls()
        {           
          Controls.Remove((Control)_API.TypeSelector);          
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
                if (ColumnsAutoGeneration)
                {
                    Columns.Clear();
                    _Buffer.Clear();
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
        public bool ColumnsAutoGeneration { get; set; } = false;
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
        private void RemoveFromBufer()
        {
            string deletedHeaderText = "";
            var headerCells = _Buffer.First().Cells;
            foreach (var item in headerCells)
            {
                var ExistedColumnHeaders = _API.Columns.Where(k => k.HeaderText == item.Body).Select(K => K.HeaderText).ToList();
                if (ExistedColumnHeaders.Count == 0)
                {
                    deletedHeaderText = item.Body;
                    break;
                }
            }
            int index = GetCellColumnIndexInBufer(deletedHeaderText);          
            if (index != -1)
            {
                for (int i = 0; i < _Buffer.Count; i++)
                {
                    _Buffer[i].Cells.RemoveAt(index);
                }
            }
        }
        private void AddToBufer(string headerText)
        {
            int max = _Source.Max(k => k.Count);
            int count = _Buffer.Count;
            if (_Buffer.Count < max)
            {
                for (int i = 0; i < max - count; i++)
                {
                    _Buffer.Add(new Row());
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
                    temp.SourceColumnIndex = index;
                    temp.SourceRowIndex = k;
                    _Buffer[k].Cells.Add(temp);
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
                _TotalRowsCount = _Buffer.Count;
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
                _API.TypeSelector.Visible = false;              

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
                    var a = _Buffer.First().Cells.Select(k => k.Body).ToList();
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


        public void ChangeSorting(string columnName, SortDirections sortDirection)
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
            for (int i = 0; i < _API.Columns.Count; i++)
            {
                var b = _API.Columns.Where(k => k.Index == i).Select(k => k).Single();
                int index = GetCellColumnIndexInBufer(b.HeaderText);              
                if (index != -1)
                {
                    var columnItems = _Buffer.Select(k => k.Cells[index].Body).ToList();
                    for (int j = 1; j <columnItems.Count; j++)
                    {                       
                            if (columnItems[j].Length < columnItems.First().Length * Convert.ToInt32(Resources.ReductionRatio))
                            {
                            continue;
                            }
                            else
                            {
                            columnItems[j] = columnItems[j].Substring(0, columnItems.First().Length * Convert.ToInt32(Resources.ReductionRatio)) + Resources.Ellipsis;
                            }                       
                    }                    
                    int columnWidth = (columnItems.Max(k => k.Length) > b.HeaderText.Length) ? columnItems.Max(k => k.Length) : b.HeaderText.Length;
                    b.Width = columnWidth;
                    if (b.Visible)
                    {
                        _TableWidth += (_LineWidth + _CellMinMargin + b.Width * (int)this.Font.Size + _CellMinMargin);
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
                var a = _Buffer.First().Cells.Select(k => k.Body).ToList();
                var b = _API.Columns.Where(k => k.Index == i).Select(k => k).Single();
                if (a.IndexOf(b.HeaderText) != -1)
                {                   
                    if (b.Visible)
                    {
                        HeaderCell Cell = new HeaderCell(b, _API)
                        {
                            Font = this.Font,
                            Width = (int)(_LineWidth * 2 + _CellMinMargin + b.Width * (int)this.Font.Size + _CellMinMargin),
                            Height = _RowHeight,
                            Location = new Point(0, 0)
                        };                                                  
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
                _API.EditorControl = _Editor.GetControl();
                _API.IsEditorUsed = true;
                _Editor.Visible = true;
                _Editor.SetFocus();               
                _Editor.GetControl().Leave += Editor_Leave;
            }
            Invalidate();
        }

        private void Editor_Leave(object sender, EventArgs e)
        {
            if (!_Editor.CancelChanges && _Editor.IsValidated)
            {
                _Editor.BufferCell.Body = _Editor.Value;
            }
            else
            {
                _Editor.BufferCell.Body = _Editor.OriginalValue;               
            }          
            _API.IsEditorOpened = false;
            if (_Editor.Closed)
            {
                _API.IsEditorUsed = false;
            }
            UpdateColumnsPosition();
            UpdateHeadersWidth();          
            CalculateTotalTableWidth();
            if (_API.SortDirection != SortDirections.None)
            {
                SortBuferRows(); 
            }
            Invalidate();           
        }
        private void SortBuferRows()
        {
            if (_Buffer.Count > 0)
            {
                Row firstRowBufer = new Row();
                firstRowBufer = _Buffer.First();
                int sortedIndex = -1;
                if (_API.SortedColumnIndex != -1 && _API.Columns.Count > 0)
                {
                    sortedIndex = GetCellColumnIndexInBufer(_API.Columns.Where(k=>k.Index==_API.SortedColumnIndex).Single().HeaderText);
                }
                _Buffer.RemoveAt(0);
                if (_API.SortDirection != SortDirections.None)
                {
                    RowComparer u = (_API.SortDirection == SortDirections.ASC) ? new RowComparer(true, sortedIndex, _API.Columns.Where(k => k.Index == _API.SortedColumnIndex).Single().DataType) : new RowComparer(false, sortedIndex, _API.Columns.Where(k => k.Index == _API.SortedColumnIndex).Single().DataType);
                    _Buffer.Sort(u);
                }
                else if (_API.SortDirection == SortDirections.None)
                {
                    if (_Buffer.First().Cells.Count > 0)
                    {
                        _Buffer.Sort((a, b) => a.Cells.First().SourceRowIndex.CompareTo(b.Cells.First().SourceRowIndex));
                    }
                }
                _Buffer.Insert(0, firstRowBufer);

            }
        }
        private void UpdateHeadersWidth()
        {
            for (int i = 0; i < _API.Columns.Count; i++)
            {
                var b = _API.Columns.Where(k => k.Index == i).Select(k => k).Single();
                if (b.Visible)
                {
                    var headers = Header.Select(k => k).Where(k => k.ColumnData.Equals(b)).ToList();
                    foreach (var item in headers)
                    {
                        item.Width = (int)(_LineWidth * 2 + _CellMinMargin + b.Width * (int)this.Font.Size + _CellMinMargin);
                    }
                }
            }
        }
        
        private int GetCellColumnIndexInBufer(string item)
        {
            int index = -1;
            for (int i = 0; i < _Buffer.First().Cells.Count; i++)
            {
                if (item == _Buffer.First().Cells[i].Body)
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
            if (_Buffer.Count > 0)
            {
                if (_Editor != null)
                {
                    if (_API.IsEditorUsed == false)
                    {
                        Controls.Remove(_Editor.GetControl());
                        _Editor = null;
                    }
                }
                int xCounterForLine = 0;                           
                if (_ViewPortRowsCount > _Buffer.Count - 1)
                {
                    _ViewPortRowsCount = _Buffer.Count - 1;
                }                                    
                int xCounterForColumns = 0;
                int xCounterForText = _LineWidth + _CellMinMargin;
                for (int i = 0; i < _API.Columns.Count; i++)
                {
                   
                    var b = _API.Columns.Where(k => k.Index == i).Select(k => k).Single();
                    int bufferRowIndex = _FirstPrintedRowIndex + 1;
                    int viewPortRowIndex = 1;
                    if (b.Visible)
                    {
                        var ColumnItems = _Buffer.First().Cells.Select(k => k.Body).ToList();
                        var HeaderCell = Header.Select(k => k).Where(u => u.ColumnData.Equals(b)).Single();
                        HeaderCell.Location = new Point(xCounterForColumns - HorisontalScrollBar.Value, 0);
                        b.XStartPosition = xCounterForColumns - HorisontalScrollBar.Value;
                        xCounterForColumns += _LineWidth + _CellMinMargin + b.Width * (int)this.Font.Size + _CellMinMargin;
                        b.XEndPosition = xCounterForColumns - HorisontalScrollBar.Value - 1;
                        for (int j = 0; j < _ViewPortRowsCount; j++)
                        {
                            if (bufferRowIndex < _Buffer.Count)
                            {                               
                                int index = GetCellColumnIndexInBufer(b.HeaderText);                             
                                Cell TempCell = _Buffer[bufferRowIndex].Cells[index];
                                string cellBody;
                                if (TempCell.Body.Length < b.HeaderText.Length * Convert.ToInt32(Resources.ReductionRatio))
                                {
                                    cellBody=TempCell.Body;
                                }
                                else
                                {
                                    cellBody = TempCell.Body.Substring(0, b.HeaderText.Length * Convert.ToInt32(Resources.ReductionRatio)) + Resources.Ellipsis;
                                }
                                e.Graphics.DrawString(cellBody, this.Font, _Brush, xCounterForText - HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex) + (RowHeight - FontHeight) / 2);
                                e.Graphics.DrawLine(_Pen, -HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex + 1), _TableWidth - HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex + 1));
                                viewPortRowIndex++;
                                bufferRowIndex++;
                                cellBody = "";

                            }
                        }
                        e.Graphics.DrawLine(_Pen, xCounterForLine - HorisontalScrollBar.Value, 0, xCounterForLine - HorisontalScrollBar.Value, RowHeight * (_ViewPortRowsCount + 1));
                        xCounterForLine += _LineWidth + _CellMinMargin + b.Width * (int)this.Font.Size + _CellMinMargin;
                        xCounterForText += b.Width * (int)Font.Size + _CellMinMargin + _LineWidth + _CellMinMargin;
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



      
        private void UpdateColumnsPosition()
        {
           
            int xCounterForColumns = 0;
            int xCounterForText = _LineWidth + _CellMinMargin;
            for (int i = 0; i < _API.Columns.Count; i++)
            {
                var b = _API.Columns.Where(k => k.Index == i).Select(k => k).Single();
                var HeadersTexts = _Buffer.First().Cells.Select(k => k.Body).ToList();
                if (HeadersTexts.IndexOf(b.HeaderText) != -1)
                {
                    int index = GetCellColumnIndexInBufer(b.HeaderText);                   
                    if (b.Visible)
                    {
                        b.XStartPosition = xCounterForColumns - HorisontalScrollBar.Value;
                        var columnItems = _Buffer.Select(k => k.Cells[index].Body).ToList();                       
                        for (int j = 1; j < columnItems.Count; j++)
                        {
                            if (columnItems[j].Length < columnItems.First().Length * Convert.ToInt32(Resources.ReductionRatio))
                            {
                                continue;
                            }
                            else
                            {
                                columnItems[j] = columnItems[j].Substring(0, columnItems.First().Length * Convert.ToInt32(Resources.ReductionRatio)) + Resources.Ellipsis;
                            }
                        }
                        int columnWidth = (columnItems.Max(k => k.Length) > b.HeaderText.Length) ? columnItems.Max(k => k.Length) : b.HeaderText.Length;
                        b.Width = columnWidth;
                        xCounterForColumns += _LineWidth + _CellMinMargin + b.Width * (int)this.Font.Size + _CellMinMargin;
                        b.XEndPosition = xCounterForColumns - HorisontalScrollBar.Value - 1;
                    }
                }

            }
        }
        private void MyDataGrid_Resize(object sender, EventArgs e)
        {
            if (_Buffer.Count != 0)
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
            var HitPointX = e.Location.X;
            var HitPointY = e.Location.Y;
            int BuferRowIndex;
            int BuferColumnIndex;
         
            if (HitPointY / RowHeight < _Buffer.Count)
            {
                int ColumnXEnd = 0;
                foreach (var item in _API.Columns)
                {
                    int ColumnXStart;
                    if (item.Visible)
                    {
                        ColumnXStart = item.XStartPosition;
                        ColumnXEnd = item.XEndPosition;
                        if (ColumnXStart < HitPointX && HitPointX < ColumnXEnd)
                        {
                            BuferRowIndex = HitPointY / RowHeight + _FirstPrintedRowIndex;
                            BuferColumnIndex = GetCellColumnIndexInBufer(item.HeaderText);
                            if (_Editor != null)
                            {
                                Controls.Remove(_Editor.GetControl());
                                _Editor = null;
                                UpdateColumnsPosition();
                            }                            
                            EditorSelector editor = new EditorSelector(_Buffer[BuferRowIndex].Cells[BuferColumnIndex], item.DataType, item.DataFormat);                            
                            ColumnXStart = item.XStartPosition;
                            ColumnXEnd = item.XEndPosition;
                            editor.CreateControl();
                            editor.OriginalValue= _Buffer[BuferRowIndex].Cells[BuferColumnIndex].Body;
                            editor.Font = this.Font;
                            editor.Width = item.XEndPosition - item.XStartPosition;
                            editor.DefaultPosition = new Point(ColumnXStart + _LineWidth, _RowHeight * BuferRowIndex + _LineWidth);
                            editor.Location = new Point(ColumnXStart + _LineWidth, _RowHeight * BuferRowIndex + _LineWidth-_FirstPrintedRowIndex*RowHeight);
                            editor.Height = RowHeight - _LineWidth;
                            editor.ColumnIndex = item.Index;
                            _Editor = editor;                          
                            break;
                        }
                    }
                    ColumnXStart = ColumnXEnd;
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
    
}








