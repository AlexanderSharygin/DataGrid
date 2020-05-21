using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Parser
{
    partial class MyDataGrid 
    {
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
        private void HorizontalScrollMouseWheel(object sender, MouseEventArgs e)
        {

            UpdateColumnsPosition();
            if (_Editor != null)
            {
                RemoveEditorFromControls(true);
            }
            if (_API.IsTypeSelectorOpened)
            {
                var columnWithOpenedTypeSelector = _API.Columns.Select(k => k).Where(k => k.Index == _API.TypeSelector.ColumnData.Index).Single();
                int xstart = columnWithOpenedTypeSelector.XStartPosition;
                _API.TypeSelector.Location = new Point(xstart + 5, _API.TypeSelector.Location.Y);
            }
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
                    if (_Editor.Location.Y > (_ViewPortRowsCount + 1) * RowHeight)
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
                var column = _API.Columns.Select(k => k).Where(k => k.Index == _Editor.ColumnIndex).Single();
                int xstart = column.XStartPosition;
                int xend = column.XEndPosition;
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
      
        private async void VScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            if (_Buffer.Count == 0)
            {
                return;
            }
            bool isNeedInvalidation = true;

            UpdateColumnsPosition();
            UpdateHeadersWidth();
            RecalculateTotalTableWidth();
            if (_Editor != null)
            {
                RemoveEditorFromControls(true);
            }
            _FirstPrintedRowIndex = VerticalScrollBar.Value / _VerticalScrollValueRatio;
            Page selectedPage = _CurrentPage;
            if (_CuurentPageNumber > 1)
            {
                _FirstPrintedRowIndex = _FirstPrintedRowIndex - (BuferSize * (_CuurentPageNumber - 1)) + _ViewPortRowsCount + 1;

            }
            if (VerticalScrollBar.Value < 0)
            {
                _FirstPrintedRowIndex = 0;
            }
            if (_OldScrollValue < VerticalScrollBar.Value)
            {
                if (_CuurentPageNumber > 2)
                {
                    _ViewPortIsScrolledDown = true;
                }
            }
            else if (_OldScrollValue > VerticalScrollBar.Value)
            {
                if (_CuurentPageNumber == 2)
                {
                    _ViewPortIsScrolledDown = false;
                }
            }
            int scrollOffset = 0;
            if (_ViewPortIsScrolledDown)
            {
                scrollOffset = 1;
            }
            if ((VerticalScrollBar.Value / _VerticalScrollValueRatio >= selectedPage.EndIndex - _ViewPortRowsCount))
            {
                selectedPage = _Pages.Where(k => k.DownScrollValue <= (VerticalScrollBar.Value / _VerticalScrollValueRatio)).LastOrDefault();
                Dictionary<string, Type> columns = GetColumnsInfo();
                int selectedPageNumber = (selectedPage.Number > 1)? (selectedPage.Number - 1) : 1;             
                var printedPage = _Pages.Select(k => k).Where(k => k.Number == selectedPageNumber).Single();
                if (_CancellationTokenSourceForScrolling != null)
                {
                    _CancellationTokenSourceForScrolling.Cancel();

                }
                _CancellationTokenSourceForScrolling = new CancellationTokenSource();
                List<object> items = new List<object>();
                try
                {
                    if (selectedPage.Number - _CurrentPage.Number > 1)
                    {
                        items = await AsyncToggleSorting(printedPage.EndIndex - 1 - _ViewPortRowsCount, BuferSize + _ViewPortRowsCount, _CancellationTokenSourceForScrolling.Token, 100);
                    }
                    else
                    {
                        items = await AsyncToggleSorting(printedPage.EndIndex - 1 - _ViewPortRowsCount, BuferSize + _ViewPortRowsCount,_CancellationTokenSourceForScrolling.Token, 0);
                    }
                    if (items.Count != 0)
                    {
                        int index = 0;
                        foreach (var column in columns)
                        {
                            List<string> ColumnItems = new List<string>();
                            ColumnItems = GetColumnItemsFromSource(column.Key, column.Value, items);
                            var a = _Source[index].First();
                            _Source[index].Clear();
                            _Source[index].Add(a);
                            _Source[index].AddRange(ColumnItems);
                            index++;
                        }
                        _FirstPrintedRowIndex = 1;
                        UpdateBufferAfterScroll();
                        _CuurentPageNumber = selectedPage.Number;
                        _CurrentPage = selectedPage;
                        RecalculateTotalTableWidth();
                        UpdateHeadersWidth();
                        isNeedInvalidation = true;
                    }
                    else
                    {
                        isNeedInvalidation = false;
                    }
                }
                catch
                {
                }

            }

            if (VerticalScrollBar.Value / _VerticalScrollValueRatio + _ViewPortRowsCount + scrollOffset <= selectedPage.StartIndex - 1 && _OldScrollValue > VerticalScrollBar.Value)
            {
                scrollOffset = 0;
                int nextPageCounter = 0;
                if (_CuurentPageNumber > 2)
                {
                    nextPageCounter = 1;
                }
                Dictionary<string, Type> columns = GetColumnsInfo();
                selectedPage = _Pages.Where(s => s.UpScrollValue > (VerticalScrollBar.Value / _VerticalScrollValueRatio)).FirstOrDefault();
                var printedPage = selectedPage;
                _CuurentPageNumber = selectedPage.Number;

                if (_CancellationTokenSourceForScrolling != null)
                {
                    _CancellationTokenSourceForScrolling.Cancel();
                }
                _CancellationTokenSourceForScrolling = new CancellationTokenSource();
                try
                {
                    List<object> items = new List<object>();
                    if (_CurrentPage.Number - selectedPage.Number > 1)
                    {

                        items = await AsyncToggleSorting(printedPage.EndIndex - BuferSize - _ViewPortRowsCount - nextPageCounter, BuferSize + _ViewPortRowsCount * nextPageCounter, _CancellationTokenSourceForScrolling.Token, 100);
                    }
                    else
                    {
                        items = await AsyncToggleSorting(printedPage.EndIndex - BuferSize - _ViewPortRowsCount - nextPageCounter, BuferSize + _ViewPortRowsCount * nextPageCounter, _CancellationTokenSourceForScrolling.Token, 0);
                    }
                    if (items.Count != 0)
                    {
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
                            _FirstPrintedRowIndex = BuferSize - _ViewPortRowsCount - 1;
                        }
                        else
                        {
                            _FirstPrintedRowIndex = BuferSize - 1;
                        }
                        if (VerticalScrollBar.Value == 0)
                        {
                            _FirstPrintedRowIndex = 0;

                        }
                        UpdateBufferAfterScroll();
                        _CuurentPageNumber = selectedPage.Number;
                        _CurrentPage = selectedPage;
                        RecalculateTotalTableWidth();
                        UpdateHeadersWidth();
                        isNeedInvalidation = true;

                    }
                    else
                    {
                        isNeedInvalidation = false;
                    }
                }
                catch
                {
                }
            }
            _OldScrollValue = VerticalScrollBar.Value;
            if (isNeedInvalidation)
            {
                Invalidate();
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

                    VerticalScrollBar.Minimum = 0;
                    VerticalScrollBar.Maximum = ((_TotalRowsCount - _ViewPortRowsCount) * _VerticalScrollValueRatio) - 1;
                    VerticalScrollBar.SmallChange = _VerticalScrollValueRatio;
                    VerticalScrollBar.LargeChange = _VerticalScrollValueRatio;
                }
                UpdateHorizontalScroll();
                HorisontalScrollBar.Value = 0;

                if (_TableWidth > this.Width)
                {
                    _HorisontalScrollLargeChangeValueRatio = _TableWidth - this.Width;
                }
                HorisontalScrollBar.SmallChange = _HorisontalScrollSmallChangeValueRatio;
                HorisontalScrollBar.LargeChange = _HorisontalScrollLargeChangeValueRatio;
                CustomInvalidate();
                _API.TypeSelector.Visible = false;

            }
        }
        private void UpdateHorizontalScroll()
        {
            var viewportWidth = this.ClientSize.Width - (VerticalScrollBar.Visible ? VerticalScrollBar.Width : 0);
            if (_TableWidth >= viewportWidth)
            {
                HorisontalScrollBar.Visible = true;
                HorisontalScrollBar.Maximum = (int)(_TableWidth - viewportWidth + _HorisontalScrollLargeChangeValueRatio);
            }
            else
            {
                HorisontalScrollBar.Visible = false;
                HorisontalScrollBar.Maximum = 0;
            }
        }
        private void UpdateBufferAfterScroll()
        {
            _Buffer.Clear();
            for (int j = 0; j < _Source.Count; j++)
            {
                AddToBufer(_Source[j].First());
            }
        }
    }
}
