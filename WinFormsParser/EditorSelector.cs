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
        public Type Type { get; }
        public Cell BuferCell { get; }
        public int Width { get; set; }
       public int Height { get; set; }
       public Point Position { get; set; }
         Control _Editor;
        public Control Editor { get=>_Editor; }
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
                StringEditor.Size = new Size(Width, Height);
               
                StringEditor.Location = Position;
                StringEditor.Text = BuferCell.Body;
                StringEditor.TabIndex = 1;
                StringEditor.Enabled = true;
                StringEditor.Select(StringEditor.Text.Length, 0);
                StringEditor.Leave += new System.EventHandler(Editor_Leave);
                StringEditor.KeyUp += new KeyEventHandler(ValueField_KeyUp);
                _Editor = StringEditor;
            }
        }

        private void ValueField_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

               // Editor.Parent.Invalidate();
                Editor.Visible = false;
               
            }
        }

        private void Editor_Leave(object sender, EventArgs e)
        {
            BuferCell.Body = Editor.Text;
        }

    }
}
