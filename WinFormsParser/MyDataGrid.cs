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
        int _ColumnHeight;
        public MyDataGrid()
        {
            InitializeComponent();
            ResizeRedraw = true;
            components = new System.ComponentModel.Container();
          
          
        }
        public List<List<string>> Source
        {
            get => _Source;
            set
            {
                _Source = value;
                if (Source.Count != 0)
                {
                  _Bufer =  CreateBufer(Source);
                    Invalidate();
                }
                

            }
        }
       public Color LineColor { get; set; }
        
        int _Margins;
        
        public int ColumnHeight
        {
            get =>_ColumnHeight;
            set
            {
                if (_ColumnHeight < Font.Height)
                {
                    _ColumnHeight = Font.Height + 2;
                }
            }
        }
      
        protected override void OnPaint(PaintEventArgs e)
        {
            DrawFrame(e);
            

        }
        public void DrawFrame(PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(LineColor, 1), Margin.Left, Margin.Top, this.Width - Margin.Left-Margin.Right,  Margin.Top);
            e.Graphics.DrawLine(new Pen(LineColor, 1), this.Width - Margin.Left - Margin.Right, Margin.Top, this.Width - Margin.Left - Margin.Right, this.Height-Margin.Top-Margin.Bottom);
          e.Graphics.DrawLine(new Pen(LineColor, 1), this.Width - Margin.Left - Margin.Right, this.Height - Margin.Top - Margin.Bottom,  Margin.Left, this.Height - Margin.Top - Margin.Bottom);
           e.Graphics.DrawLine(new Pen(LineColor, 1), Margin.Left, this.Height - Margin.Top - Margin.Bottom, Margin.Left, Margin.Top);
            Brush brush = new SolidBrush(ForeColor);
            Pen p = new Pen(LineColor);
         
            foreach (var Row in _Bufer)
            {
       
                var rowEndCounter = Row.Last().XPosition+Row.Last().ColumnWidth*Font.Size;
                foreach (var Cell in Row)
                {
                    e.Graphics.DrawString(Cell.Body, this.Font, brush, Cell.XPosition, Cell.YPosition);
                    e.Graphics.DrawLine(p, Cell.XPosition + Cell.ColumnWidth*Font.Size,Margin.Top, Cell.XPosition + Cell.ColumnWidth*Font.Size,100);
                   
                }

                e.Graphics.DrawLine(p, Margin.Left, Row[0].YPosition + ColumnHeight, rowEndCounter, Row[0].YPosition + ColumnHeight);
            }
        }
        private new List<List<Cell>> CreateBufer(List<List<string>> Source)
        {
           int CurrentWidth = Margin.Left*2;
            
            List <List<Cell>> TableRows = new List<List<Cell>>();
           for (int i = 0; i < Source.Count; i++)
           {
                TableRows.Add(new List<Cell>());
            }
            int RowIndex = 0;
            int CellIndex = 0;
            var yCounter =  Margin.Top+2;
            foreach (var Rows in TableRows)
            {                                      
                foreach (var item in Source[RowIndex])
                {
                    TableRows[RowIndex].Add(new Cell (Source[RowIndex][CellIndex],0,yCounter));
                  
                    CellIndex++;                
                }
                  yCounter += ColumnHeight+2;
                //TableRows.Add(Row);
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
                CurrentWidth += ColumnWidth * (int)this.Font.Size+(i+1)*2;
            }
            return TableRows;

        }


    }
    class Cell
    {

        public string Body { get; set; }

        public int XPosition { get; set; }
        public int YPosition { get; }
        public int ColumnWidth { get; set; }
        //   public bool NeedRefresh { get; }
        public Cell(string Body, int XPosition, int YPosition)
        {
            this.Body = Body;
            this.XPosition = XPosition;
            this.YPosition = YPosition;
            // this.NeedRefresh = NeedRefresh;
        }
    }
}
