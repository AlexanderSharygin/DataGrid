﻿using System;
using System.Collections.Generic;
using Parser.Properties;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;


namespace Parser
{

   [System.ComponentModel.DesignerCategory("Code")]


    public partial class MyDataGrid : UserControl
    {
      
        List<List<string>> _Source;
        IQueryable<object> _ItemsSource;
        List<Page> _Pages = new List<Page>();
       private int _CuurentPageNumber;
        private Page _CurrentPage;
        private int _OldScrollValue;
        List<Row> _Buffer;
        Source _API;
        EditorSelector _Editor;
        int _RowHeight;
        int _LineWidth = 1;
        int _FirstPrintedRowIndex = 0;
        int _VerticalScrollValueRatio = 10;
        int _HorisontalScrollSmallChangeValueRatio = 10;
        int _HorisontalScrollLargeChangeValueRatio;
        int _CellMinMargin = 2;
        int _TotalRowsCount;
        int _ViewPortRowsCount;
        int _TableWidth;
        Brush _Brush;
        Pen _Pen;
        List<HeaderCell> _Header;
        bool _ViewPortIsScrolledDown = false;
       CancellationTokenSource _CancellationTokenSource;      
   
        public string PrivateKeyColumn { get; set; }
        public IEnumerable<object> ItemsSource
        {
            get
            { return _ItemsSource; }
            set
            {
                _ItemsSource = value.AsQueryable();

                //  _Pages.Clear();
                //  _TotalRowsCount =  _ItemsSource.Count();
                //  int pagesCount = (int)(Math.Ceiling(Convert.ToDecimal(_TotalRowsCount / BuferSize)));
                //  if (pagesCount == 0)
                //  {
                //      pagesCount = 1;
                //  }
                Dictionary<string, Type> columnsInfo = GetColumnsInfo();
                _ViewPortRowsCount = (this.Height) / (RowHeight) - 1;
                //   for (int i = 0; i < pagesCount; i++)
                //  {
                //       _Pages.Add(CreateNewPage(i));
                //   }
                _Source = GetStringDataSource(columnsInfo);
                AsyncGetCount();
                //   _CurrentPage = _Pages.FirstOrDefault();
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
        public int BuferSize { get; set; } = 50;
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
        public event DataChangedHeandler DataChanged;
        public delegate void DataChangedHeandler(object sender, EventArgs eventArgs);       
        public MyDataGrid()
        {
            AutoScaleMode = AutoScaleMode.None;
            InitializeComponent();
            _Source = new List<List<string>>();
            _Buffer = new List<Row>();
            _API = new Source();
            _Brush = new SolidBrush(ForeColor);
            _Pen = new Pen(LineColor, _LineWidth);
            _CuurentPageNumber = 1;
            _CurrentPage = new Page();
            _API.Event_PropertyChanged += APIPropertyChanged;
       
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
            MouseWheel += DataGridMouseWheel;
            HorisontalScrollBar.MouseWheel += HorizontalScrollMouseWheel;
            Leave += LostFocus;
        }    
     
        private Page CreateNewPage(int number)
        {
            Page tempPage = new Page();
            tempPage.Number = number + 1;
            tempPage.StartIndex = number * BuferSize;
            tempPage.EndIndex = tempPage.StartIndex + BuferSize;
            if (tempPage.Number == 1)
            {
                tempPage.SkipElementsCount = 0;
                tempPage.TakeElementsCount = BuferSize;
                tempPage.DownScrollValue = 0;
                tempPage.UpScrollValue = BuferSize - _ViewPortRowsCount + 1;
            }
            if (tempPage.Number > 1)
            {
                int startPoint = _Pages[number - 1].EndIndex - _ViewPortRowsCount;
                int takeCount = BuferSize + _ViewPortRowsCount - 1;
                tempPage.SkipElementsCount = startPoint;
                tempPage.TakeElementsCount = takeCount;
                if (tempPage.Number == 2)
                {
                    tempPage.DownScrollValue = _Pages[0].UpScrollValue;
                    tempPage.UpScrollValue = tempPage.DownScrollValue + BuferSize - 1;
                }
                else
                {
                    tempPage.DownScrollValue = _Pages[number - 1].DownScrollValue + BuferSize;
                    tempPage.UpScrollValue = tempPage.DownScrollValue + BuferSize - 1;
                }
            }
            return tempPage;
        }
        private Dictionary<string, Type> GetColumnsInfo()
        {
            Dictionary<string, Type> columns = new Dictionary<string, Type>();               
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(_ItemsSource.ElementType);
            foreach (PropertyDescriptor property in properties)
            {
                if (!columns.ContainsKey(property.Name))
                {
                    columns.Add(property.Name, property.PropertyType);
                }
            }
            return columns;
        }
        private List<List<string>> GetStringDataSource(Dictionary<string, Type> columns)
        {
            List<List<string>> stringSource = new List<List<string>>(BuferSize);
            var dataSource = _ItemsSource.Take(BuferSize).ToList();
            foreach (var column in columns)
            {
                List<string> ColumnItems = new List<string>();
                ColumnItems.Add(column.Key);
                ColumnItems.AddRange(GetColumnItemsFromSource(column.Key, column.Value, dataSource));
                stringSource.Add(ColumnItems);
            }
            return stringSource;
        }
      //-------
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
            _Header.Clear();
        }
        private void Editor_Leave(object sender, EventArgs e)
        {
            if (!_Editor.CancelChanges && _Editor.IsValidated && _Editor.Value != _Editor.OriginalValue)
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
                        tempObjectProperty.SetValue(tempObject, Convert.ChangeType(ConvertBuuferValueToBoolView(cell), typeof(bool)));
                    }
                    else
                    {
                        tempObjectProperty?.SetValue(tempObject, Convert.ChangeType(cell.Body, tempObjectProperty.PropertyType));
                    }
                }

