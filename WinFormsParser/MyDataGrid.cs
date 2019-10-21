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
        int _RowHeight;
        int _LineWidth = 1;
        int _FirstPrintedRowIndex = 0;
        int _ScrollValueRatio = 10;
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
            _Brush = new SolidBrush(ForeColor);
            _Pen = new Pen(LineColor, _LineWidth);
        }
        public List<List<string>> Source
        {
            get => _Source;
            set
            {
                _Source = value;
                if (Source.Count != 0)
                {
                    _Bufer = CreateBufer(Source);
                    _TotalRowsCount = _Bufer.Count;
                    _ViewPortRowsCount = this.Height / (RowHeight) - 1;
                    if (_TotalRowsCount > _ViewPortRowsCount)
                    {
                        VerticalScrollBar.Visible = true;
                    }
                    VerticalScrollBar.Maximum = ((_TotalRowsCount - _ViewPortRowsCount) * _ScrollValueRatio);
                    VerticalScrollBar.SmallChange = _ScrollValueRatio;
                    Invalidate();
                }
            }
        }
        public Color LineColor { get; set; }

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
            DrawTable(e);
        }
        public void DrawOutsideFrame(PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(LineColor, _LineWidth), Margin.Left, Margin.Top, this.Width - Margin.Left - Margin.Right, Margin.Top);
            e.Graphics.DrawLine(new Pen(LineColor, _LineWidth), this.Width - Margin.Left - Margin.Right, Margin.Top, this.Width - Margin.Left - Margin.Right, this.Height - Margin.Top - Margin.Bottom);
            e.Graphics.DrawLine(new Pen(LineColor, _LineWidth), this.Width - Margin.Left - Margin.Right, this.Height - Margin.Top - Margin.Bottom, Margin.Left, this.Height - Margin.Top - Margin.Bottom);
            e.Graphics.DrawLine(new Pen(LineColor, _LineWidth), Margin.Left, this.Height - Margin.Top - Margin.Bottom, Margin.Left, Margin.Top);

        }
        public void DrawHeader(PaintEventArgs e)
        {           
            if (_Bufer.Count != 0)
            {
                int ViewPortRowsCount = this.Height / RowHeight - 1;
                if (ViewPortRowsCount > _Bufer.Count - 1)
                {
                    ViewPortRowsCount = _Bufer.Count - 1;
                }
                foreach (var HaderCell in _Bufer.First())
                {
                    e.Graphics.DrawString(HaderCell.Body, this.Font, _Brush, HaderCell.XPosition, Margin.Top + (RowHeight - FontHeight) / 2);
                    e.Graphics.DrawLine(_Pen, HaderCell.XPosition + HaderCell.ColumnWidth * Font.Size, Margin.Top, HaderCell.XPosition + HaderCell.ColumnWidth * Font.Size, Margin.Top + RowHeight * (ViewPortRowsCount + 1));
                }
                _TableWidth = _Bufer[0].Last().XPosition + _Bufer[0].Last().ColumnWidth * Font.Size;
                e.Graphics.DrawLine(_Pen, Margin.Left, RowHeight, _TableWidth, RowHeight);
            }
        }

        public void DrawTable(PaintEventArgs e)
        {
            DrawOutsideFrame(e);
            DrawHeader(e);          
            if (_Bufer.Count != 0)
            {              
                int buferRowIndex = _FirstPrintedRowIndex + 1;
                int viewPortRowIndex = 1;
                for (int i = 0; i < _ViewPortRowsCount; i++)
                {
                    if (buferRowIndex < _Bufer.Count)
                    {
                        foreach (var Cell in _Bufer[buferRowIndex])
                        {
                            e.Graphics.DrawString(Cell.Body, this.Font, _Brush, Cell.XPosition, RowHeight * (viewPortRowIndex) + (RowHeight - FontHeight) / 2);

                        }
                        e.Graphics.DrawLine(_Pen, Margin.Left, RowHeight * (viewPortRowIndex + 1), _TableWidth, RowHeight * (viewPortRowIndex + 1));
                        viewPortRowIndex++;
                        buferRowIndex++;
                    }
                }
            }
        }
        private List<List<Cell>> CreateBufer(List<List<string>> Source)
        {
            int CurrentWidth = Margin.Left + _ScrollValueRatio;
            List<List<Cell>> TableRows = new List<List<Cell>>();
            for (int i = 0; i < Source.Count; i++)
            {
                TableRows.Add(new List<Cell>());
            }
            int RowIndex = 0;
            int CellIndex = 0;
            foreach (var Rows in TableRows)
            {
                foreach (var item in Source[RowIndex])
                {
                    TableRows[RowIndex].Add(new Cell(Source[RowIndex][CellIndex], 0));
                    CellIndex++;
                }
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
                CurrentWidth += ColumnWidth * (int)this.Font.Size + _CellMinMargin + _LineWidth;
            }
            return TableRows;
        }
        class Cell
        {
            public string Body { get; set; }
            public int XPosition { get; set; }
            public int ColumnWidth { get; set; }
            public Cell(string Body, int XPosition)
            {
                this.Body = Body;
                this.XPosition = XPosition;
            }
        }
        private void MyDataGrid_Resize(object sender, EventArgs e)
        {
            if (_Bufer.Count != 0)
            {
                _ViewPortRowsCount = this.Height / (RowHeight) - 1;
                if (_TotalRowsCount > _ViewPortRowsCount)
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
                VerticalScrollBar.Maximum = ((_TotalRowsCount - _ViewPortRowsCount) * 10);
            }
            Invalidate();
        }
        private void VScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            _FirstPrintedRowIndex = VerticalScrollBar.Value / _ScrollValueRatio;
            if (VerticalScrollBar.Value < 0)
            {
                _FirstPrintedRowIndex = 0;
            }
            Invalidate();
        }
    }
}

