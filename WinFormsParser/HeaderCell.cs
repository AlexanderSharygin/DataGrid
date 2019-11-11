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
        Column _ColumnData = new Column("",0,0);
        List<HeaderCell> _OtherCells = new List<HeaderCell>();
        public List<HeaderCell> OtherCells { get =>_OtherCells; set { _OtherCells = value; } }
        public HeaderCell()
        {
            InitializeComponent();
            this.Height = Font.Height + _CellMinMargin * 2;

            
        }
        public HeaderCell(Column ColumnData)
        {
            InitializeComponent();
            this.Height = Font.Height + _CellMinMargin * 2;
            _ColumnData = ColumnData;

        }
        public int ColumnIndex { get; set; }
        public Sort SortDirection
        {
            get => _ColumnData.SortDirecion;
           
        }
        public void ChangeSortDirection()
        {
            
            if (_ColumnData.SortDirecion == Sort.DESC)
            {
               _ColumnData.SortDirecion = Sort.None;
            }
            else if (_ColumnData.SortDirecion == Sort.None)
            {
                _ColumnData.SortDirecion = Sort.ASC;
            }
            else
            {
                _ColumnData.SortDirecion = Sort.DESC;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            e.Graphics.DrawString(Text, Font, new SolidBrush(Color.Black), _CellMinMargin, _CellMinMargin);
            if (_ColumnData.SortDirecion == Sort.DESC)
            {
                Point[] p = new Point[3];
                int a = this.Height / 2 - _CellMinMargin * 2;
                p[0] = new Point(this.Width - 2, this.Height / 2 - a);
                p[1] = new Point(this.Width - 7, this.Height / 2 + a);
                p[2] = new Point(this.Width - 12, this.Height / 2 - a);
               
                e.Graphics.FillPolygon(new SolidBrush(Color.Black),p);
            }
            if (_ColumnData.SortDirecion == Sort.ASC)
            {
                Point[] p = new Point[3];
                int a = this.Height / 2 - _CellMinMargin * 2;
                p[0] = new Point(this.Width - 2, this.Height / 2 + a);
                p[1] = new Point(this.Width - 7, this.Height / 2 - a);
                p[2] = new Point(this.Width - 12, this.Height / 2 + a);
               e.Graphics.FillPolygon(new SolidBrush(Color.Black), p);
            }
        }
        public void DropSorting()
        {
            _ColumnData.IsSortedBy = false;
            _ColumnData.SortDirecion = Sort.None;
            Invalidate();
        }
        private void HeaderCell_Click(object sender, EventArgs e)
        {

            foreach (var item in _OtherCells)
            {
                item.DropSorting();
            }
            _ColumnData.IsSortedBy = true;
            ChangeSortDirection();
            Invalidate();
           Parent.Invalidate();
           
        }
    }
    
}
