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
        private Type ColumnType;
        private int _Width;
        private int _Height;
        private Point _Location;
        private bool _Visible;
        private Control _Editor;

        public Font Font { get; set; }
        public int ColumnIndex { get; set; }
        public Cell BuferCell { get; }     
        public int Width { get => _Editor.Width; set  { _Width = value; _Editor.Width = _Width; } }        
        public int Height { get => _Editor.Height; set { _Height = value; _Editor.Height = _Height; } }     
        public Point Location { get=> _Editor.Location; set { _Location = value; _Editor.Location = _Location; } }
        public string OriginalValue { get; set; }      
        public string Value { get=>_Editor.Text; }
        public bool Dropchanges { get; set; } = false;
        public Point DefaultPosition { get; set; }
        public bool Visible { get => _Editor.Visible; set { _Visible = value; _Editor.Visible = _Visible; } }

        public bool closed = false;
        public EditorSelector(Cell cell, Type type)
        {
            BuferCell = cell;
            ColumnType = type;
        }
        public Type GetComponentType()
        {
            return _Editor.GetType();
        }
        public Control GetControl()
        {
            return _Editor;
        }
        public void SetFocus()
        {
            _Editor.Focus();
        }       
        public void CreateEditor()
        {
           
            if (ColumnType == typeof(string))
            {
                TextBox Editor = new TextBox();                     
                Editor.Font = Font;
                Editor.AutoSize = false;
                Editor.Text = BuferCell.Body;
                Editor.TabIndex = 1;
                Editor.Enabled = true;
                Editor.Select(Editor.Text.Length, 0);
                Editor.KeyUp += new KeyEventHandler(ValueField_KeyUp);
                _Editor = Editor;                            
            }
            if (ColumnType == typeof(Int32))
            {
                TextBox Editor = new TextBox();
                Editor.Font = Font;
                Editor.AutoSize = false;
                Editor.Text = BuferCell.Body;
                Editor.TabIndex = 1;
                Editor.Enabled = true;
                Editor.Select(Editor.Text.Length, 0);
                Editor.KeyPress += Editor_KeyPress;
                Editor.KeyUp += new KeyEventHandler(ValueField_KeyUp);
                _Editor = Editor;
            }
        }


      
        private void Editor_KeyPress(object sender, KeyPressEventArgs e)
        {
            var PressedButton = e.KeyChar;
           
                bool isNumber = char.IsNumber(PressedButton);
            TextBox tb = sender as TextBox;
            if (isNumber)
            {                 
                e.Handled =  long.Parse(tb.Text + PressedButton.ToString()) >= int.MaxValue || long.Parse(tb.Text + PressedButton.ToString())<int.MinValue || (tb.Text.Contains("-") && tb.SelectionStart == 0);
            }
                else
                e.Handled = !(isNumber || char.IsControl(PressedButton) || (PressedButton == '-'&& !tb.Text.Contains("-") && tb.SelectionStart==0));
            
        }

       
        private void ValueField_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               
                closed = true;
                Dropchanges = false;
                _Editor.Visible = false;
               

            }
            if (e.KeyCode == Keys.Escape)
            {
                closed = true;
                Dropchanges = true;
                _Editor.Visible = false;
              

            }
        } 
    }
}
