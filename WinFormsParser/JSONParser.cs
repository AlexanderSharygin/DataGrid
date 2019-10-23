using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parser;
using Parser.Properties;

namespace WinFormsParser
{
    public partial class Show_Button : Form
    {
        List<Dictionary<string, string>> _JSONObjects;
        public Show_Button()
        {
            InitializeComponent();
           _JSONObjects = new List<Dictionary<string, string>>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var inputText = File.ReadAllText("Files\\Data.txt");
            _JSONObjects = JSONParser.ParseSimpleJSON(inputText);
            FillFieldsList();
          
           
           

        }
        private void FillFieldsList()
        {
            List <string> _AggregatedObjectsFields = _JSONObjects.SelectMany(j => j.Keys).Distinct().ToList();
            LB_FieldsList.Items.Clear();
            _AggregatedObjectsFields.ForEach(k => LB_FieldsList.Items.Add(k));
        }

        private void Buttom_Show_Click(object sender, EventArgs e)
        {
            
            List<List<string>> Table = GetTable();
          
            myDataGrid1.Source = Table;
        
            DG_Table.Columns.Clear();
            bool isFirstRow = true;
            int rowIndex = 0;
           
            foreach (var row in Table)
            {
                int columnIndex = 0;
                if (isFirstRow)
                {
                    foreach (var cell in row)
                    {
                        DataGridViewColumn temp = new DataGridViewColumn();
                        temp.HeaderText = cell;
                        temp.Name = temp.HeaderText;
                        temp.CellTemplate = new DataGridViewTextBoxCell();
                        temp.SortMode = DataGridViewColumnSortMode.Automatic;
                        DG_Table.Columns.Add(temp);
                    }
                    isFirstRow = false;            
                }
                else
                {
                    DG_Table.Rows.Add();

                    foreach (var cell in row)
                    {
                        DG_Table[columnIndex,rowIndex].Value = cell;
                        columnIndex++;
                    }
                    rowIndex++;
                }
            }
        }
        private List<List <string>> GetTable()
        {
            List<string> _SelectedMenuItems = LB_FieldsList.SelectedItems.Cast<string>().ToList<string>(); 
            List<List <string>> Table = new List<List<string>>();
            int index = 0;
            int rowsCount = _JSONObjects.Count;
            List<string> headerRow = new List<string>(_SelectedMenuItems.Count);
            foreach (var item in _SelectedMenuItems)
            {
                headerRow.Add(item);
            }
            Table.Add(headerRow);
            while (index < rowsCount)
            {
                List<string> Row = new List<string>(_SelectedMenuItems.Count);
                foreach (var item in _SelectedMenuItems)
                {
                    if (_JSONObjects[index].ContainsKey(item))
                    {
                        if (_JSONObjects[index][item].Length < item.Length * Convert.ToInt32(Resources.ReductionRatio))
                        {
                            Row.Add(_JSONObjects[index][item]);
                        }
                        else
                        {
                            Row.Add(_JSONObjects[index][item].Substring(0, item.Length * Convert.ToInt32(Resources.ReductionRatio)) + Resources.Ellipsis);
                        }
                    }
                    else
                    {
                       Row.Add(Resources.UndefinedFieldText);
                    }
                }
                Table.Add(Row);
                index++;
            }            
            return Table;
             
        }

        private void MyDataGrid1_Load(object sender, EventArgs e)
        {

        }

        private void GB_Table_Enter(object sender, EventArgs e)
        {

        }

        private void MyDataGrid1_Resize(object sender, EventArgs e)
        {

       
        }
    }
}
