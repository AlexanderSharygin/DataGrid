using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Parser
{
    public partial class HeaderCell : UserControl
    {
        int _CellMinMargin = 2;
        public bool IsMoved;
        bool _IsToMoving = false;
        Column _ColumnData;
        internal APICore _API;
        TypeSelector _TypeSelector = new TypeSelector();

       

        public string HeaderText { get; set; }
        public int StartX { get; set; }
        internal List<HeaderCell> NeighborCells { get; set; } = new List<HeaderCell>();    
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
            _TypeSelector.Items = ColumnData.AllTypes.TypesCollection.Keys.ToList();
            _TypeSelector.SelectedItem = ColumnData.AllTypes.GetKeyyValue(ColumnData.Type);
            _TypeSelector.Visible = false;
            _TypeSelector.Font = this.Font;
            Controls.Add(_TypeSelector);
            _TypeSelector.ColumnData = ColumnData;                
        }   
        public void ChangeSortDirection()
        {      
                if (_API.SortDirection == Sort.DESC)
                {
                _API.SortDirection = Sort.None;
                }
                else if (_API.SortDirection == Sort.None)
                {
                _API.SortDirection = Sort.ASC;
                }
                else
                {
                _API.SortDirection = Sort.DESC;
                }          
        }
        protected override void OnPaint(PaintEventArgs e)
        {
           // _ColumnData.Type = _ColumnData.AllTypes.TypesCollection[_TypeSelector.SelectedItem];
         
            e.Graphics.DrawString(HeaderText, Font, new SolidBrush(Color.Black), _CellMinMargin, _CellMinMargin);
            if (_API.SortedColumnIndex == _ColumnData.Index)
            {
                if (_API.SortDirection == Sort.DESC)
                {
                    Point[] p = new Point[3];
                    int a = this.Height / 2 - _CellMinMargin * 2;
                    p[0] = new Point(this.Width - 2, this.Height / 2 - a);
                    p[1] = new Point(this.Width - 7, this.Height / 2 + a);
                    p[2] = new Point(this.Width - 12, this.Height / 2 - a);

                    e.Graphics.FillPolygon(new SolidBrush(Color.Black), p);
                }
                if (_API.SortDirection == Sort.ASC)
                {
                    Point[] p = new Point[3];
                    int a = this.Height / 2 - _CellMinMargin * 2;
                    p[0] = new Point(this.Width - 2, this.Height / 2 + a);
                    p[1] = new Point(this.Width - 7, this.Height / 2 - a);
                    p[2] = new Point(this.Width - 12, this.Height / 2 + a);
                    e.Graphics.FillPolygon(new SolidBrush(Color.Black), p);
                }
            }           
        }  
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
                if (_TypeSelector.Visible)
                {
                    _TypeSelector.Visible = false;
                    Parent.Invalidate();
                }
                else
                {
                    _TypeSelector.Width = 80;
                    _TypeSelector.Height = 20;
                    _TypeSelector.Location = new Point(this.Location.X + 5, this.Location.Y + 5);
                    _TypeSelector.Visible = true;
                    _TypeSelector.Parent = this.Parent;
                    _TypeSelector.BringToFront();
                }
            }
            if (e.Button == MouseButtons.Left)
            {
                bool MovingMode = false;
                foreach (var item in NeighborCells)
                {
                    if (item._IsToMoving == true)
                    {
                        int newIndex = item._ColumnData.Index;
                        item._ColumnData.Index = _ColumnData.Index;
                        _ColumnData.Index = newIndex;
                        MovingMode = true;
                        if (_API.SortedColumnIndex == item._ColumnData.Index) 
                        {
                            _API.SortedColumnIndex = newIndex;
                        }
                        else if (_API.SortedColumnIndex == newIndex)
                        {
                            _API.SortedColumnIndex = item._ColumnData.Index;
                        }
                        break;
                    }
                }
                if (!MovingMode)
                {                  
                   if (_API.SortedColumnIndex != _ColumnData.Index)
                    {
                       _API.SortDirection = Sort.None;
                   }
                  _API.SortedColumnIndex = _ColumnData.Index;
                  ChangeSortDirection();
                  
                }
                else
                {
                    Parent.Invalidate();
                    IsMoved = true;
                }
         
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
