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
    public partial class TableEditor : UserControl
    {
    
        
        public TableEditor()
        {
            InitializeComponent();
            components = new System.ComponentModel.Container();
            
           
        }
        public TableEditor(Type t)
        {
            InitializeComponent();
            components = new System.ComponentModel.Container();
            _ColumnType = t;
            

        }
       
        Cell _TableCell;
        Type _ColumnType;
        
      
        internal Cell TableCell
        {
            get => _TableCell;
            set
            {
                _TableCell = value;
                GenerateForm();
            }
        }
        public void GenerateForm()
        {
            if (_ColumnType == typeof(string))
            {
                TextBox ValueField = new TextBox();
                ValueField.Location = new Point(0, 0);
                ValueField.Width = this.Width;
                ValueField.Height = this.Height;
                ValueField.Text = _TableCell.Body;
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

        private void TableEditor_Load(object sender, EventArgs e)
        {
           
            
        }
    }
}
