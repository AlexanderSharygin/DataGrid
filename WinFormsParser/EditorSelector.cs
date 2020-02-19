using System;
using System.Drawing;
using System.Globalization;
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
        public bool IsValidated { get; private set; } = true;
        public Font Font { get; set; }
        public int ColumnIndex { get; set; }
        public Cell BufferCell { get; }     
        public int Width { get => _Editor.Width; set  { _Width = value; _Editor.Width = _Width; } }        
        public int Height { get => _Editor.Height; set { _Height = value; _Editor.Height = _Height; } }     
        public Point Location { get=> _Editor.Location; set { _Location = value; _Editor.Location = _Location; } }
        public string OriginalValue { get; set; }      
        public string Value { get=>_Editor.Text; }
        public bool CancelChanges { get; set; } = false;
        public Point DefaultPosition { get; set; }
        public bool Visible { get => _Editor.Visible; set { _Visible = value; _Editor.Visible = _Visible; } }
        public bool Closed { get; set; } = false;
        public EditorSelector(Cell cell, Type type)
        {
            BufferCell = cell;
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
        public void CreateControl()
        {

            if (ColumnType == typeof(string))
            {
                TextBox Editor = new TextBox()
                {
                    Font = Font,
                    AutoSize = false,
                    Text = BufferCell.Body,
                    TabIndex = 1,
                    Enabled = true
                };
                Editor.Select(Editor.Text.Length, 0);
                Editor.KeyUp += new KeyEventHandler(ValueField_KeyUp);
                _Editor = Editor;
            }
            if (ColumnType == typeof(int))
            {
                IsValidated = false;               
                TextBox Editor = new TextBox()
                {
                    Font = Font,
                    AutoSize = false,
                    Text = BufferCell.Body,
                    TabIndex = 1,
                    Enabled = true
                };               
                int value;
                if (!int.TryParse(BufferCell.Body, out value))
                {
                    Editor.BackColor = Color.Red;
                }
                Editor.Select(Editor.Text.Length, 0);
                Editor.KeyPress += IntTypeValidation;
                Editor.KeyUp += new KeyEventHandler(ValueField_KeyUp);
                _Editor = Editor;
            }
            if (ColumnType == typeof(DateTime))
            {
                DateTimePicker Editor = new DateTimePicker()
                {
                    Font = Font,
                    AutoSize = false,
                    // It would be a column setting...
                    CustomFormat = "yyyy/MM/dd",
                    Format = DateTimePickerFormat.Custom,
                    TabIndex = 1,
                    Enabled = true
                };               
                DateTime date;
                // Isn't CultureInfo excessive since you explicitly specify custom format?
                if (DateTime.TryParseExact(BufferCell.Body, "yyyy/MM/dd", CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out date))
                {
                    Editor.Value = date;
                }
                else
                {
                    // No, you should not misleading the user! 
                    Editor.Value = DateTime.Now;
                }
                Editor.KeyUp += new KeyEventHandler(ValueField_KeyUp);
                _Editor = Editor;
            }
        }      
        private void IntTypeValidation(object sender, KeyPressEventArgs e)
        {                 
            var PressedButton = e.KeyChar;
            if (ColumnType == typeof(int))
            {
                int value;
                if (int.TryParse(_Editor.Text + PressedButton, out value))
                {
                    _Editor.BackColor = Color.White;
                    IsValidated = true;
                }              
                else
                {
                    IsValidated = false;
                }
            }
            bool isNumber = char.IsNumber(PressedButton);
            TextBox tb = sender as TextBox;
            int a;
            if (!int.TryParse(_Editor.Text + PressedButton, out a) && _Editor.Text != "" )
            {
                e.Handled = !(isNumber || char.IsControl(PressedButton) || (PressedButton == '-' && !tb.Text.Contains("-") && tb.SelectionStart == 0));
            }
            else if (isNumber)
            {
                e.Handled = long.Parse(tb.Text + PressedButton.ToString()) >= int.MaxValue || long.Parse(tb.Text + PressedButton.ToString()) < int.MinValue || (tb.Text.Contains("-") && tb.SelectionStart == 0);
            }
         
        }
        private void AdditionalValidations(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && ColumnType != typeof(string))
            {
                if (_Editor.Text == "")
                {
                    _Editor.BackColor = Color.Red;
                    IsValidated = false;
                }
            }
            int b;
            if (ColumnType == typeof(int) && int.TryParse(_Editor.Text, out b))
            {

                _Editor.BackColor = Color.White;
                IsValidated = true;

            }
        }
        private bool IsNewValueValid()
        {
            if (ColumnType == typeof(int))
            {
                int value;
                return int.TryParse(_Editor.Text, out value);

            }
            else
            { 
                return true;
            }
        }
       
        private void ValueField_KeyUp(object sender, KeyEventArgs e)
        {

            AdditionalValidations(e);
            if (e.KeyCode == Keys.Enter)
            {                
                if (!IsNewValueValid())
                {                   
                        IsValidated = false;                                     
                }
                if (IsValidated)
                {
                    Closed = true;
                    CancelChanges = false;
                    _Editor.Visible = false;
                }
                else
                {
                    Closed = true;
                    CancelChanges = true;
                    _Editor.Visible = false;
                }              
            }
            if (e.KeyCode == Keys.Escape)
            {
                Closed = true;
                CancelChanges = true;
                _Editor.Visible = false;
            }            
           
          


        } 
    }
}
