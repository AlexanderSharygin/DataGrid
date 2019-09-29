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
            GetTable(); 
        }
        private void GetTable()
        {
            List<string> _SelectedMenuItems = LB_FieldsList.SelectedItems.Cast<string>().ToList<string>(); 
            List<string>[] Table = new List<string>[LB_FieldsList.SelectedItems.Count];
           for (int i = 0; i < Table.Length; i++)
            {
                Table[i] = new List<string>();
            }
            int index = 0;
            foreach (var column in Table)
            {
               
                column.Add(_SelectedMenuItems[index]);
                foreach (var JSONObject in _JSONObjects)
                {
                    column.Add((JSONObject.ContainsKey(_SelectedMenuItems[index])) ? JSONObject[_SelectedMenuItems[index]] : Resources.UndefinedFieldText);
                }           
                for (int i = 0; i < column.Count; i++)
                 {
                    if (column[i].Length > _SelectedMenuItems[index].Length * Convert.ToInt32(Resources.ReductionRatio))
                    column[i]= column[i].Substring(0, _SelectedMenuItems[index].Length * Convert.ToInt32(Resources.ReductionRatio)) + Resources.Ellipsis;

                }
                var MaxCellWidth = column.Max(k => k.Length);                  
                index++;
            }          
        }

    }
}