                var sourceeData = TooggleSorting(_CurrentPage.SkipElementsCount, _CurrentPage.TakeElementsCount).AsQueryable();
                var sourceObject = sourceeData.GetObjectWithEqualProperties(tempObject);
                PropertyDescriptorCollection sourceObjectProperties = TypeDescriptor.GetProperties(sourceObject);
                var sourceObjectProperty = sourceObjectProperties.Find(_API.Columns[_Editor.BufferCell.SourceColumnIndex].HeaderText, false);
                if (sourceObjectProperty != null)
                {
                    sourceObjectProperty.SetValue(sourceObject, Convert.ChangeType(_Editor.Value, sourceObjectProperty.PropertyType));
                }
                if (_Editor.GetControl().GetType() == typeof(CheckBox))
                {
                    SetBoolValueInBuffer();
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
                    SetBoolValueInBuffer();
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
            RecalculateTotalTableWidth();
            if (_API.SortDirection != SortDirections.None && (_API.SortedColumnIndex == _Editor.ColumnIndex && _Editor.OriginalValue != _Editor.Value))
            {
                SortData();
            }
            VerticalScrollBar.Maximum = ((_TotalRowsCount - _ViewPortRowsCount) * _VerticalScrollValueRatio) - 1;
            Invalidate();
        }


