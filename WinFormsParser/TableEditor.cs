using System;

using System.Drawing;

using System.Windows.Forms;

namespace Parser
{
    partial class TableEditor : Control
    {

       
        Type _ColumnType;
        Cell _TableCell;
        private System.ComponentModel.IContainer components = null;
        private Control Editor;
        internal TableEditor(Type t)
        {
            InitializeComponent();
            components = new System.ComponentModel.Container();
            _ColumnType = t;
          
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();           
            this.Name = "TableEditor";
            this.Size = new System.Drawing.Size(150, 48);
            this.Leave += new System.EventHandler(this.TableEditor_Leave);           
            this.ResumeLayout(false);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void AddEditor(Cell t)
        {
             _TableCell=t;
            if (_ColumnType == typeof(string))
            {
                TextBox StringEditor = new TextBox();
                StringEditor.Location = new Point(0, 0);
               StringEditor.Width = this.Width;
                StringEditor.Height = this.Height;
                StringEditor.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
                StringEditor.Text = _TableCell.Body;
                StringEditor.TabIndex = 1;
                StringEditor.Select();
                StringEditor.Focus();               
                StringEditor.KeyUp += new KeyEventHandler(ValueField_KeyUp);                 
                Controls.Add(StringEditor);
                Editor = StringEditor;
           }
        }

       

        private void ValueField_KeyUp(object sender, KeyEventArgs e)
        {

             if (e.KeyCode == Keys.Enter)
             {               
                _TableCell.Body = Editor.Text;
                Visible = false;
                Parent.Invalidate();
            }
        }   

        private void TableEditor_Leave(object sender, EventArgs e)
        {           
            _TableCell.Body = Editor.Text;
            Visible = false;
        }

      
       
    }
}
