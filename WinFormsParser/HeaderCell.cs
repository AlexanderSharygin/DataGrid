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
        APICore _API;
        
        TypeSelector _TypeSelector = new TypeSelector();
        public string HeaderText { get; set; }       
        internal List<HeaderCell> NeighborCells { get; set; } = new List<HeaderCell>();
        private IContainer components = null;
       
      
        internal HeaderCell(Column ColumnData)
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
        }

        private void TypeSelector_VisibleChanged(object sender, EventArgs e)
        {
            var control = (TypeSelector)(sender);
            if (!control.Visible)
            {
                _API.IsTypeSelectorOpened = false;
            }
        }
        // Do you have HeaderCells that aren't connected to your API? If not, put this code in the constructor
        internal void ConnectToAPI(APICore api)
        {
            _API = api;             
            _TypeSelector.Items = _API.DataTypes;         
           _TypeSelector.SelectedItem = _API.DataTypes.GetKeyByValue(ColumnData.DataType);
          
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
            // if is ugly here. use switch/case
                if (_API.SortDirection == SortDirections.DESC)
                {
                _API.SortDirection = SortDirections.None;
                }
                else if (_API.SortDirection == SortDirections.None)
                {
                _API.SortDirection = SortDirections.ASC;
                }
                else
                {
                _API.SortDirection = SortDirections.DESC;
                }          
        }
        protected override void OnPaint(PaintEventArgs e)
        {
           
            Pen _Pen = new Pen(Color.Black); 
            e.Graphics.DrawLine(_Pen, 0, 0, this.Width, 0);
            e.Graphics.DrawLine(_Pen, this.Width-1, 1, this.Width-1, this.Height);
            e.Graphics.DrawLine(_Pen, this.Width-1, this.Height, 1, this.Height);
            e.Graphics.DrawLine(_Pen, 0, this.Height, 0, 0);
            ColumnData.DataType = _API.DataTypes.TypesCollection[_TypeSelector.SelectedItem];
            e.Graphics.DrawString(HeaderText, Font, new SolidBrush(Color.Black), _CellMinMargin, _CellMinMargin);
            if (_API.SortedColumnIndex == ColumnData.Index)
            {
                // consider refactoring this part. you are almost repeating yourself
                if (_API.SortDirection == SortDirections.DESC)
                {
                    Point[] p = new Point[3];
                    int a = this.Height / 2 - _CellMinMargin * 2;
                    p[0] = new Point(this.Width - 2, this.Height / 2 - a);
                    p[1] = new Point(this.Width - 7, this.Height / 2 + a);
                    p[2] = new Point(this.Width - 12, this.Height / 2 - a);

                    e.Graphics.FillPolygon(new SolidBrush(Color.Black), p);
                }
                if (_API.SortDirection == SortDirections.ASC)
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
            // too complex IF-ELSE chain.
            if (_API.IsEditorUsed)
            {
                // again, stop iterating Controls. Keep a reference to your editor instead.
                var a = Parent.Controls;
                foreach (var item in a)
                {
                    if (item.GetType() == _API.EditorControlType)
                    {
                        Parent.Controls.Remove((Control)item);
                    }
                }
                _API.IsEditorUsed = false;

            }
            
          else if (_API.IsTypeSelectorOpened)
            {
                var aa = Parent.Controls;
                foreach (var item in aa)
                {
                    if (item.GetType() == typeof(TypeSelector))
                    {
                        Parent.Controls.Remove((Control)item);
                    }
                }
                _API.IsTypeSelectorOpened = false;
            }
          
            else
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
                        _API.IsTypeSelectorOpened = true;
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
                       // _API.SortColumns();
                        Parent.Invalidate();
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

}
