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
        int _MaxX;      
        int _StartRowIndex = 1;
        int _StartIndex = 0;
        public MyDataGrid()
        {
            InitializeComponent();
            ResizeRedraw = true;
            components = new System.ComponentModel.Container();           
            vScrollBar1.Minimum = 0;
            vScrollBar1.Value = (_StartRowIndex-1)*10;
        }

        public List<List<string>> Source
        {
            get => _Source;
            set
            {
                _Source = value;
                if (Source.Count != 0)
                {
                    VerticalScroll.Value = 0;
                    _Bufer = CreateBufer(Source);
                    int TotalRowsCount = _Bufer.Count;
                    int ViewPortRowsCount = this.Height / (RowHeight + 2 * _LineWidth);
                    if (TotalRowsCount > ViewPortRowsCount)
                    {
                        vScrollBar1.Visible = true;
                    }
                    vScrollBar1.Maximum = ((TotalRowsCount - ViewPortRowsCount) * 10);
                    vScrollBar1.SmallChange = 10;
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
                if (_RowHeight < Font.Height)
                {
                    _RowHeight = Font.Height + 2;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
           
            DrawFrame(e);

        }
        
        public void DrawFrame(PaintEventArgs e)
        {
            int index = 1;
            e.Graphics.DrawLine(new Pen(LineColor, _LineWidth), Margin.Left, Margin.Top, this.Width - Margin.Left - Margin.Right, Margin.Top);
            e.Graphics.DrawLine(new Pen(LineColor, _LineWidth), this.Width - Margin.Left - Margin.Right, Margin.Top, this.Width - Margin.Left - Margin.Right, this.Height - Margin.Top - Margin.Bottom);
            e.Graphics.DrawLine(new Pen(LineColor, _LineWidth), this.Width - Margin.Left - Margin.Right, this.Height - Margin.Top - Margin.Bottom, Margin.Left, this.Height - Margin.Top - Margin.Bottom);
            e.Graphics.DrawLine(new Pen(LineColor, _LineWidth), Margin.Left, this.Height - Margin.Top - Margin.Bottom, Margin.Left, Margin.Top);
            Brush brush = new SolidBrush(ForeColor);
            Pen p = new Pen(LineColor, _LineWidth);
            if (_Bufer.Count != 0)
            {
                int ViewPortRowsCount = this.Height / (RowHeight + 2 * _LineWidth);
                if (ViewPortRowsCount > _Bufer.Count-1)
                {
                    ViewPortRowsCount = _Bufer.Count-1;
                }
                for (int i = 0; i < _Bufer[0].Count; i++)
                {
                    Cell cell = _Bufer[0][i];
                    e.Graphics.DrawString(cell.Body, this.Font, brush, cell.XPosition, Margin.Top+2);
                    e.Graphics.DrawLine(p, cell.XPosition + cell.ColumnWidth * Font.Size, Margin.Top, cell.XPosition + cell.ColumnWidth * Font.Size, Margin.Top + RowHeight * (ViewPortRowsCount+1) + 2 + _LineWidth);

                }
                var rowEndCounter = _Bufer[0].Last().XPosition + _Bufer[0].Last().ColumnWidth * Font.Size;
                e.Graphics.DrawLine(p, Margin.Left, RowHeight + 4, rowEndCounter, RowHeight + 4);
                int CI = _StartIndex+1;
                for (int i = 0; i < ViewPortRowsCount; i++)
                {

                    if (CI<_Bufer.Count)
                    {
                        foreach (var Cell in _Bufer[CI])
                        {
                            e.Graphics.DrawString(Cell.Body, this.Font, brush, Cell.XPosition, RowHeight * index + 4 + _LineWidth);

                        }

                        e.Graphics.DrawLine(p, Margin.Left, RowHeight * (index + 1) + 4, rowEndCounter, RowHeight * (index + 1) + 4);
                        index++;
                        CI++;
                    }

                }

            }

        }
        private new List<List<Cell>> CreateBufer(List<List<string>> Source)
        {
            int CurrentWidth = Margin.Left + 2;

            List<List<Cell>> TableRows = new List<List<Cell>>();
            for (int i = 0; i < Source.Count; i++)
            {
                TableRows.Add(new List<Cell>());
            }
            int RowIndex = 0;
            int CellIndex = 0;
            var yCounter = Margin.Top + 2;
            foreach (var Rows in TableRows)
            {
                foreach (var item in Source[RowIndex])
                {
                    TableRows[RowIndex].Add(new Cell(Source[RowIndex][CellIndex], 0, yCounter));
                    CellIndex++;
                }
                yCounter += RowHeight + 2;
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
                CurrentWidth += ColumnWidth * (int)this.Font.Size + 2 + _LineWidth;
            }         
            _MaxX = TableRows.Last().Last().XPosition + TableRows.Last().Last().ColumnWidth * (int)Font.Size + 1 + _LineWidth;
          
            return TableRows;
           
        }       
        class Cell
        {

            public string Body { get; set; }

            public int XPosition { get; set; }          
            public int ColumnWidth { get; set; }          
            public Cell(string Body, int XPosition, int YPosition)
            {
                this.Body = Body;
                this.XPosition = XPosition;             
            }
        }

        private void MyDataGrid_Resize(object sender, EventArgs e)
        {
           
            if (_Bufer.Count != 0)
            {
                int TotalRowsCount = _Bufer.Count;
                int ViewPortRowsCount = this.Height / (RowHeight + 2 * _LineWidth);
                if (TotalRowsCount > ViewPortRowsCount)
                {
                    vScrollBar1.Visible = true;
                }
                else
                {
                    vScrollBar1.Visible = false;
                }
               
                if (vScrollBar1.Value < 0)
                {
                    vScrollBar1.Minimum = 0;
                    vScrollBar1.Value = 0;
                }
                vScrollBar1.Maximum = ((TotalRowsCount - ViewPortRowsCount) * 10);

            }
            Invalidate();

        }

        private void VScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            _StartIndex= vScrollBar1.Value / 10;
            if (vScrollBar1.Value < 0)
            {
                _StartIndex = 0;
            }
            Invalidate();
        }
    }
}

