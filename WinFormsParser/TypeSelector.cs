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
    public partial class TypeSelector : UserControl
    {
        List<string> _Items = new List<string>();
        string _SelectedItem;
        internal Column ColumnData { get; set; } = new Column("", 0, 0);
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

        

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SelectedItem = comboBox1.SelectedItem.ToString();
            ColumnData.Type = ColumnData.AllTypes.TypesCollection[_SelectedItem];        
          
            if (Parent != null)
            {
                this.Visible = false;
                           
            }
        }
    }
}
