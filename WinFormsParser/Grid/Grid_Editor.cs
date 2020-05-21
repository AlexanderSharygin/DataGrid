using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser
{

    partial class MyDataGrid
    {
        private void RemoveEditorFromControls(bool isDropChanges)
        {
            var editorControl = _Editor?.GetControl();
            if (editorControl != null)
            {
                Controls.Remove(editorControl);
                if (!isDropChanges)
                {
                    _Editor.BufferCell.Body = _Editor.OriginalValue;
                }
                _Editor = null;
                _API.IsEditorUsed = false;
                _API.IsEditorOpened = false;
            }
        }
        private async void Editor_Leave(object sender, EventArgs e)
        {
            if (!_Editor.CancelChanges && _Editor.IsValidated && _Editor.Value != _Editor.OriginalValue)
            {
                Type sourceObjectsType = _ItemsSource.First().GetType();
                var tempObject = Activator.CreateInstance(sourceObjectsType);
                PropertyDescriptorCollection tempObjectProperties = TypeDescriptor.GetProperties(tempObject);
                Row bufeRow = _Buffer[_Editor.BufferCell.BuferRowIndex];
                foreach (Cell cell in bufeRow.Cells)
                {
                    var tempObjectProperty = tempObjectProperties.Find(_API.Columns[cell.SourceColumnIndex].HeaderText, false);
                    if (tempObjectProperty?.PropertyType == typeof(bool))
                    {
                        tempObjectProperty.SetValue(tempObject, Convert.ChangeType(ConvertBuuferValueToBoolView(cell), typeof(bool)));
                    }
                    else
                    {
                        tempObjectProperty?.SetValue(tempObject, Convert.ChangeType(cell.Body, tempObjectProperty.PropertyType));
                    }
                }

                var sourceeData = TooggleSorting(_CurrentPage.SkipElementsCount, _CurrentPage.TakeElementsCount, CancellationToken.None).AsQueryable();
                bool isPerformed = false;
                _ProgressScreen.Size = new Size(150, 50);
                _ProgressScreen.Location = new Point(Width / 2 - 75, Height / 2 - 25);
                Controls.Add(_ProgressScreen);
                object sourceObject = new object();
                Task t1 = Task.Factory.StartNew(() =>
                {
                    sourceObject = sourceeData.GetObjectWithEqualProperties(tempObject);
                    isPerformed = true;

                });
                Task t2 = Task.Factory.StartNew(() =>
                {

                    CancellationTokenSource cts = new CancellationTokenSource();
                   
                    _ProgressScreen.RunProgress(cts.Token);
                    while (!isPerformed)
                    {
                        continue;
                    }
                    if (isPerformed)
                    {

                        cts.Cancel();
                        
                    }

                });
                await Task.WhenAll(new[] { t1, t2 });
                
                Controls.Remove(_ProgressScreen);

                PropertyDescriptorCollection sourceObjectProperties = TypeDescriptor.GetProperties(sourceObject);
                var sourceObjectProperty = sourceObjectProperties.Find(_API.Columns[_Editor.BufferCell.SourceColumnIndex].HeaderText, false);
                if (sourceObjectProperty != null)
                {
                    sourceObjectProperty.SetValue(sourceObject, Convert.ChangeType(_Editor.Value, sourceObjectProperty.PropertyType));
                }
                if (_Editor.GetControl().GetType() == typeof(CheckBox))
                {
                    SetBoolValueInBuffer();
                }
                else
                {
                    _Editor.BufferCell.Body = _Editor.Value;
                }
                if (DataChanged != null)
                {
                    EventArgs eventArgs = new EventArgs();
                    DataChanged(this, eventArgs);
                }
            }
            else
            {
                if (_Editor.GetControl().GetType() == typeof(CheckBox))
                {
                    SetBoolValueInBuffer();
                }
                else
                {
                    _Editor.BufferCell.Body = _Editor.OriginalValue;
                }
            }
            _API.IsEditorOpened = false;
            if (_Editor.Closed)
            {
                _API.IsEditorUsed = false;
            }
            UpdateColumnsPosition();
            UpdateHeadersWidth();
            RecalculateTotalTableWidth();
            if (_API.SortDirection != SortDirections.None && (_API.SortedColumnIndex == _Editor.ColumnIndex && _Editor.OriginalValue != _Editor.Value && !_Editor.CancelChanges))
            {
                SortData();
            }
            VerticalScrollBar.Maximum = ((_TotalRowsCount - _ViewPortRowsCount) * _VerticalScrollValueRatio) - 1;
            Invalidate();
        }
    }
        class Editor
    {
      
        private Type _ColumnType;
        private int _Width;
        private int _Height;
        private Point _Location;
        private bool _Visible;
        private Control _Editor;
        private string _DataTimeFormat;    
      
        public bool IsMultilain { get; set; }
        public int ScrollCounter { get; set; }
        public bool IsValidated { get; private set; } = true;
        public Font Font { get; set; }
        public int ColumnIndex { get; set; }
        public Cell BufferCell { get; }     
        public int Width
        { 
            get => _Editor.Width;
            set
            {
                _Width = value;
                _Editor.Width = _Width;
            }
        }        
        public int Height 
        {
            get => _Editor.Height;
            set
            {
                _Height = value;
                _Editor.Height = _Height;
            }
        }     
        public Point Location 
        {
            get=> _Editor.Location; 
            set 
            { _Location = value;
                _Editor.Location = _Location;
                if (_ColumnType == typeof(Boolean))
                {
                    _Editor.Width = _Editor.Height;                  
                    BufferCell.BodyToPrint = "";
                }
            } 
        }
        public string OriginalValue { get; set; }       
        public string Value { get => _Editor.Text; }
        public bool CancelChanges { get; set; } = false;
        public Point DefaultPosition { get; set; }
        public bool Visible { get => _Editor.Visible; set { _Visible = value; _Editor.Visible = _Visible; } }
        public bool Closed { get; set; } = false;
    
      
        public Editor(Cell cell, Type type, string dataFormat)
        {
            BufferCell = cell;
            _ColumnType = type;
            _DataTimeFormat = dataFormat;
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
            if (_ColumnType == typeof(string))
            {
                if (IsMultilain)
                {
                    RichTextBox editor = new RichTextBox();
                    editor.Text = BufferCell.Body;
                    editor.Multiline = true;
                    editor.SelectionAlignment = HorizontalAlignment.Left;
                    editor.TabIndex = 1;
                    editor.Enabled = true;
                    editor.Select(editor.Text.Length, 0);
                    editor.KeyUp += new KeyEventHandler(ValueField_KeyUp);
                    _Editor = editor;
                }
                else
                {
                    TextBox editor = new TextBox()
                    {
                        Font = Font,
                        AutoSize = false,
                        Text = BufferCell.Body,
                        TabIndex = 1,
                        Enabled = true
                    };
                    editor.Select(editor.Text.Length, 0);
                    editor.KeyUp += new KeyEventHandler(ValueField_KeyUp);
                    _Editor = editor;
                }
            }
            if (_ColumnType == typeof(int))
            {
                IsValidated = false;               
                TextBox editor = new TextBox()
                {
                    Font = Font,
                    AutoSize = false,
                    Text = BufferCell.Body,
                    TabIndex = 1,
                    Enabled = true
                };                          
                if (!int.TryParse(BufferCell.Body, out int value))
                {
                    editor.BackColor = Color.Red;
                }
                editor.Select(editor.Text.Length, 0);
                editor.KeyPress += IntTypeValidation;
                editor.KeyUp += new KeyEventHandler(ValueField_KeyUp);
                _Editor = editor;
            }
            if (_ColumnType == typeof(bool))
            {
                CheckBox editor = new CheckBox()
                {                  
                    AutoSize = false,                         
                    TabIndex = 1,
                    Enabled = true
                };
                editor.Checked = (BufferCell.Body==Properties.Resources.TrueValue)? true: false;                
                editor.KeyUp += new KeyEventHandler(ValueField_KeyUp);
                editor.CheckedChanged += new EventHandler(CheckedChanged);
                _Editor = editor;               
                if (editor.Checked)
                {
                    _Editor.Text = "True";
                }
                else
                {
                    _Editor.Text = "False";
                }              
            }
            if (_ColumnType == typeof(DateTime))
            {
                DateTimePicker editor = new DateTimePicker()
                {
                    Font = Font,
                    AutoSize = false,                   
                    CustomFormat = _DataTimeFormat,
                    Format = DateTimePickerFormat.Custom,
                    TabIndex = 1,
                    Enabled = true
                };               
                DateTime date;         
                if (DateTime.TryParseExact(BufferCell.Body, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    editor.Value = date;
                }
                else
                {                  
                    editor.Value = DateTime.Parse("2000/01/01");                
                }
                editor.KeyUp += new KeyEventHandler(ValueField_KeyUp);
                _Editor = editor;
            }
        }
      

  
        private void CheckedChanged(object sender, EventArgs e)
        {
            CheckBox editor = (CheckBox)sender;
            if (editor.Checked)
            {
                _Editor.Text = "True";
            }
            else
            {
                _Editor.Text = "False";
            }
        }
        private void IntTypeValidation(object sender, KeyPressEventArgs e)
        {                 
            var pressedButton = e.KeyChar;
            if (_ColumnType == typeof(int))
            {               
                if (int.TryParse(_Editor.Text + pressedButton, out int output))
                {
                    _Editor.BackColor = Color.White;
                    IsValidated = true;
                }              
                else
                {
                    IsValidated = false;
                }
            }
            bool isNumber = char.IsNumber(pressedButton);
            TextBox tb = sender as TextBox;         
            if (!int.TryParse(_Editor.Text + pressedButton, out int value) && _Editor.Text != "" )
            {
                e.Handled = !(isNumber || char.IsControl(pressedButton) || (pressedButton == '-' && !tb.Text.Contains("-") && tb.SelectionStart == 0));
            }
            else if (isNumber)
            {
                e.Handled = long.Parse(tb.Text + pressedButton.ToString()) >= int.MaxValue || long.Parse(tb.Text + pressedButton.ToString()) < int.MinValue || (tb.Text.Contains("-") && tb.SelectionStart == 0);
            }
         
        }
        private void AdditionalValidations(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && _ColumnType != typeof(string))
            {
                if (_Editor.Text == "")
                {
                    _Editor.BackColor = Color.Red;
                    IsValidated = false;
                }
            }
            if (_ColumnType == typeof(int) && int.TryParse(_Editor.Text, out int b))
            {
                _Editor.BackColor = Color.White;
                IsValidated = true;
            }
        }
        private bool IsNewValueValid()
        {
            if (_ColumnType == typeof(int))
            {               
                return int.TryParse(_Editor.Text, out int value);
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
