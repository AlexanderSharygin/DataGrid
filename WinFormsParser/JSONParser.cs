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
                            temp.Add(obj[item]);                    
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
      

        private void LB_FieldsList_MouseClick(object sender, EventArgs e)
        {          
            var a = LB_FieldsList.SelectedItems;
            List<string> SelectedItems = new List<string>();
            foreach (var item in a)
            {
                SelectedItems.Add(item.ToString());
            }
             var b = LB_FieldsList.Items;
            List<string> AllItems = new List<string>();
            foreach (var item in b)
            {
                AllItems.Add(item.ToString());
            }
            foreach (var item in AllItems)
            {
                if (SelectedItems.IndexOf(item) >= 0)
                {
                    var Columns = DataTable.Columns.Select(k => k).Where(k => k.HeaderText == item).ToList();
                    foreach (var column in Columns)
                    {
                        column.Visible = true;
                    }

                }
                else
                {
                    var Columns = DataTable.Columns.Select(k => k).Where(k => k.HeaderText == item).ToList();
                    foreach (var column in Columns)
                    {
                        column.Visible = false;
                    }
                }
            }        

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var table = GetTable();
            DataTable.ColumnsAutoGeneration = false;           
            DataTable.Source = table;
            List<string> AggregatedObjectsFields = table.Select(k => k.First()).ToList();              

            List<string> selectedItems = new List<string>();            
            foreach (var item in AggregatedObjectsFields)
          {
              
             DataTable.Columns.Add(new Column(item, typeof(string)) { Visible = false });
              
           }      
            LB_FieldsList.Items.Clear();
             CB_FieldsList1.Items.Clear();
            CB_FieldsList2.Items.Clear();
            foreach (var item in DataTable.Columns)
            {

                if (LB_FieldsList.Items.IndexOf(item.HeaderText) == -1)
                {
                    LB_FieldsList.Items.Add(item.HeaderText);
                }
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
                DataTable.ChangeSorting((string)CB_FieldsList1.SelectedItem, SortDirections.ASC);
            }
            if (CB_SortDirection.SelectedItem.ToString() == "DESC")
            {
                DataTable.ChangeSorting((string)CB_FieldsList1.SelectedItem, SortDirections.DESC);

            }
            if (CB_SortDirection.SelectedItem.ToString() == "None")
            {
                DataTable.ChangeSorting((string)CB_FieldsList1.SelectedItem, SortDirections.None);

            }
        }
        private void Remove_Click(object sender, EventArgs e)
        {           
           DataTable.RemoveColumnByName(CB_FieldsList2.SelectedItem.ToString()) ;
            LB_FieldsList.Items.Remove(CB_FieldsList2.SelectedItem.ToString());
            UpdateUI();
        }      
        private void Button1_Click_1(object sender, EventArgs e)
        {    
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
            DataTable.ColumnsAutoGeneration = true;
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

        private void Button3_Click_1(object sender, EventArgs e)
        {
            DataTable.ColumnsAutoGeneration = true;
            DataTable.Source = GetTable();
            LB_FieldsList.Items.Clear();
            CB_FieldsList1.Items.Clear();
            CB_FieldsList2.Items.Clear();
            foreach (var item in DataTable.Columns)
            {

                if (LB_FieldsList.Items.IndexOf(item.HeaderText) == -1)
                {
                    LB_FieldsList.Items.Add(item.HeaderText);
                }
                if (item.Visible)
                {
                    LB_FieldsList.SelectedItems.Add(item.HeaderText);
                }
                CB_FieldsList1.Items.Add(item.HeaderText);
                CB_FieldsList2.Items.Add(item.HeaderText);
            }
            CB_FieldsList2.Text = "";
            NU_FieldsIndexes.Maximum = DataTable.Columns.Count - 1;
        }

        

      
    }
}
