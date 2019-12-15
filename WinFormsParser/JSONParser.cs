using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            ObservableCollection<List<string>> a = new ObservableCollection<List<string>>();
            List<List<string>> b = new List<List<string>>();
            b = GetTable();
            foreach (var item in b)
            {
                a.Add(item);
            }
            DataTable.ColumnsData = a;        
           
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

        public List<string> GetColumnItems(string FieldName)
        {
            List<string> ColumnItems = new List<string>();
            ColumnItems.Add(FieldName);
            foreach (var item in _JSONObjects)
            {
                if (item.ContainsKey(FieldName))
                {
                    if (item[FieldName].Length < FieldName.Length * Convert.ToInt32(Resources.ReductionRatio))
                    {
                        ColumnItems.Add(item[FieldName]);
                    }
                    else
                    {
                        ColumnItems.Add(item[FieldName].Substring(0, FieldName.Length * Convert.ToInt32(Resources.ReductionRatio)) + Resources.Ellipsis);
                    }
                }
                else
                {
                    ColumnItems.Add(Resources.UndefinedFieldText);
                }
            }
            return ColumnItems;
        }

        private void LB_FieldsList_SelectedValueChanged(object sender, EventArgs e)
        {
            ObservableCollection<List<string>> a = new ObservableCollection<List<string>>();
            List<List<string>> b = new List<List<string>>();
            b = GetTable();
            foreach (var item in b)
            {
                a.Add(item);
            }
            DataTable.ColumnsData = a;


        }

        private void LB_FieldsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            List<string> AggregatedObjectsFields = _JSONObjects.SelectMany(j => j.Keys).Distinct().ToList();
            LB_FieldsList.Items.Clear();
            AggregatedObjectsFields.ForEach(k => LB_FieldsList.Items.Add(k));
            int index = 0;
            foreach (var item in AggregatedObjectsFields)
            {
                List<string> temp = GetColumnItems(item);
                DataTable.Columns.Add(new Column(temp.First(), index, typeof(string), temp.GetRange(1, temp.Count - 1)));
                index++;
            }         
            DataTable.Update();

        }
        
    }
}
