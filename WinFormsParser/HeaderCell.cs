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
    public partial class HeaderCell : UserControl
    {
        int _CellMinMargin = 2;
        int _SortDirection = 0;
       
        public HeaderCell()
        {
            InitializeComponent();
            this.Height = Font.Height + _CellMinMargin * 2;

            
        }
        public int ColumnIndex { get; set; }
        public int SortDirection
        {
            get => _SortDirection;
            set
            {
                _SortDirection = value;
               
               Invalidate();
            }
        }
      
        protected override void OnPaint(PaintEventArgs e)
        {

            e.Graphics.DrawString(Text, Font, new SolidBrush(Color.Black), _CellMinMargin, _CellMinMargin);
            if (_SortDirection < 0)
            {
                Point[] p = new Point[3];
                int a = this.Height / 2 - _CellMinMargin * 2;
                p[0] = new Point(this.Width - 2, this.Height / 2 - a);
                p[1] = new Point(this.Width - 7, this.Height / 2 + a);
                p[2] = new Point(this.Width - 12, this.Height / 2 - a);
               
                e.Graphics.FillPolygon(new SolidBrush(Color.Black),p);
            }
            if (_SortDirection > 0)
            {
                Point[] p = new Point[3];
                int a = this.Height / 2 - _CellMinMargin * 2;
                p[0] = new Point(this.Width - 2, this.Height / 2 + a);
                p[1] = new Point(this.Width - 7, this.Height / 2 - a);
                p[2] = new Point(this.Width - 12, this.Height / 2 + a);
               e.Graphics.FillPolygon(new SolidBrush(Color.Black), p);
            }
        }

        private void HeaderCell_Click(object sender, EventArgs e)
        {
            SortProcessor.ColumnSortIndex = ColumnIndex;
            SortProcessor.ChangeSortDirection();
           
            Parent.Invalidate();
           
        }
    }
    
}
