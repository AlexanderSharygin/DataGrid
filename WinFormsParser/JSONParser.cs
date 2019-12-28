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
        List<int> prev = new List<int>();
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

          
            List<List<string>> b = new List<List<string>>();
            b = GetTable();
           
            DataTable.ColumnsData = b;        
           
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
          

            var a = LB_FieldsList.SelectedIndices;

            List<int> Selected = new List<int>();
            string SelectedItem="";
            foreach (var item in a)
            {
                Selected.Add((int)item);
            }
            if (prev.Count > 0)
            {
                if (Selected.Count > prev.Count)
                {
                    foreach (var item in Selected)
                    {
                        var ind = prev.IndexOf(item);
                        if (ind == -1)
                        {
                            SelectedItem = LB_FieldsList.Items[item].ToString();
                        }
                    }

                }
                else if (Selected.Count < prev.Count)
                {
                    foreach (var item in prev)
                    {
                        var ind = Selected.IndexOf(item);
                        if (ind == -1)
                        {
                            SelectedItem = LB_FieldsList.Items[item].ToString();
                        }
                    }
                }
            }
            else
            {
                SelectedItem = LB_FieldsList.SelectedItem.ToString();
            }
            
            
            foreach (var item in DataTable.Columns)
            {
                if (item.HeaderText == SelectedItem)
                {
                    if (Selected.Count > prev.Count)
                    {
                        item.Visible = true;            
                    }
                    if (Selected.Count < prev.Count)
                    {
                        item.Visible = false;           
                    }
                }
            }
            prev = Selected;
            //  ObservableCollection<List<string>> a = new ObservableCollection<List<string>>();
            //List<List<string>> b = new List<List<string>>();
            //  b = GetTable();
            //   foreach (var item in b)
            // {
            //        a.Add(item);
            //  }
            //    DataTable.ColumnsData = a;


        }



        private void Button1_Click(object sender, EventArgs e)
        {

            LB_FieldsList.Items.Clear();
            List<string> New = new List<string> { "eeg", "gwfg", "wewefv" };
            DataTable.Columns.Add(new Column("ID", 0, typeof(string), New));
           DataTable.Columns.Add(new Column("ID", 0, typeof(string), New));
            List<string> AggregatedObjectsFields = _JSONObjects.SelectMany(j => j.Keys).Distinct().ToList();
           
            AggregatedObjectsFields.ForEach(k => LB_FieldsList.Items.Add(k));
            int index = 0;

            foreach (var item in AggregatedObjectsFields)
            {
                List<string> temp = GetColumnItems(item);
                DataTable.Columns.Add(new Column(temp.First(), index, typeof(string), temp.GetRange(1, temp.Count - 1)));

                DataTable.Columns.Last().Visible = false;

                index++;
            }





        }

       

        private void Button2_Click(object sender, EventArgs e)
        {

            // List<string> New4 = new List<string> { "ejeg", "gwjfg", "wewjefv" };

            // DataTable.Columns.Add(new Column("ID1", 3, typeof(string), New4));
            // DataTable.Columns.Add(new Column("ID1", 2, typeof(string), New4));
            DataTable.ChangeSorting("FirstName", Sort.DESC);
          
        }
    }
}
