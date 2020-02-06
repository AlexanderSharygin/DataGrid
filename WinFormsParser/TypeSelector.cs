﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Parser
{
    public partial class TypeSelector : UserControl
    {
        List<string> _Items = new List<string>();
        string _SelectedItem;
        internal Types AllTypes { get; set; } = new Types();
        internal Column ColumnData { get; set; } = new Column("", typeof(String));
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
        //   ColumnData.Type =   AllTypes.TypesCollection[_SelectedItem]; 
          
            if (Parent != null)
            {
                this.Visible = false;
                           
            }
        }
    }
}
