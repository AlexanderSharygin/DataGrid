namespace Parser
{
    partial class Show_Button
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.GB_Fields = new System.Windows.Forms.GroupBox();
            this.LB_FieldsList = new System.Windows.Forms.ListBox();
            this.GB_Table = new System.Windows.Forms.GroupBox();
            this._DataTable = new Parser.MyDataGrid();
            this.Add = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Remove = new System.Windows.Forms.Button();
            this.CB_FieldsList1 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CB_SortDirection = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.NU_FieldsIndexes = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CB_FieldsList2 = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.NUD_PageSize = new System.Windows.Forms.NumericUpDown();
            this.button4 = new System.Windows.Forms.Button();
            this.GB_Fields.SuspendLayout();
            this.GB_Table.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NU_FieldsIndexes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_PageSize)).BeginInit();
            this.SuspendLayout();
            // 
            // GB_Fields
            // 
            this.GB_Fields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.GB_Fields.Controls.Add(this.LB_FieldsList);
            this.GB_Fields.Location = new System.Drawing.Point(12, 72);
            this.GB_Fields.Name = "GB_Fields";
            this.GB_Fields.Size = new System.Drawing.Size(192, 270);
            this.GB_Fields.TabIndex = 0;
            this.GB_Fields.TabStop = false;
            this.GB_Fields.Text = "Список полей";
            // 
            // LB_FieldsList
            // 
            this.LB_FieldsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LB_FieldsList.FormattingEnabled = true;
            this.LB_FieldsList.Location = new System.Drawing.Point(6, 21);
            this.LB_FieldsList.MinimumSize = new System.Drawing.Size(4, 50);
            this.LB_FieldsList.Name = "LB_FieldsList";
            this.LB_FieldsList.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.LB_FieldsList.Size = new System.Drawing.Size(179, 238);
            this.LB_FieldsList.TabIndex = 0;
            this.LB_FieldsList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LB_FieldsList_MouseClick);
            // 
            // GB_Table
            // 
            this.GB_Table.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_Table.Controls.Add(this._DataTable);
            this.GB_Table.Location = new System.Drawing.Point(210, 12);
            this.GB_Table.Name = "GB_Table";
            this.GB_Table.Size = new System.Drawing.Size(559, 592);
            this.GB_Table.TabIndex = 2;
            this.GB_Table.TabStop = false;
            this.GB_Table.Text = "Таблица полей";
            // 
            // _DataTable
            // 
            this._DataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._DataTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._DataTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._DataTable.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._DataTable.ItemsSource = null;
            this._DataTable.LineColor = System.Drawing.Color.Black;
            this._DataTable.Location = new System.Drawing.Point(24, 25);
            this._DataTable.Margin = new System.Windows.Forms.Padding(0);
            this._DataTable.Name = "_DataTable";
            this._DataTable.PageSize = 100;
            this._DataTable.PrivateKeyColumn = null;
            this._DataTable.RowHeight = 18;
            this._DataTable.Size = new System.Drawing.Size(532, 556);
            this._DataTable.TabIndex = 1;
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(12, 12);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(192, 24);
            this.Add.TabIndex = 3;
            this.Add.Text = "Добавить все";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.AddAll_Button_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 60);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(180, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Sort";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.ChangeSorting_Button_Click);
            // 
            // Remove
            // 
            this.Remove.Location = new System.Drawing.Point(125, 32);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(61, 21);
            this.Remove.TabIndex = 5;
            this.Remove.Text = "Remove";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.RemoveByNameButton_Click);
            // 
            // CB_FieldsList1
            // 
            this.CB_FieldsList1.FormattingEnabled = true;
            this.CB_FieldsList1.Location = new System.Drawing.Point(6, 33);
            this.CB_FieldsList1.Name = "CB_FieldsList1";
            this.CB_FieldsList1.Size = new System.Drawing.Size(101, 21);
            this.CB_FieldsList1.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.CB_SortDirection);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.CB_FieldsList1);
            this.groupBox1.Location = new System.Drawing.Point(12, 400);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 94);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Сортировка";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Направление";
            // 
            // CB_SortDirection
            // 
            this.CB_SortDirection.FormattingEnabled = true;
            this.CB_SortDirection.Items.AddRange(new object[] {
            "ASC",
            "DESC",
            "None"});
            this.CB_SortDirection.Location = new System.Drawing.Point(114, 33);
            this.CB_SortDirection.Name = "CB_SortDirection";
            this.CB_SortDirection.Size = new System.Drawing.Size(71, 21);
            this.CB_SortDirection.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Поле";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.NU_FieldsIndexes);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.Remove);
            this.groupBox2.Controls.Add(this.CB_FieldsList2);
            this.groupBox2.Location = new System.Drawing.Point(12, 500);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(192, 104);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Удаление";
            // 
            // NU_FieldsIndexes
            // 
            this.NU_FieldsIndexes.Location = new System.Drawing.Point(6, 73);
            this.NU_FieldsIndexes.Name = "NU_FieldsIndexes";
            this.NU_FieldsIndexes.Size = new System.Drawing.Size(113, 20);
            this.NU_FieldsIndexes.TabIndex = 13;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(125, 73);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 21);
            this.button1.TabIndex = 12;
            this.button1.Text = "Remove";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.RemoveByIndex_Button_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Индекс";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Поле";
            // 
            // CB_FieldsList2
            // 
            this.CB_FieldsList2.FormattingEnabled = true;
            this.CB_FieldsList2.Location = new System.Drawing.Point(6, 32);
            this.CB_FieldsList2.Name = "CB_FieldsList2";
            this.CB_FieldsList2.Size = new System.Drawing.Size(113, 21);
            this.CB_FieldsList2.TabIndex = 10;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 43);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(192, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "Автогенрация";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Autogeneration_Button_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 349);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Размер страницы";
            // 
            // NUD_PageSize
            // 
            this.NUD_PageSize.Location = new System.Drawing.Point(18, 365);
            this.NUD_PageSize.Name = "NUD_PageSize";
            this.NUD_PageSize.Size = new System.Drawing.Size(120, 20);
            this.NUD_PageSize.TabIndex = 13;
            this.NUD_PageSize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
           
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(144, 365);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(52, 20);
            this.button4.TabIndex = 14;
            this.button4.Text = "Apply";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // Show_Button
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 616);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.NUD_PageSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.GB_Table);
            this.Controls.Add(this.GB_Fields);
            this.Name = "Show_Button";
            this.Text = "JSONParser";
            this.Load += new System.EventHandler(this.Application_Load);
            this.GB_Fields.ResumeLayout(false);
            this.GB_Table.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NU_FieldsIndexes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_PageSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox GB_Fields;
        private System.Windows.Forms.ListBox LB_FieldsList;
        private System.Windows.Forms.GroupBox GB_Table;
        private Parser.MyDataGrid _DataTable;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.ComboBox CB_FieldsList1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CB_SortDirection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CB_FieldsList2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown NU_FieldsIndexes;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown NUD_PageSize;
        private System.Windows.Forms.Button button4;
    }
}