        //------------
        private void RemoveFromBufer()
        {
            string deletedColumnHeaderText = "";
            var headerCells = _Buffer.First().Cells;
            foreach (var item in headerCells)
            {
                var ExistedColumnHeaders = _API.Columns.Where(k => k.HeaderText == item.Body).Select(K => K.HeaderText).ToList();
                if (ExistedColumnHeaders.Count == 0)
                {
                    deletedColumnHeaderText = item.Body;
                    break;
                }
            }
            int index = GetColumnIndexInBufer(deletedColumnHeaderText);
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
            int maxColumnLengtj = _Source.Max(k => k.Count);
            int currentBufferCount = _Buffer.Count;
            if (_Buffer.Count < maxColumnLengtj)
            {
                for (int i = 0; i < maxColumnLengtj - currentBufferCount; i++)
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
      
        private void RecalculateTotalTableWidth()
        {
            _TableWidth = 0;
            for (int i = 0; i < _API.Columns.Count; i++)
            {
                var column = _API.Columns.Where(k => k.Index == i).Select(k => k).Single();
                int index = GetColumnIndexInBufer(column.HeaderText);
                if (index != -1)
                {
                    var columnItemsFroBufer = _Buffer.Select(k => k.Cells[index].Body).ToList();
                    for (int j = 1; j < columnItemsFroBufer.Count; j++)
                    {
                        if (columnItemsFroBufer[j].Length < columnItemsFroBufer.First().Length * Convert.ToInt32(Resources.ReductionRatio))
                        {
                            continue;
                        }
                        else
                        {
                            columnItemsFroBufer[j] = columnItemsFroBufer[j].Substring(0, columnItemsFroBufer.First().Length * Convert.ToInt32(Resources.ReductionRatio)) + Resources.Ellipsis;
                        }
                    }
                    int columnWidth = (columnItemsFroBufer.Max(k => k.Length) > column.HeaderText.Length) ? columnItemsFroBufer.Max(k => k.Length) : column.HeaderText.Length;
                    column.Width = columnWidth;
                    if (column.Visible)
                    {
                        _TableWidth += (_LineWidth + _CellMinMargin + column.Width * (int)this.Font.Size + _CellMinMargin);
                    }
                }
            }
        }
        private void SetBoolValueInBuffer()
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
        private bool ConvertBuuferValueToBoolView(Cell bufferCell)
        {
            bool result = false;
            if (bufferCell.Body == Resources.TrueValue)
            {
                result = true;
            }
            return result;
        }

        private List<string> GetColumnItemsFromSource(string columnName, Type type, List<object> source)
        {

            List<string> columnItems = new List<string>();
            foreach (var item in source)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
                var property = properties.Find(columnName, false);
                if (property != null)
                {
                    if (type == typeof(DateTime))
                    {
                        DateTime temp = (DateTime)property?.GetValue(item);
                        string itemValue = temp.ToString(Resources.DefaultDataFormat);
                        columnItems.Add(itemValue);
                    }
                    else if (type == typeof(Boolean))
                    {
                        bool temp = (bool)property?.GetValue(item);
                        if (temp)
                        {
                            columnItems.Add(Resources.TrueValue);
                        }
                        else
                        {
                            columnItems.Add(Resources.FalseValue);
                        }
                    }
                    else
                    {
                        columnItems.Add(property.GetValue(item).ToString());
                    }
                }
                else
                {
                    columnItems.Add("");
                }
            }
            return columnItems;
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
                if (_Buffer.Count > 0)
                {
                    var columnsInBuffer = _Buffer.First().Cells.Select(k => k.Body).ToList();
                    if (columnsInBuffer.IndexOf(Columns.Last().HeaderText) == -1)
                    {
                        AddToBufer(Columns.Last().HeaderText);
                    }
                }
                else
                {
                    AddToBufer(Columns.Last().HeaderText);
                }
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
        private int GetColumnIndexInBufer(string columnName)
        {
            int index = -1;
            for (int i = 0; i < _Buffer.First().Cells.Count; i++)
            {
                if (columnName == _Buffer.First().Cells[i].Body)
                {
                    index = i;
                }
            }
            return index;
        }
        private void CreateHeaderItem(Column column)
        {
            HeaderCell Cell = new HeaderCell(column, _API)
            {
                Font = this.Font,
                Width = (int)(_LineWidth * 2 + _CellMinMargin + column.Width * (int)this.Font.Size + _CellMinMargin),
                Height = _RowHeight,
                Location = new Point(0, 0)
            };
            _Header.Add(Cell);
            Controls.Add(Cell);
        }

        //-----

        private void SortData()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var sortedItems = TooggleSorting(_CurrentPage.SkipElementsCount, _CurrentPage.TakeElementsCount).ToList();
            Dictionary<string, Type> columns = GetColumnsInfo();
            int index = 0;
            foreach (var item in columns)
            {
                List<string> ColumnItems = new List<string>();
                ColumnItems = GetColumnItemsFromSource(item.Key, item.Value, sortedItems);
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
                    var items = _ItemsSource?.OrderBy(_API.Columns[_API.SortedColumnIndex].HeaderText).Skip(skipCount).Take(takeCount).AsQueryable();
                    return items;
                }
                if (_API.SortDirection == SortDirections.DESC)
                {
                    var items = _ItemsSource?.OrderByDescending(_API.Columns[_API.SortedColumnIndex].HeaderText).Skip(skipCount).Take(takeCount).AsQueryable();
                    return items;
                }

                else if (_API.SortDirection == SortDirections.None)
                {
                    var sortedColumn = _API.Columns.Select(k => k).Where(k => k.HeaderText == PrivateKeyColumn).FirstOrDefault();
                    var items = _ItemsSource?.OrderBy(sortedColumn.HeaderText).Skip(skipCount).Take(takeCount).AsQueryable();
                    return items;
                }
            }
            else
            {
                var sortedColumn = _API.Columns.Select(k => k).Where(k => k.HeaderText == PrivateKeyColumn).FirstOrDefault();
                var items = _ItemsSource?.OrderBy(sortedColumn.HeaderText).Skip(skipCount).Take(takeCount).AsQueryable();
                return items;
            }
            return _ItemsSource;
        }
//----------------
        private void UpdateHeadersWidth()
        {
            for (int i = 0; i < _API.Columns.Count; i++)
            {
                var column = _API.Columns.Where(k => k.Index == i).Select(k => k).Single();
                if (column.Visible)
                {
                    var headers = _Header.Select(k => k).Where(k => k.ColumnData.Equals(column)).ToList();
                    foreach (var item in headers)
                    {
                        item.Width = (int)(_LineWidth * 2 + _CellMinMargin + column.Width * (int)this.Font.Size + _CellMinMargin);
                    }
                }
            }
        }     
        private void UpdateColumnsPosition()
        {
            int xCounterForColumns = 0;
            int xCounterForText = _LineWidth + _CellMinMargin;
            for (int i = 0; i < _API.Columns.Count; i++)
            {
                var column = _API.Columns.Where(k => k.Index == i).Select(k => k).Single();
                var HeadersTexts = _Buffer.First().Cells.Select(k => k.Body).ToList();
                if (HeadersTexts.IndexOf(column.HeaderText) != -1)
                {
                    int index = GetColumnIndexInBufer(column.HeaderText);
                    if (column.Visible)
                    {
                        column.XStartPosition = xCounterForColumns - HorisontalScrollBar.Value;
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
                        int columnWidth = (columnItems.Max(k => k.Length) > column.HeaderText.Length) ? columnItems.Max(k => k.Length) : column.HeaderText.Length;
                        column.Width = columnWidth;
                        xCounterForColumns += _LineWidth + _CellMinMargin + column.Width * (int)this.Font.Size + _CellMinMargin;
                        column.XEndPosition = xCounterForColumns - HorisontalScrollBar.Value - 1;
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
            RecalculateTotalTableWidth();
            _Header = new List<HeaderCell>();
            RemoveHeaderFromControls();
            for (int i = 0; i < _API.Columns.Count; i++)
            {
                var columnsInBufer = _Buffer.First().Cells.Select(k => k.Body).ToList();
                var column = _API.Columns.Where(k => k.Index == i).Select(k => k).Single();
                if (columnsInBufer.IndexOf(column.HeaderText) != -1)
                {
                    if (column.Visible)
                    {
                        CreateHeaderItem(column);
                    }

                }
            }
            foreach (var headerCell in _Header)
            {
                List<HeaderCell> temp = new List<HeaderCell>();
                var headersForAnotherColumns = _Header.Select(k => k).Where(k => k != headerCell).ToList();
                headerCell.NeighborCells.Clear();
                headerCell.NeighborCells.AddRange(headersForAnotherColumns);
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
      
        private void LostFocus(object sender, EventArgs e)
        {
            RemoveEditorFromControls(true);
            _Editor = null;
            CustomInvalidate();
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
                    if (bufferRowIndex < 0)
                    {
                        bufferRowIndex = 1;
                    }
                    int viewPortRowIndex = 1;
                    if (b.Visible)
                    {
                        var ColumnItems = _Buffer.First().Cells.Select(k => k.Body).ToList();
                        var HeaderCell = _Header.Select(k => k).Where(u => u.ColumnData.Equals(b)).Single();
                        HeaderCell.Location = new Point(xCounterForColumns - HorisontalScrollBar.Value, 0);
                        b.XStartPosition = xCounterForColumns - HorisontalScrollBar.Value;
                        xCounterForColumns += _LineWidth + _CellMinMargin + b.Width * (int)this.Font.Size + _CellMinMargin;
                        b.XEndPosition = xCounterForColumns - HorisontalScrollBar.Value - 1;
                        for (int j = 0; j < _ViewPortRowsCount; j++)
                        {
                            if (bufferRowIndex < _Buffer.Count)
                            {
                                int index = GetColumnIndexInBufer(b.HeaderText);
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
        private void MyDataGrid_MouseClick(object sender, MouseEventArgs e)
        {

            RemoveTypeSelectorFromControls();
            var HitPointX = e.Location.X;
            var HitPointY = e.Location.Y;
            int BuferRowIndex;
            int BuferColumnIndex;
            if (HitPointY < (_ViewPortRowsCount + 1) * RowHeight)
            {
                int ColumnXEnd = 0;
                foreach (var column in _API.Columns)
                {
                    int ColumnXStart;
                    if (column.Visible)
                    {
                        ColumnXStart = column.XStartPosition;
                        ColumnXEnd = column.XEndPosition;
                        if (ColumnXStart < HitPointX && HitPointX < ColumnXEnd)
                        {
                            BuferRowIndex = HitPointY / RowHeight + _FirstPrintedRowIndex;
                            BuferColumnIndex = GetColumnIndexInBufer(column.HeaderText);
                            if (_Editor != null)
                            {
                                Controls.Remove(_Editor.GetControl());
                                _Editor = null;
                                UpdateColumnsPosition();
                            }
                            EditorSelector editor = new EditorSelector(_Buffer[BuferRowIndex].Cells[BuferColumnIndex], column.DataType, column.DataFormat);
                            if (_Buffer[BuferRowIndex].Cells[BuferColumnIndex].Body.Length > column.HeaderText.Length * Convert.ToInt32(Resources.ReductionRatio) || _Buffer[BuferRowIndex].Cells[BuferColumnIndex].Body.Contains(Environment.NewLine))
                            {
                                editor.IsMultilain = true;
                            }
                            ColumnXStart = column.XStartPosition;
                            ColumnXEnd = column.XEndPosition;
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
                            editor.Width = column.XEndPosition - column.XStartPosition;
                            editor.DefaultPosition = new Point(ColumnXStart + _LineWidth, _RowHeight * BuferRowIndex + _LineWidth);
                            var viewportheight = this.Height;
                            if (HorisontalScrollBar.Visible)
                            {
                                viewportheight = viewportheight - HorisontalScrollBar.Height;
                            }
                            editor.Location = new Point(ColumnXStart + _LineWidth, _RowHeight * BuferRowIndex + _LineWidth - _FirstPrintedRowIndex * RowHeight);
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
                            if (editor.Location.Y + editor.Height > viewportheight && _TotalRowsCount - BuferRowIndex <= 1)
                            {
                                editor.Location = new Point(editor.Location.X, editor.Location.Y - editor.Height + RowHeight - _LineWidth);
                            }
                            editor.ColumnIndex = column.Index;
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








