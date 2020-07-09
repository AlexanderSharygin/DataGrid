using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;



namespace Parser
{

    public partial class Show_Button : Form
    {
        private List<Dictionary<string, string>> _JSONObjects;
        private List<Worker> _Workers;
        List<int> _PrevSelecteFields = new List<int>();
        DBModel _DBData;
        public Show_Button()
        {
            InitializeComponent();
            _JSONObjects = new List<Dictionary<string, string>>();
            _Workers = new List<Worker>();
            _DataTable.DataChanged += new MyDataGrid.DataChangedHeandler(SubmitChanges);
            _DBData = new DBModel();
        }

        void SubmitChanges(object sender, EventArgs eventArgs)
        {
            try
            {
                _DBData.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Ошибка при сохранении в БД. Превышено ограничение символов БД или введено недопустимое пустое значение для данного поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Application_Load(object sender, EventArgs e)
        {
          //  var inputText = File.ReadAllText("Files\\Data.txt");
          //  _Workers = JSONParser.CreateObjects<Worker>(inputText);

        }
        private void LB_FieldsList_MouseClick(object sender, EventArgs e)
        {

            var selectedItems = LB_FieldsList.SelectedItems;
            List<string> SelectedItems = new List<string>();
            foreach (var item in selectedItems)
            {
                SelectedItems.Add(item.ToString());
            }
            var allItems = LB_FieldsList.Items;
            List<string> AllItems = new List<string>();
            foreach (var item in allItems)
            {
                AllItems.Add(item.ToString());
            }
            foreach (var item in AllItems)
            {
                if (SelectedItems.IndexOf(item) >= 0)
                {
                    var Columns = _DataTable.Columns.Select(k => k).Where(k => k.HeaderText == item).ToList();
                    foreach (var column in Columns)
                    {
                        column.Visible = true;
                    }

                }
                else
                {
                    var Columns = _DataTable.Columns.Select(k => k).Where(k => k.HeaderText == item).ToList();
                    foreach (var column in Columns)
                    {
                        column.Visible = false;
                    }
                }
            }

        }

        private void AddAll_Button_Click(object sender, EventArgs e)
        {

            _DataTable.ColumnsAutoGeneration = false;
            _DataTable.ItemsSource = _DBData.Workers.AsEnumerable();
            _DataTable.PrivateKeyColumn = "Id";
            List<string> AggregatedObjectsFields = new List<string>() { "Id", "FirstName", "LastName", "Prefix", "Position", "BirthDate", "Notes", "Address", "StateID", "Salary", "IsAlcoholic" };
            foreach (var fieldName in AggregatedObjectsFields)
            {
                _DataTable.Columns.Add(new Column(fieldName, typeof(string)) { Visible = false });
            }
            LB_FieldsList.Items.Clear();
            CB_FieldsList1.Items.Clear();
            CB_FieldsList2.Items.Clear();
            foreach (var column in _DataTable.Columns)
            {
                if (LB_FieldsList.Items.IndexOf(column.HeaderText) == -1)
                {
                    LB_FieldsList.Items.Add(column.HeaderText);
                }
                if (column.Visible)
                {
                    LB_FieldsList.SelectedItems.Add(column.HeaderText);
                }
                CB_FieldsList1.Items.Add(column.HeaderText);
                CB_FieldsList2.Items.Add(column.HeaderText);
            }
            CB_FieldsList2.Text = "";
            NU_FieldsIndexes.Maximum = _DataTable.Columns.Count - 1;
        }

        private void ChangeSorting_Button_Click(object sender, EventArgs e)
        {

            if (CB_SortDirection.SelectedItem.ToString() == "ASC")
            {
                _DataTable.ChangeSorting((string)CB_FieldsList1.SelectedItem, SortDirections.ASC);
            }
            if (CB_SortDirection.SelectedItem.ToString() == "DESC")
            {
                _DataTable.ChangeSorting((string)CB_FieldsList1.SelectedItem, SortDirections.DESC);

            }
            if (CB_SortDirection.SelectedItem.ToString() == "None")
            {
                _DataTable.ChangeSorting((string)CB_FieldsList1.SelectedItem, SortDirections.None);

            }
        }
        private void RemoveByNameButton_Click(object sender, EventArgs e)
        {
            bool isDeleted = _DataTable.RemoveColumn(CB_FieldsList2.SelectedItem.ToString());
            if (!isDeleted)
            {
                MessageBox.Show("Нельзя удалить первичный ключ");
            }
            else
            {
                LB_FieldsList.Items.Remove(CB_FieldsList2.SelectedItem.ToString());
                UpdateUI();
            }
        }
        private void RemoveByIndex_Button_Click(object sender, EventArgs e)
        {
            bool isDeleted = _DataTable.RemoveColumn((int)NU_FieldsIndexes.Value);
            if (!isDeleted)
            {
                MessageBox.Show("Нельзя удалить первичный ключ");
            }
            else
            {
                LB_FieldsList.Items.RemoveAt((int)NU_FieldsIndexes.Value);
                UpdateUI();
            }
        }
        private void UpdateUI()
        {
            CB_FieldsList1.Items.Clear();
            CB_FieldsList2.Items.Clear();
            _PrevSelecteFields.Clear();
            foreach (var column in _DataTable.Columns)
            {
                CB_FieldsList1.Items.Add(column.HeaderText);
                CB_FieldsList2.Items.Add(column.HeaderText);
                if (column.Visible)
                {
                    _PrevSelecteFields.Add(column.Index);
                }
            }
            CB_FieldsList2.Text = "";
            NU_FieldsIndexes.Maximum = _DataTable.Columns.Count - 1;
        }


        private void Autogeneration_Button_Click(object sender, EventArgs e)
        {

            _DataTable.ColumnsAutoGeneration = true;          
            _DataTable.ItemsSource = _DBData.Workers.AsEnumerable();
            //  DataTable.ItemsSource = _Workers;
            _DataTable.PrivateKeyColumn = "Id";
            LB_FieldsList.Items.Clear();
            CB_FieldsList1.Items.Clear();
            CB_FieldsList2.Items.Clear();
            foreach (var columns in _DataTable.Columns)
            {
                if (LB_FieldsList.Items.IndexOf(columns.HeaderText) == -1)
                {
                    LB_FieldsList.Items.Add(columns.HeaderText);
                }
                if (columns.Visible)
                {
                    LB_FieldsList.SelectedItems.Add(columns.HeaderText);
                }
                CB_FieldsList1.Items.Add(columns.HeaderText);
                CB_FieldsList2.Items.Add(columns.HeaderText);
            }
            CB_FieldsList2.Text = "";
            NU_FieldsIndexes.Maximum = _DataTable.Columns.Count - 1;
        }
    }
  /*  class Worker
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Prefix { get; set; }
        public string Position { get; set; }
        public DateTime BirthDate { get; set; }
        public string Notes { get; set; }
        public string Address { get; set; }
        public int StateID { get; set; }
        //   public int Salary { get; set; }
        //  public bool IsAlcoholic { get; set; }
    }*/
}
