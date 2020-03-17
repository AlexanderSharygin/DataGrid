using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Parser
{
    [System.ComponentModel.DesignerCategory("Code")]
  
    public class HeaderCell : Control
    {
        int _CellMinMargin = 2;
        bool _IsToMoving = false;
        internal Column ColumnData { get; }
        Source _API;
        
        TypeSelector _TypeSelector = new TypeSelector();
        public string HeaderText { get; set; }       
        internal List<HeaderCell> NeighborCells { get; set; } = new List<HeaderCell>();
        private IContainer components = null;
       
      
        internal HeaderCell(Column ColumnData, Source api)
        {
            this.SuspendLayout();
            this.BackgroundImageLayout = ImageLayout.Zoom;
            this.DoubleBuffered = true;
            this.Name = "HeaderCell";
            this.Size = new Size(127, 31);
            this.MouseClick += new MouseEventHandler(this.HeaderCell_MouseClick);
            this.ResumeLayout(false);
            components = new Container();
            this.ColumnData = ColumnData;
            HeaderText = ColumnData.HeaderText;                  
            _TypeSelector.Visible = false;
            _TypeSelector.Font = this.Font;
            Controls.Add(_TypeSelector);
            _TypeSelector.ColumnData = ColumnData;
            _TypeSelector.VisibleChanged += TypeSelector_VisibleChanged;
            _API = api;
            _TypeSelector.Items = _API.DataTypes;
            _TypeSelector.SelectedItem = _API.DataTypes.GetKeyByValue(ColumnData.DataType);
        }

        private void TypeSelector_VisibleChanged(object sender, EventArgs e)
        {
            var control = (TypeSelector)(sender);
            if (!control.Visible)
            {
                _API.IsTypeSelectorOpened = false;
            }
        }       
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
      
      
        public void ToggleSortDirection()
        {
         
            switch (_API.SortDirection)
            {
                case SortDirections.DESC: _API.SortDirection = SortDirections.None; break;
                case SortDirections.None: _API.SortDirection = SortDirections.ASC; break;
                default: _API.SortDirection = SortDirections.DESC; break;
            }

        }
        private void DrawSortDirectionIcon(bool isASC, PaintEventArgs e)
        {
            Point[] p = new Point[3];
            int a = this.Height / 2 - _CellMinMargin * 2;
            if (!isASC)
            {
                a = a * -1;
            }
            p[0] = new Point(this.Width - 2, this.Height / 2 + a);
            p[1] = new Point(this.Width - 7, this.Height / 2 - a);
            p[2] = new Point(this.Width - 12, this.Height / 2 + a);
            e.Graphics.FillPolygon(new SolidBrush(Color.Black), p);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
           
            Pen _Pen = new Pen(Color.Black); 
            e.Graphics.DrawLine(_Pen, 0, 0, this.Width, 0);
            e.Graphics.DrawLine(_Pen, this.Width-1, 1, this.Width-1, this.Height);
            e.Graphics.DrawLine(_Pen, this.Width-1, this.Height, 1, this.Height);
            e.Graphics.DrawLine(_Pen, 0, this.Height, 0, 0);
            if (_API.DataTypes.TypesCollection.ContainsKey(_TypeSelector.SelectedItem))
            {
                ColumnData.DataType = _API.DataTypes.TypesCollection[_TypeSelector.SelectedItem];
            }
            else
            {
                ColumnData.DataType = typeof(string);
            }
            e.Graphics.DrawString(HeaderText, Font, new SolidBrush(Color.Black), _CellMinMargin, _CellMinMargin);
            if (_API.SortedColumnIndex == ColumnData.Index)
            {              
                if (_API.SortDirection == SortDirections.DESC)
                {
                    DrawSortDirectionIcon(false, e);
                }
                if (_API.SortDirection == SortDirections.ASC)
                {
                    DrawSortDirectionIcon(true, e);
                }
            }           
        }
        private void TypeSelectorShow(MouseEventArgs e)
        {
            Parent.Controls.Remove(_API.TypeSelector);
            if (_TypeSelector.Visible)
            {
                _API.IsTypeSelectorOpened = false;
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
                _API.TypeSelector = _TypeSelector;
                _API.IsTypeSelectorOpened = true;
                _TypeSelector.BringToFront();

                foreach (var item in Parent.Controls)
                {
                    if (item.GetType() == typeof(VScrollBar) || item.GetType() == typeof(HScrollBar))
                    {
                        var a = (Control)item;
                        a.BringToFront();
                    }
                }
                

            }
        }
        private void CheckColumnForMoving(MouseEventArgs e)
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
        private void MoveOrChangeSorting(MouseEventArgs e)
        {
            bool MovingMode = false;
            foreach (var item in NeighborCells)
            {
                if (item._IsToMoving == true)
                {

                    int newIndex = item.ColumnData.Index;
                    item.ColumnData.Index = ColumnData.Index;
                    item._IsToMoving = false;
                    item.BackColor = Parent.BackColor;
                    ColumnData.Index = newIndex;
                    MovingMode = true;
                    if (_API.SortedColumnIndex == item.ColumnData.Index)
                    {
                        _API.SortedColumnIndex = newIndex;
                    }
                    else if (_API.SortedColumnIndex == newIndex)
                    {
                        _API.SortedColumnIndex = item.ColumnData.Index;
                    }
                    break;
                }
            }
            if (!MovingMode)
            {
                if (_API.SortedColumnIndex != ColumnData.Index)
                {
                    _API.SortDirection = SortDirections.None;
                }
                _API.SortedColumnIndex = ColumnData.Index;

                ToggleSortDirection();

            }
            else
            {
                Parent.Invalidate();
            }
        }
        private void HeaderCell_MouseClick(object sender, MouseEventArgs e)
        {
           
            if (_API.IsEditorUsed)
            {
                Parent.Controls.Remove(_API.EditorControl);
                _API.IsEditorUsed = false;

            }
            else if (_API.IsTypeSelectorOpened)
            {
                Parent.Controls.Remove(_API.TypeSelector);
                _API.IsTypeSelectorOpened = false;
            }
            else
            {
                if (e.Button == MouseButtons.Middle)
                {
                    TypeSelectorShow(e);
                   
                }
                if (e.Button == MouseButtons.Left)
                {
                    MoveOrChangeSorting(e);                 
                   
                }
                if (e.Button == MouseButtons.Right)
                {
                    CheckColumnForMoving(e);
                }

            }
         
        }

    }

}
