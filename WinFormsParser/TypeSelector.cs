using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Parser
{
    public partial class TypeSelector : UserControl
    {
        List<string> _Items = new List<string>();
        string _SelectedItem;
        internal APICore _API = new APICore();
        internal Column ColumnData { get; set; }
        public TypeSelector()
        {
            InitializeComponent();
        }
        public string SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                comboBox1.SelectedItem = _SelectedItem;
            }
        }
        public List<string> Items
        {
            get =>_Items;
            set
            {
                _Items = value; comboBox1.DataSource = _Items;
            }
        }
        internal void AddAPI(APICore api)
        {
            _API = api;
          

        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SelectedItem = comboBox1.SelectedItem.ToString();
         ColumnData.DataType =   _API.AllTypes.TypesCollection[_SelectedItem];
            _API.IsTypeSelectorOpened = false;

            if (Parent != null)
            {
                this.Visible = false;


               /* foreach (var item in Parent.Controls)
                {
                    {
                        Type Type = item.GetType();

                        if (Type == typeof(TypeSelector))
                        {
                            Parent.Controls.Remove((Control)item);
                        }

                    }
                }*/
            }
        }
    }
}
