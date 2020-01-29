using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser
{
    class EditorSelector
    {
        public Font Font { get; set; }
        public int ColumnIndex { get; set; }
        public Type Type { get; }
        public Cell BuferCell { get; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string OriginalValue { get; set; }
        public bool Dropchanges = false;

        public Control Editor { get; private set; }
        public EditorSelector(Cell cell, Type type)
        {
            BuferCell = cell;
            Type = type;
         }
        public void CreateEditor()
        {
           
            if (Type == typeof(string))
            {
                TextBox StringEditor = new TextBox();                     
                StringEditor.Font = Font;
                StringEditor.AutoSize = false;
              //  StringEditor.Size = new Size(Width, Height);            
              
                StringEditor.Text = BuferCell.Body;
                StringEditor.TabIndex = 1;
                StringEditor.Enabled = true;
                StringEditor.Select(StringEditor.Text.Length, 0);              
                StringEditor.KeyUp += new KeyEventHandler(ValueField_KeyUp);
                Editor = StringEditor;
            }
        }

        private void ValueField_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Dropchanges = false;
                Editor.Visible = false;               
            }
            if (e.KeyCode == Keys.Escape)
            {
                Dropchanges = true;
                Editor.Visible = false;
            }
        }

      

    }
}
