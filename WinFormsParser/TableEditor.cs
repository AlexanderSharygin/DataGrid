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
    partial class TableEditor : UserControl
    {

       
        Type _ColumnType;
        Cell _TableCell;
        internal TableEditor(Type t)
        {
            InitializeComponent();
            components = new System.ComponentModel.Container();
            _ColumnType = t;
          
        }           
      
        
        public void AddSelector(Cell t)
        {
             _TableCell=t;
            if (_ColumnType == typeof(string))
            {
                TextBox ValueField = new TextBox();
                ValueField.Location = new Point(0, 0);
                ValueField.Width = this.Width;
                ValueField.Height = this.Height;
                ValueField.Text = _TableCell.Body;
                ValueField.TabIndex = 1;
                ValueField.Select();
                ValueField.Focus();
                ValueField.KeyUp += new KeyEventHandler(ValueField_KeyUp);
              
                Controls.Add(ValueField);
           }
        }

       

        private void ValueField_KeyUp(object sender, KeyEventArgs e)
        {

             if (e.KeyCode == Keys.Enter)
             {
                for (int i = 0; i < Controls.Count; i++)
                {
                    if (Controls[i].GetType() == typeof(TextBox))
                    {
                        _TableCell.Body = Controls[i].Text;
                    }
                }
                Visible = false;
                Parent.Invalidate();

            }
        }      

        private void TableEditor_Resize(object sender, EventArgs e)
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i].GetType() == typeof(TextBox))
                {
                   Controls[i].Width = this.Width;
                   Controls[i].Height = this.Height;
                  
                }

            }
           
        }

       

        private void TableEditor_Leave(object sender, EventArgs e)
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i].GetType() == typeof(TextBox))
                {
                    _TableCell.Body = Controls[i].Text;
                }
            }
            Visible = false;
        }

      
       
    }
}
