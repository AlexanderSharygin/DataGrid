using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Parser;


namespace WinFormsParser
{
   
    public partial class Show_Button : Form
    {
        List<Dictionary<string, string>> _JSONObjects;
       List<Worker> _Workers;
      
        List<int> prev = new List<int>();
        DBModel DBData;
        public Show_Button()
        {
            InitializeComponent();
            _JSONObjects = new List<Dictionary<string, string>>();
            _Workers = new List<Worker>();
            DataTable.DataChanged += new MyDataGrid.DataChangedHeandler(CommitChanges);
            DBData = new DBModel();


        }
        void CommitChanges(object sender, EventArgs eventArgs)
        {


            try
            {
                DBData.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка при сохранении в БД. Превышено ограничение символов БД или введено недопустимое пустое значение для данного поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            }


        }


        private void Form1_Load(object sender, EventArgs e)
        {
            var inputText = File.ReadAllText("Files\\Data.txt");
            _Workers = JSONParser.CreateObjects<Worker>(inputText);
         

        }
        private List<string> GetAggregatedFields()
        {

            List<string> fields = new List<string>();
            foreach (var worker in _Workers)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(worker);
                foreach (PropertyDescriptor property in properties)
                {
                    ColumnInfo Column = new ColumnInfo();
                    string item = property.Name;
                    if (!fields.Contains(item))
                    {
                        fields.Add(item);
                    }

                }
            }
            return fields;
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

            DataTable.ColumnsAutoGeneration = false;
            DataTable.ItemsSource = _Workers;
            List<string> AggregatedObjectsFields = GetAggregatedFields();

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
            NU_FieldsIndexes.Maximum = DataTable.Columns.Count - 1;
        }

        private void Button2_Click(object sender, EventArgs e)
        {

            if (CB_SortDirection.SelectedItem.ToString() == "ASC")
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
            DataTable.RemoveColumnByName(CB_FieldsList2.SelectedItem.ToString());
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

            DataTable.ItemsSource = _Workers;
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
         
            
                var Data = DBData.Workers.Where(k => k.Id < 2000).ToList();
          
           
                DataTable.ItemsSource = Data;
        
             
            
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
    class Worker
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Prefix { get; set; }
        public string Position { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public string Notes { get; set; }
        public string Address { get; set; }
        public int StateID { get; set; }
    }
}
