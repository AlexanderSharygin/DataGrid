using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
        }
        private List<List <string>> GetTable()
        {
            List<string> _AgregatedFields = _JSONObjects.SelectMany(j => j.Keys).Distinct().ToList();  
            List<List <string>> Table = new List<List<string>>();           
            foreach (var item in _AgregatedFields)
            {
                List<string> temp = new List<string>();
                temp.Add(item);
                foreach (var obj in _JSONObjects)
                {
                    if (obj.ContainsKey(item))
                    {
                        if (obj[item].Length < item.Length * Convert.ToInt32(Resources.ReductionRatio))
                        {
                            temp.Add(obj[item]);
                        }
                        else
                        {
                            temp.Add(obj[item].Substring(0, item.Length * Convert.ToInt32(Resources.ReductionRatio)) + Resources.Ellipsis);
                        }
                    }
                    else
                    {
                       temp.Add(Resources.UndefinedFieldText);
                    }
                   
                }
                Table.Add(temp);
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

        private void LB_FieldsList_MouseClick(object sender, EventArgs e)
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


        }



        private void Button1_Click(object sender, EventArgs e)
        {
          
            List<string> AggregatedObjectsFields = _JSONObjects.SelectMany(j => j.Keys).Distinct().ToList();
           int index = 0;
            List<string> selectedItems = new List<string>();
            
            foreach (var item in AggregatedObjectsFields)
            {
                List<string> temp = GetColumnItems(item);
                DataTable.Columns.Add(new Column(temp.First(), index, typeof(string), temp.GetRange(1, temp.Count - 1)));

                DataTable.Columns.Last().Visible = false;

                index++;
            }
            LB_FieldsList.Items.Clear();
             CB_FieldsList1.Items.Clear();
            CB_FieldsList2.Items.Clear();
            foreach (var item in DataTable.Columns)
            {
               
                LB_FieldsList.Items.Add(item.HeaderText);
                if (item.Visible)
                {
                    LB_FieldsList.SelectedItems.Add(item.HeaderText);
                }
                CB_FieldsList1.Items.Add(item.HeaderText);
                CB_FieldsList2.Items.Add(item.HeaderText);
            }
            CB_FieldsList2.Text = "";
            NU_FieldsIndexes.Maximum = DataTable.Columns.Count-1;
        }     

        private void Button2_Click(object sender, EventArgs e)
        {
          
            if (CB_SortDirection.SelectedItem.ToString()=="ASC")
            {
                DataTable.ChangeSorting((string)CB_FieldsList1.SelectedItem, Sort.ASC);
            }
            if (CB_SortDirection.SelectedItem.ToString() == "DESC")
            {
                DataTable.ChangeSorting((string)CB_FieldsList1.SelectedItem, Sort.DESC);

            }
            if (CB_SortDirection.SelectedItem.ToString() == "None")
            {
                DataTable.ChangeSorting((string)CB_FieldsList1.SelectedItem, Sort.None);

            }
        }
        private void Remove_Click(object sender, EventArgs e)
        {           
            DataTable.Columns.RemoveByName(CB_FieldsList2.SelectedItem.ToString(), k => DataTable.Columns.Where(t=>t.HeaderText== CB_FieldsList2.SelectedItem.ToString()).Select(t=>t.Index).Single()) ;
            LB_FieldsList.Items.Remove(CB_FieldsList2.SelectedItem.ToString());
            UpdateUI();
        }      
        private void Button1_Click_1(object sender, EventArgs e)
        {


          //  DataTable.RowHeight = 100;
    //    DataTable.Font = new System.Drawing.Font(DataTable.Font.FontFamily, 25.5f);
      
           string temp = DataTable.Columns[(int)NU_FieldsIndexes.Value].HeaderText;
           DataTable.Columns.RemoveAt(DataTable.Columns[(int)NU_FieldsIndexes.Value].Index);
           LB_FieldsList.Items.Remove(temp);
            temp = "";
            UpdateUI();

        }
        private void UpdateUI()
        {
            CB_FieldsList1.Items.Clear();
            CB_FieldsList2.Items.Clear();
            prev.Clear();
            foreach (var item in DataTable.Columns)
            {
                CB_FieldsList1.Items.Add(item.HeaderText);
                CB_FieldsList2.Items.Add(item.HeaderText);
                if (item.Visible)
                {
                    prev.Add(item.Index);
                }
            }
            CB_FieldsList2.Text = "";
            NU_FieldsIndexes.Maximum = DataTable.Columns.Count - 1;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            DataTable.ColumnsAutoGeneretion = true;
            DataTable.Source = GetTable();           
            LB_FieldsList.Items.Clear();
            foreach (var item in DataTable.Columns)
            {

                LB_FieldsList.Items.Add(item.HeaderText);
                if (item.Visible)
                {
                    LB_FieldsList.SelectedItems.Add(item.HeaderText);
                    
                }
                CB_FieldsList1.Items.Add(item.HeaderText);
                CB_FieldsList2.Items.Add(item.HeaderText);
            }
            foreach (var item in LB_FieldsList.SelectedIndices)
            {
                prev.Add((int)item);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable.Source = GetTable();
            DataTable.Columns.Add(new Column("ID", 0, 2) { Visible = true }); 
            DataTable.Columns.Add(new Column("LastName", 0, 10));
            DataTable.UpdateBufer();

        }
    }
}
