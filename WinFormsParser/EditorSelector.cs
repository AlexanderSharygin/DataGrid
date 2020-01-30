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
                TextBox StringEditor = new TextBox();                     
                StringEditor.Font = Font;
                StringEditor.AutoSize = false;
                StringEditor.Text = BuferCell.Body;
                StringEditor.TabIndex = 1;
                StringEditor.Enabled = true;
                StringEditor.Select(StringEditor.Text.Length, 0);              
                StringEditor.KeyUp += new KeyEventHandler(ValueField_KeyUp);
                _Editor = StringEditor;                            
            }
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
