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
        bool _IsToMoving = false;
        public string HeaderText { get; set; }
        Column _ColumnData = new Column("",0,0);
         public List<HeaderCell> NeighborCells { get; set; } = new List<HeaderCell>();
        public HeaderCell()
        {
            InitializeComponent();
            this.Height = Font.Height + _CellMinMargin * 2;

            
        }
        public HeaderCell(Column ColumnData)
        {
            InitializeComponent();       
            _ColumnData = ColumnData;
            HeaderText = ColumnData.HeaderText;

        }    
        public Sort SortDirection
        {
            get => _ColumnData.SortDirecion;
           
        }
        private void ChangeSortDirection()
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

            e.Graphics.DrawString(HeaderText, Font, new SolidBrush(Color.Black), _CellMinMargin, _CellMinMargin);
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

        private void HeaderCell_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                bool MovingMode = false;
                int newIndex = -1;
                foreach (var item in NeighborCells)
                {
                    if (item._IsToMoving == true)
                    {                        
                        newIndex = item._ColumnData.Index;
                        item._ColumnData.Index = _ColumnData.Index;
                        _ColumnData.Index = newIndex;
                        MovingMode = true;
                        break;
                    }
                }
                if (!MovingMode)
                {
                    foreach (var item in NeighborCells)
                    {
                        item.DropSorting();
                    }
                    _ColumnData.IsSortedBy = true;
                    ChangeSortDirection();
                    Invalidate();
                }
                Parent.Invalidate();
            }
            if (e.Button == MouseButtons.Right)
            {
                if (!_IsToMoving)
                {
                    BackColor = Color.Coral;
                    _IsToMoving = true;
                    foreach (var item in NeighborCells)
                    {
                        item._IsToMoving = false;
                        item.BackColor = Parent.BackColor;
                    }
                }
                else if (_IsToMoving)
                {
                    BackColor = Parent.BackColor;
                    _IsToMoving = false;
                }
               
                
            }
        }
    }
    
}
