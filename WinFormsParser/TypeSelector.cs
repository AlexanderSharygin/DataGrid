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
        Column _ColumnData = new Column("", 0, 0);
        public Column ColumnData { get => _ColumnData; set { _ColumnData = value; }  }
        public TypeSelector()
        {
            InitializeComponent();

            
            
        }
        public string SelectedItem { get => _SelectedItem; set { _SelectedItem = value; comboBox1.SelectedItem = _SelectedItem; ; } }
        public List<string> Items
        { get=>_Items; set { _Items = value; comboBox1.DataSource = _Items; } }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SelectedItem = comboBox1.SelectedItem.ToString();
            _ColumnData.ColumnType = _ColumnData.AllTypes.TypesCollection[_SelectedItem];        
          
            if (Parent != null)
            {
                this.Visible = false;
                              //  Parent.Invalidate();
            }
        }
    }
}
