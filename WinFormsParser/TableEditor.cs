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
    public partial class TableEditor : UserControl
    {
        string _Value;
        internal Cell Tablecell { get; set; }
        public TableEditor()
        {
            InitializeComponent();
            components = new System.ComponentModel.Container();
            
            this.KeyPress += TableEditor_KeyPress;
        }
        public string Value { get => _Value;
            set
            {
               
                _Value = value;
                textBox1.Text = _Value;


            }
            
                 }

        private void TableEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Tablecell.Body = textBox1.Text;
                Parent.Invalidate();
            }
        }

        private void TableEditor_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TableEditor_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void TableEditor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }
    }
}
