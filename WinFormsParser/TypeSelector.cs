using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Parser
{
    public partial class TypeSelector : UserControl
    {
     
        string _SelectedItem;       
        Types _Items  = new Types();       
        internal Column ColumnData { get; set; }
        public TypeSelector()
        {
            InitializeComponent();
        }
        internal Types Items
        {
            get => _Items;
            set
            {
                _Items = value;
                List<string> keys = new List<string>();
                foreach (var item in _Items.TypesCollection.Keys)
                {
                    keys.Add(item.ToString());
                }
                var dataType = ColumnData.DataType;
                CB_Types.DataSource = keys;
                ColumnData.DataType = dataType;
               
            }
        }

        public string SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                CB_Types.SelectedItem = _SelectedItem;
            }
        }
      
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SelectedItem = CB_Types.SelectedItem.ToString();
         ColumnData.DataType =   Items.TypesCollection[_SelectedItem];
            if (Parent != null)
            {
                this.Visible = false;

            }
        }
    }
}
