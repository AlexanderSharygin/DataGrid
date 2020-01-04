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
    public partial class TableCell : UserControl
    {
        Brush _Brush;
        Pen _Pen;

        int _CellMinMargin = 2;
        public string Value { get; set; }
        public Type Type { get; set; }
        public int ColumnIndex { get; set; }
        public int LineWidth { get; set; }
        public Color LineColor { get; set; }
        public int StartX { get; set; }
       
        public TableCell()
        {
            InitializeComponent();
            components = new System.ComponentModel.Container();
           
           
            _Brush = new SolidBrush(ForeColor);
            _Pen = new Pen(LineColor,LineWidth);
           
        }
        protected override void OnPaint(PaintEventArgs e)
        {
           
            e.Graphics.DrawString(Value, Font, new SolidBrush(Color.Black), _CellMinMargin, _CellMinMargin);




        }

    

       
    }
}

