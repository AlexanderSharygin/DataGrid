using System;
using System.Collections.Generic;
using System.Drawing;
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
        Column _ColumnData; //= new Column("", 0, 0);
        public List<HeaderCell> NeighborCells { get; set; } = new List<HeaderCell>();
        TypeSelector TypeSelector = new TypeSelector();
        public HeaderCell()
        {
            InitializeComponent();
            this.Height = Font.Height + _CellMinMargin * 2;


        }
        internal HeaderCell(Column ColumnData)
        {
            InitializeComponent();
            components = new System.ComponentModel.Container();
            _ColumnData = ColumnData;
            HeaderText = ColumnData.HeaderText;
            TypeSelector.Items = ColumnData.AllTypes.TypesCollection.Keys.ToList();
            TypeSelector.SelectedItem = ColumnData.AllTypes.GetKeyyValue(ColumnData.Type);
            TypeSelector.Visible = false;
            TypeSelector.Font = this.Font;
            Controls.Add(TypeSelector);
            TypeSelector.ColumnData = ColumnData;



        }
        public Sort SortDirection
        {
            get => _ColumnData.SortDirection;

        }
        private void ChangeSortDirection()
        {

            
            if (_ColumnData.SortDirection == Sort.DESC)
            {
                _ColumnData.SortDirection = Sort.None;
            }
            else if (_ColumnData.SortDirection == Sort.None)
            {
                _ColumnData.SortDirection = Sort.ASC;
            }
            else
            {
                _ColumnData.SortDirection = Sort.DESC;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            _ColumnData.Type = _ColumnData.AllTypes.TypesCollection[TypeSelector.SelectedItem];
            e.Graphics.DrawString(HeaderText, Font, new SolidBrush(Color.Black), _CellMinMargin, _CellMinMargin);
            if (_ColumnData.SortDirection == Sort.DESC)
            {
                Point[] p = new Point[3];
                int a = this.Height / 2 - _CellMinMargin * 2;
                p[0] = new Point(this.Width - 2, this.Height / 2 - a);
                p[1] = new Point(this.Width - 7, this.Height / 2 + a);
                p[2] = new Point(this.Width - 12, this.Height / 2 - a);

                e.Graphics.FillPolygon(new SolidBrush(Color.Black), p);
            }
            if (_ColumnData.SortDirection == Sort.ASC)
            {
                Point[] p = new Point[3];
                int a = this.Height / 2 - _CellMinMargin * 2;
                p[0] = new Point(this.Width - 2, this.Height / 2 + a);
                p[1] = new Point(this.Width - 7, this.Height / 2 - a);
                p[2] = new Point(this.Width - 12, this.Height / 2 + a);
                e.Graphics.FillPolygon(new SolidBrush(Color.Black), p);
            }
        }
      //  public void DropSorting()
      //  {
       //     _ColumnData.IsSortedBy = false;
       ////     _ColumnData.SortDirection = Sort.None;
      //      Invalidate();
      //  }

        private void HeaderCell_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Middle)
            {

                for (int i = 0; i < Parent.Controls.Count; i++)
                {
                    Type Type = Parent.Controls[i].GetType();

                    if (Type == typeof(TypeSelector))
                    {
                        Parent.Controls.RemoveAt(i);
                    }
                }

                if (TypeSelector.Visible)
                {
                    TypeSelector.Visible = false;
                    Parent.Invalidate();
                }
                else
                {

                    TypeSelector.Width = 80;
                    TypeSelector.Height = 20;
                    TypeSelector.Location = new Point(this.Location.X + 5, this.Location.Y + 5);
                    TypeSelector.Visible = true;
                    TypeSelector.Parent = this.Parent;
                    TypeSelector.BringToFront();
                }
            }
            if (e.Button == MouseButtons.Left)
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
                    // foreach (var item in NeighborCells)
                    // {
                    //      item.DropSorting();
                    //  }
                    _ColumnData.IsSortedBy = true;
                    ChangeSortDirection();
                    // Invalidate();
                }
                else
                {
                    Parent.Invalidate();
                }
              // 
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
