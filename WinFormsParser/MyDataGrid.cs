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
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Parser
{

    [System.ComponentModel.DesignerCategory("Code")]


    public partial class MyDataGrid : UserControl
    {

        List<List<string>> _Source;
        IQueryable<object> _ItemsSource;
        List<Page> _Pages = new List<Page>();
        private int _CuurentPageNumber;
        public string PrivateKeyColumn { get; set; }

        public IEnumerable<object> ItemsSource
        {
            get
            { return _ItemsSource; }
            set
            {
                _ItemsSource = value.AsQueryable();    
                
                _TotalRowsCount = _ItemsSource.Count();
                int pagesCount = (int)(Math.Ceiling(Convert.ToDecimal(_TotalRowsCount / BuferSize)));

                
                Dictionary<string, Type> columnsInfo = GetColumnsInfo();
                _ViewPortRowsCount = (this.Height) / (RowHeight) - 1;
                for (int i = 0; i < pagesCount; i++)
                {
                    Page temp = new Page();
                    temp.Number = i + 1;
                    temp.StartIndex = i * BuferSize;
                    temp.EndIndex = temp.StartIndex + BuferSize;

                  
                    if (temp.Number == 1)
                    {
                        temp.SkipElementsCount = 0;
                        temp.TakeElementsCount = BuferSize;
                        temp.DownScrollValue = 0;
                        temp.UpScrollValue = BuferSize - _ViewPortRowsCount+1;
                    }
                    if (temp.Number > 1)
                    {
                        
                        int startPoint = _Pages[i-1].EndIndex - _ViewPortRowsCount;
                      
                        int takeCount = BuferSize + _ViewPortRowsCount - 1;
                        temp.SkipElementsCount = startPoint;
                        temp.TakeElementsCount = takeCount;
                        if (temp.Number == 2)
                        {
                            temp.DownScrollValue = _Pages[0].UpScrollValue;
                            temp.UpScrollValue = temp.DownScrollValue + BuferSize - 1;
                        }
                        else
                        {
                            temp.DownScrollValue = _Pages[i - 1].DownScrollValue + BuferSize;
                            temp.UpScrollValue =temp.DownScrollValue + BuferSize-1;
                        }
                    }
                    _Pages.Add(temp);
                    

                }
                _Source = GetStringSource(columnsInfo);
                _Page = _Pages.FirstOrDefault();
                if (ColumnsAutoGeneration)
                {
                    Columns.Clear();

                    foreach (var item in columnsInfo)
                    {
                        Columns.Add(new Column(item.Key, item.Value) { Visible = true });
                    }
                }
            }
        }
        private Page _Page;
        private int _OldScrollValue;
        List<Row> _Buffer;
        Source _API;
        EditorSelector _Editor;
        int _RowHeight;
        int _LineWidth = 1;
        int _FirstPrintedRowIndex = 0;
        int _VerticalScrollValueRatio = 10;
        int _HorisontalScrollSmallChangeValueRatio = 10;
        int __HorisontalScrollLargeChangeValueRatio;
        int _CellMinMargin = 2;
        int _TotalRowsCount;
        int _ViewPortRowsCount;
        int _TableWidth;
        Brush _Brush;
        Pen _Pen;
        List<HeaderCell> Header;     
        public int BuferSize { get; set; } = 50;
        public event DataChangedHeandler DataChanged;
        public delegate void DataChangedHeandler(object sender, EventArgs eventArgs);
         public MyDataGrid()
        {


            AutoScaleMode = AutoScaleMode.None;
            InitializeComponent();
            _Source = new List<List<string>>();
            _Buffer = new List<Row>();
            _API = new Source();
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
            _CuurentPageNumber = 1;
           _Page = new Page();
            MouseWheel += DataGridMouseWheel;
            HorisontalScrollBar.MouseWheel += HorizontalScrollMouseWheel;
            Leave += MyDataGrid_LostFocus;

        }

       

        internal ObservableCollection<Column> Columns
        {
            get
            {
                return _API.Columns;
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

       

        private Dictionary<string, Type> GetColumnsInfo()
        {
            Dictionary<string, Type> columns = new Dictionary<string, Type>();
            var @object = _ItemsSource.FirstOrDefault();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(@object);
            foreach (PropertyDescriptor property in properties)
            {
                if (!columns.ContainsKey(property.Name))
                {
                    columns.Add(property.Name, property.PropertyType);
                }
            }

            return columns;

        }
        private List<List<string>> GetStringSource(Dictionary<string, Type> columns)
        {
            List<List<string>> StringSource = new List<List<string>>(BuferSize);
            var items = _ItemsSource.Take(BuferSize).ToList();
            foreach (var item in columns)
            {

                List<string> ColumnItems = new List<string>();
                ColumnItems.Add(item.Key);
             
                ColumnItems.AddRange(GetColumnItemsFromSource(item.Key, item.Value, items));
                StringSource.Add(ColumnItems);
            }
            return StringSource;
        }

        private void HorizontalScrollMouseWheel(object sender, MouseEventArgs e)
        {
           
            UpdateColumnsPosition();
            if (_Editor != null)
            {
                RemoveEditorFromControls(true);
            }        
            if (_API.IsTypeSelectorOpened)
            {
                var item = _API.Columns.Select(k => k).Where(k => k.Index == _API.TypeSelector.ColumnData.Index).Single();
                int xstart = item.XStartPosition;
                _API.TypeSelector.Location = new Point(xstart + 5, _API.TypeSelector.Location.Y);
            }
        }

        private void RemoveEditorFromControls(bool isDropChanges)
        {
            var editorControl = _Editor?.GetControl();
            if (editorControl != null)
            {
                Controls.Remove(editorControl);
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
            Controls.Remove(_API.TypeSelector);
        }
        private void MyDataGrid_LostFocus(object sender, EventArgs e)
        {
            RemoveEditorFromControls(true);
            _Editor = null;
            CustomInvalidate();
        }

        private void DataGridMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0 && VerticalScrollBar.Value + VerticalScrollBar.SmallChange < VerticalScrollBar.Maximum)
            {
                VerticalScrollBar.Value += VerticalScrollBar.SmallChange;
            }
            if (e.Delta > 0 && VerticalScrollBar.Value > VerticalScrollBar.Minimum)
            {
                if (VerticalScrollBar.Value >= VerticalScrollBar.SmallChange)
                {
                    VerticalScrollBar.Value -= VerticalScrollBar.SmallChange;
                    if (VerticalScrollBar.Value < VerticalScrollBar.SmallChange)
                    {
                        VerticalScrollBar.Value = VerticalScrollBar.Minimum;
                    }
                }

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
                    temp.BuferRowIndex = k;

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
                VerticalScrollBar.Maximum = ((_TotalRowsCount- _ViewPortRowsCount) * _VerticalScrollValueRatio) - 1;
                VerticalScrollBar.SmallChange = _VerticalScrollValueRatio;
                VerticalScrollBar.LargeChange = _VerticalScrollValueRatio;
                UpdateHorizontalScroll();
                HorisontalScrollBar.Value = 0;

                if (_TableWidth > this.Width)
                {
                    __HorisontalScrollLargeChangeValueRatio = _TableWidth - this.Width;
                }
                HorisontalScrollBar.SmallChange = _HorisontalScrollSmallChangeValueRatio;
                HorisontalScrollBar.LargeChange = __HorisontalScrollLargeChangeValueRatio;
                CustomInvalidate();
                _API.TypeSelector.Visible = false;

            }
        }
        private void APIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SortDirection")
            {
                SortData();
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

                        throw new Exception("Невозможно добавить колонку отсутствующую в источнике данных source");
                    }
                }
                UpdateScrolls();
                foreach (var item in Columns)
                {
                    item.PropertyChanged -= ColumnPropertyChanged;
                    item.PropertyChanged += ColumnPropertyChanged;
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
            if (!_Editor.CancelChanges && _Editor.IsValidated && _Editor.Value!=_Editor.OriginalValue)
            {
                Type sourceObjectsType = _ItemsSource.First().GetType();
                var tempObject = Activator.CreateInstance(sourceObjectsType);
                PropertyDescriptorCollection tempObjectProperties = TypeDescriptor.GetProperties(tempObject);
                Row bufeRow = _Buffer[_Editor.BufferCell.BuferRowIndex];
                foreach (Cell cell in bufeRow.Cells)
                {
                    var tempObjectProperty = tempObjectProperties.Find(_API.Columns[cell.SourceColumnIndex].HeaderText, false);
                    if (tempObjectProperty?.PropertyType == typeof(bool))
                    {
                        if (cell.Body == Resources.TrueValue)
                        {
                            tempObjectProperty.SetValue(tempObject, Convert.ChangeType(true, typeof(bool)));
                        }
                        else
                        {
                            tempObjectProperty.SetValue(tempObject, Convert.ChangeType(false, typeof(bool)));
                        }
                    }
                    else
                    {
                        tempObjectProperty?.SetValue(tempObject, Convert.ChangeType(cell.Body, tempObjectProperty.PropertyType));
                    }
                }
                var data = TooggleSorting(_Page.SkipElementsCount, _Page.TakeElementsCount).AsQueryable();
                var sourceObject = data.GetObjectWithMatchingProperties(tempObject);
                PropertyDescriptorCollection sourceObjectProperties = TypeDescriptor.GetProperties(sourceObject);

                var sourceObjectProperty = sourceObjectProperties.Find(_API.Columns[_Editor.BufferCell.SourceColumnIndex].HeaderText, false);
                if (sourceObjectProperty != null)
                {
                    sourceObjectProperty.SetValue(sourceObject, Convert.ChangeType(_Editor.Value, sourceObjectProperty.PropertyType));
                }

                if (_Editor.GetControl().GetType() == typeof(CheckBox))
                {
                    CheckBox control = (CheckBox)_Editor.GetControl();
                    if (control.Checked)
                    {
                        _Editor.BufferCell.Body = Resources.TrueValue;
                    }
                    else
                    {
                        _Editor.BufferCell.Body = Resources.FalseValue;
                    }
                }
                else
                {

                    _Editor.BufferCell.Body = _Editor.Value;
                }

                if (DataChanged != null)
                {
                    EventArgs eventArgs = new EventArgs();
                    DataChanged(this, eventArgs);
                }

            }
            else
            {
                if (_Editor.GetControl().GetType() == typeof(CheckBox))
                {
                    CheckBox control = (CheckBox)_Editor.GetControl();
                    if (control.Checked)
                    {
                        _Editor.BufferCell.Body = Resources.TrueValue;
                    }
                    else
                    {
                        _Editor.BufferCell.Body = Resources.FalseValue;
                    }
                }
                else
                {
                    _Editor.BufferCell.Body = _Editor.OriginalValue;
                }
            }
            _API.IsEditorOpened = false;
            if (_Editor.Closed)
            {
                _API.IsEditorUsed = false;
            }
            UpdateColumnsPosition();
            UpdateHeadersWidth();
            CalculateTotalTableWidth();
            if (_API.SortDirection != SortDirections.None && (_API.SortedColumnIndex==_Editor.ColumnIndex && _Editor.OriginalValue!=_Editor.Value))
            {
                SortData();

            }
            VerticalScrollBar.Maximum = ((_TotalRowsCount - _ViewPortRowsCount) * _VerticalScrollValueRatio) - 1;
            Invalidate();

        }
        private void SortData()
        {
           var items = TooggleSorting(_Page.SkipElementsCount, _Page.TakeElementsCount).ToList();         
            Dictionary<string, Type> columns = GetColumnsInfo();
            int index = 0;
            foreach (var item in columns)
            {
                List<string> ColumnItems = new List<string>();
                ColumnItems = GetColumnItemsFromSource(item.Key, item.Value, items);
                List<string> viewPortItems = new List<string>();
                var a = _Source[index].First();
                _Source[index].Clear();
                _Source[index].Add(a);
                _Source[index].AddRange(ColumnItems);
                _Source[index].AddRange(viewPortItems);
                index++;

            }
            _Buffer.Clear();
            for (int j = 0; j < _Source.Count; j++)
            {
                AddToBufer(_Source[j].First());
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

                                    cellBody = TempCell.Body;
                                }
                                else
                                {
                                    var text = TempCell.Body.Substring(0, b.HeaderText.Length * Convert.ToInt32(Resources.ReductionRatio)) + Resources.Ellipsis;
                                    if (text.Contains(Environment.NewLine))
                                    {
                                        text = text.Replace(Environment.NewLine, " ");
                                    }
                                    if (text.Contains("\r"))
                                    {
                                        text = text.Replace("\r", " ");
                                    }
                                    if (text.Contains("\n"))
                                    {
                                        text = text.Replace("\n", " ");
                                    }
                                    cellBody = text;
                                    TempCell.BodyToPrint = cellBody;
                                }

                                e.Graphics.DrawString(TempCell.BodyToPrint, this.Font, _Brush, xCounterForText - HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex) + (RowHeight - FontHeight) / 2);
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
                HorisontalScrollBar.Maximum = (int)(_TableWidth - viewportWidth + __HorisontalScrollLargeChangeValueRatio);

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
                VerticalScrollBar.Maximum = ((_TotalRowsCount - _ViewPortRowsCount) * _VerticalScrollValueRatio) - 1;

            }
            if (_Editor != null)
            {
                var viewportheight = this.Height;
                if (HorisontalScrollBar.Visible)
                {
                    viewportheight = viewportheight - HorisontalScrollBar.Height;
                    if (_Editor.Location.Y > (_ViewPortRowsCount+1)*RowHeight)
                    {

                        _Editor.Visible = false;
                    }
                }
                if (!HorisontalScrollBar.Visible)
                {
                 
                    if (_Editor.Location.Y > _ViewPortRowsCount * RowHeight)
                    {

                        _Editor.Visible = true;
                    }
                }
                var item = _API.Columns.Select(k => k).Where(k => k.Index == _Editor.ColumnIndex).Single();
                int xstart = item.XStartPosition;
                int xend = item.XEndPosition;
                _Editor.Location = new Point(xstart + _LineWidth, _Editor.Location.Y);
                if (_Editor.GetComponentType() == typeof(CheckBox))
                {
                    _Editor.Location = new Point(_Editor.Location.X + (xend - xstart) / 2 - _Editor.GetControl().Width / 2, _Editor.Location.Y);
                }
                if (_Editor.Location.Y + _Editor.Height > _TotalRowsCount * _RowHeight && _Editor.Location.Y + _Editor.Height > this.Height)
                {

                    _Editor.Location = new Point(_Editor.Location.X, _Editor.Location.Y - _Editor.Height + RowHeight - _LineWidth);
                }



            }
            if (_API.IsTypeSelectorOpened)
            {
                int xstart = _API.TypeSelector.ColumnData.XStartPosition;
                _API.TypeSelector.Location = new Point(xstart + 5, _API.TypeSelector.Location.Y);


            }
            UpdateHorizontalScroll();
            Invalidate();
        }
      
        private List<string> GetColumnItemsFromSource(string name, Type type, List<object>items)
        {

            List<string> ColumnItems = new List<string>();
            foreach (var @object in items)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(@object);
                var property = properties.Find(name, false);
                if (property != null)
                {
                    if (type == typeof(DateTime))
                    {
                        DateTime temp = (DateTime)property?.GetValue(@object);
                        string itemValue = temp.ToString(Resources.DefaultDataFormat);
                        ColumnItems.Add(itemValue);
                    }
                    else if (type == typeof(Boolean))
                    {
                        bool temp = (bool)property?.GetValue(@object);
                        if (temp)
                        {
                            ColumnItems.Add(Resources.TrueValue);
                        }
                        else
                        {
                            ColumnItems.Add(Resources.FalseValue);
                        }
                    }
                    else
                    {
                        ColumnItems.Add(property.GetValue(@object).ToString());
                    }
                }
                else
                {
                    ColumnItems.Add("");
                }
            }
            return ColumnItems;
        }







      

        bool IsScrolledDown = false;
        private IQueryable<object> TooggleSorting(int skipCount, int takeCount)
        {
            if (skipCount < 0)
            {
                skipCount = 0;
            }
            
            if (_API.SortedColumnIndex != -1)
            {
                if (_API.SortDirection == SortDirections.ASC)
                {
                   var items = _ItemsSource.OrderBy(_API.Columns[_API.SortedColumnIndex].HeaderText).Skip(skipCount).Take(takeCount).AsQueryable();
                    return items;
                }
                if (_API.SortDirection == SortDirections.DESC)
                {
                  var items = _ItemsSource.OrderByDescending(_API.Columns[_API.SortedColumnIndex].HeaderText).Skip(skipCount).Take(takeCount).AsQueryable();
                    return items;
                }
              
                else if (_API.SortDirection == SortDirections.None)
                {
                    var sortedColumn = _API.Columns.Select(k => k).Where(k => k.HeaderText == PrivateKeyColumn).FirstOrDefault();
                    var items = _ItemsSource.OrderBy(sortedColumn.HeaderText).Skip(skipCount).Take(takeCount).AsQueryable();
                    return items;
                }
            }
            else
            {
                var sortedColumn = _API.Columns.Select(k => k).Where(k => k.HeaderText == PrivateKeyColumn).FirstOrDefault();
                var  items = _ItemsSource.OrderBy(sortedColumn.HeaderText).Skip(skipCount).Take(takeCount).AsQueryable();
                return items;
            }
            return _ItemsSource;
        }

     
        private void VScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            UpdateColumnsPosition();
            UpdateHeadersWidth();
            CalculateTotalTableWidth();
            if (_Editor != null)
            {
                RemoveEditorFromControls(true);
            }
            _FirstPrintedRowIndex = VerticalScrollBar.Value / _VerticalScrollValueRatio;           
            Page selectedPage = new Page();
            if (_OldScrollValue > VerticalScrollBar.Value)
            {
                selectedPage = _Page;
                // selectedPage = _Pages.Where(k => k.UpScrollValue > (VerticalScrollBar.Value / _VerticalScrollValueRatio)).FirstOrDefault();

            }
            else
            {
               //   selectedPage = _Pages.Where(k => k.DownScrollValue < (VerticalScrollBar.Value / _VerticalScrollValueRatio)).LastOrDefault();
               selectedPage = _Page;
            }

            int aaaa = selectedPage.Number;
            if (_CuurentPageNumber > 1)           
            {
                _FirstPrintedRowIndex = _FirstPrintedRowIndex- (BuferSize* (_CuurentPageNumber - 1))+_ViewPortRowsCount+1;
            }
            int scrollOffset = 0;
            if (_OldScrollValue < VerticalScrollBar.Value)
            {
               //   _FirstPrintedRowIndex++;
               // if (_Editor != null)
              //  {
              //      _Editor.ScrollCounter++;
             //   }
                if (_CuurentPageNumber > 2)
                {
                   IsScrolledDown=true;
                }
               
            }
            else if (_OldScrollValue > VerticalScrollBar.Value)
            {
                //_FirstPrintedRowIndex--;
             //   if (_Editor != null)
              //  {
               //     _Editor.ScrollCounter--;
             //   }
                if (_CuurentPageNumber == 2)
                {
                    IsScrolledDown = false;
                }
            }            
            if (VerticalScrollBar.Value < 0)
            {
                _FirstPrintedRowIndex = 0;               
            }
            if (IsScrolledDown)
            {
                scrollOffset = 1;
            }

           
             if ((VerticalScrollBar.Value / _VerticalScrollValueRatio >= selectedPage.EndIndex-_ViewPortRowsCount))
            {             
                selectedPage = _Pages.Where(k => k.DownScrollValue <= (VerticalScrollBar.Value / _VerticalScrollValueRatio)).LastOrDefault();
                Dictionary<string, Type> columns = GetColumnsInfo();
               var printedPage = _Pages.Select(k => k).Where(k => k.Number == selectedPage.Number-1).Single();

             var items = TooggleSorting(printedPage.EndIndex - 1 - _ViewPortRowsCount, BuferSize + _ViewPortRowsCount).ToList();
                int i = 0;
          
                foreach (var item in columns)
                {
                    List<string> ColumnItems = new List<string>();
                    ColumnItems = GetColumnItemsFromSource(item.Key, item.Value, items);
                    var a = _Source[i].First();
                    _Source[i].Clear();
                    _Source[i].Add(a);
                    _Source[i].AddRange(ColumnItems);
                    i++;
                }
                _FirstPrintedRowIndex = 1;
                _Buffer.Clear();
                for (int j = 0; j < _Source.Count; j++)
                {
                    AddToBufer(_Source[j].First());
                }            
                _CuurentPageNumber = selectedPage.Number;       
               _Page = selectedPage;
                CalculateTotalTableWidth();
                UpdateHeadersWidth();
            }


            if (VerticalScrollBar.Value / _VerticalScrollValueRatio +_ViewPortRowsCount +scrollOffset <=selectedPage.StartIndex-1 && _OldScrollValue>VerticalScrollBar.Value)
            {
                scrollOffset =0;            
                
                int k = 0;
                if (_CuurentPageNumber > 2)
                {
                    k = 1;
                }
                Dictionary<string, Type> columns = GetColumnsInfo();
                selectedPage = _Pages.Where(s => s.UpScrollValue > (VerticalScrollBar.Value / _VerticalScrollValueRatio)).FirstOrDefault();
                var printedPage = selectedPage;
                _CuurentPageNumber = selectedPage.Number;
             //   if (_CuurentPageNumber > 1)
             //  {
              //      printedPage = _Pages.Select(kk => kk).Where(kk => kk.Number == selectedPage.Number - 1).Single();
              //  }
               
               
             
                var items = TooggleSorting(printedPage.EndIndex - BuferSize - _ViewPortRowsCount - k, BuferSize + _ViewPortRowsCount * k).ToList();
                int index = 0;
                foreach (var item in columns)
                {
                    

                    List<string> ColumnItems = new List<string>();
                  
                    ColumnItems = GetColumnItemsFromSource(item.Key, item.Value, items);
                  
                
                    List<string> viewPortItems = new List<string>();

                    var a = _Source[index].First();
                  
                    _Source[index].Clear();
                    _Source[index].Add(a);
                    _Source[index].AddRange(ColumnItems);
                    _Source[index].AddRange(viewPortItems);
                    index++;                  
                }
                if (_CuurentPageNumber < 2)
                {
                    _FirstPrintedRowIndex = BuferSize - _ViewPortRowsCount-1;
                }
                else
                {
                    _FirstPrintedRowIndex = BuferSize-1;
                }
                if (VerticalScrollBar.Value == 0)
                {
                    _FirstPrintedRowIndex = 0;

                }
                _Buffer.Clear();
                for (int j = 0; j < _Source.Count; j++)
                {
                    AddToBufer(_Source[j].First());
                }

              //  selectedPage = printedPage;
                _CuurentPageNumber = selectedPage.Number;
                _Page = selectedPage;
                CalculateTotalTableWidth();
                UpdateHeadersWidth();
            }

            if (_Editor != null)
            {
                var item = _API.Columns.Select(u => u).Where(u => u.Index == _Editor.ColumnIndex).Single();
                int xstart = item.XStartPosition;
                int xend = item.XEndPosition;

                if (_Editor.GetComponentType() != typeof(CheckBox))
                {
                    _Editor.Location = new Point(xstart + _LineWidth, _Editor.Location.Y);
                    _Editor.Width = item.XEndPosition - item.XStartPosition;
                 
                }
                _Editor.Location = new Point(xstart + _LineWidth, _Editor.DefaultPosition.Y - _Editor.ScrollCounter * RowHeight);
                if (_Editor.GetComponentType() == typeof(CheckBox))
                {
                    _Editor.Location = new Point(_Editor.Location.X+(xend-xstart)/2-_Editor.GetControl().Width/2, _Editor.Location.Y);
                }
                    _Editor.SetFocus();
                if (_Editor.Location.Y > (_ViewPortRowsCount + 1) * RowHeight)
                {
                    _Editor.Visible = false;
                }
                else
                {
                    _Editor.Visible = true;
                }

            }
          
            _OldScrollValue = VerticalScrollBar.Value;
           
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
                RemoveEditorFromControls(true);
            }         
            if (_API.IsTypeSelectorOpened)
            {
                var item = _API.Columns.Select(k => k).Where(k => k.Index == _API.TypeSelector.ColumnData.Index).Single();
               int xstart = item.XStartPosition;               
               _API.TypeSelector.Location = new Point(xstart + 5, _API.TypeSelector.Location.Y);            
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
            

            if (HitPointY  < (_ViewPortRowsCount+1 ) * RowHeight)
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

                            if (_Buffer[BuferRowIndex].Cells[BuferColumnIndex].Body.Length > item.HeaderText.Length * Convert.ToInt32(Resources.ReductionRatio) || _Buffer[BuferRowIndex].Cells[BuferColumnIndex].Body.Contains(Environment.NewLine))
                            {
                                editor.IsMultilain = true;

                            }
                            ColumnXStart = item.XStartPosition;
                            ColumnXEnd = item.XEndPosition;
                            editor.CreateControl();
                            if (editor.GetComponentType() == typeof(CheckBox))
                            {
                                if (_Buffer[BuferRowIndex].Cells[BuferColumnIndex].Body == Properties.Resources.FalseValue)
                                {
                                    editor.OriginalValue = "False";
                                }
                                else
                                {
                                    editor.OriginalValue = "True";
                                }
                            }
                            else
                            {
                                editor.OriginalValue = _Buffer[BuferRowIndex].Cells[BuferColumnIndex].Body;
                            }
                             editor.Font = this.Font;
                            editor.Width = item.XEndPosition - item.XStartPosition;
                            editor.DefaultPosition = new Point(ColumnXStart + _LineWidth, _RowHeight * BuferRowIndex + _LineWidth);                         
                            var viewportheight = this.Height;
                            if (HorisontalScrollBar.Visible)
                            {
                                viewportheight = viewportheight - HorisontalScrollBar.Height;
                            }                           
                            editor.Location = new Point(ColumnXStart + _LineWidth, _RowHeight * BuferRowIndex + _LineWidth-_FirstPrintedRowIndex*RowHeight);
                            if (editor.GetComponentType() == typeof(CheckBox))
                            {
                                editor.Location = new Point(ColumnXStart + _LineWidth + (ColumnXEnd + _LineWidth - ColumnXStart) / 2 - editor.GetControl().Width / 2, editor.Location.Y);
                            }
                            if (editor.IsMultilain)
                            {
                                editor.Height = RowHeight * 3 - _LineWidth;
                            }
                            else
                            {
                                editor.Height = RowHeight - _LineWidth;
                            }
                            if (editor.Location.Y + editor.Height > viewportheight && _TotalRowsCount - BuferRowIndex<=1)
                            {
                                 editor.Location = new Point(editor.Location.X, editor.Location.Y - editor.Height+RowHeight-_LineWidth);                                                             
                            }
                            editor.ColumnIndex = item.Index;
                            _Editor = editor;
                            _Editor.ScrollCounter = _FirstPrintedRowIndex;
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








