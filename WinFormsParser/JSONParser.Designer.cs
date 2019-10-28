namespace WinFormsParser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Show_Button));
            this.GB_Fields = new System.Windows.Forms.GroupBox();
            this.Buttom_Show = new System.Windows.Forms.Button();
            this.LB_FieldsList = new System.Windows.Forms.ListBox();
            this.GB_Table = new System.Windows.Forms.GroupBox();
            this.myDataGrid1 = new Parser.MyDataGrid();
            this.DG_Table = new System.Windows.Forms.DataGridView();
            this.GB_Fields.SuspendLayout();
            this.GB_Table.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DG_Table)).BeginInit();
            this.SuspendLayout();
            // 
            // GB_Fields
            // 
            this.GB_Fields.Controls.Add(this.Buttom_Show);
            this.GB_Fields.Controls.Add(this.LB_FieldsList);
            this.GB_Fields.Location = new System.Drawing.Point(12, 12);
            this.GB_Fields.Name = "GB_Fields";
            this.GB_Fields.Size = new System.Drawing.Size(192, 426);
            this.GB_Fields.TabIndex = 0;
            this.GB_Fields.TabStop = false;
            this.GB_Fields.Text = "Список полей";
            // 
            // Buttom_Show
            // 
            this.Buttom_Show.Location = new System.Drawing.Point(7, 395);
            this.Buttom_Show.Name = "Buttom_Show";
            this.Buttom_Show.Size = new System.Drawing.Size(179, 23);
            this.Buttom_Show.TabIndex = 1;
            this.Buttom_Show.Text = "Показать";
            this.Buttom_Show.UseVisualStyleBackColor = true;
            this.Buttom_Show.Click += new System.EventHandler(this.Buttom_Show_Click);
            // 
            // LB_FieldsList
            // 
            this.LB_FieldsList.FormattingEnabled = true;
            this.LB_FieldsList.Location = new System.Drawing.Point(7, 20);
            this.LB_FieldsList.Name = "LB_FieldsList";
            this.LB_FieldsList.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.LB_FieldsList.Size = new System.Drawing.Size(179, 368);
            this.LB_FieldsList.TabIndex = 0;
            // 
            // GB_Table
            // 
            this.GB_Table.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_Table.Controls.Add(this.myDataGrid1);
            this.GB_Table.Controls.Add(this.DG_Table);
            this.GB_Table.Location = new System.Drawing.Point(210, 12);
            this.GB_Table.Name = "GB_Table";
            this.GB_Table.Size = new System.Drawing.Size(537, 426);
            this.GB_Table.TabIndex = 2;
            this.GB_Table.TabStop = false;
            this.GB_Table.Text = "Таблица полей";
            this.GB_Table.Enter += new System.EventHandler(this.GB_Table_Enter);
            // 
            // myDataGrid1
            // 
            this.myDataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myDataGrid1.AutoScroll = true;
            this.myDataGrid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.myDataGrid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.myDataGrid1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.myDataGrid1.LineColor = System.Drawing.Color.Black;
            this.myDataGrid1.Location = new System.Drawing.Point(7, 85);
            this.myDataGrid1.Margin = new System.Windows.Forms.Padding(0);
            this.myDataGrid1.Name = "myDataGrid1";
            this.myDataGrid1.RowHeight = 18;
            this.myDataGrid1.Size = new System.Drawing.Size(510, 170);
            this.myDataGrid1.Source = ((System.Collections.Generic.List<System.Collections.Generic.List<string>>)(resources.GetObject("myDataGrid1.Source")));
            this.myDataGrid1.TabIndex = 1;
            this.myDataGrid1.Resize += new System.EventHandler(this.MyDataGrid1_Resize);
            // 
            // DG_Table
            // 
            this.DG_Table.AllowUserToAddRows = false;
            this.DG_Table.AllowUserToDeleteRows = false;
            this.DG_Table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DG_Table.Location = new System.Drawing.Point(7, 20);
            this.DG_Table.Name = "DG_Table";
            this.DG_Table.ReadOnly = true;
            this.DG_Table.Size = new System.Drawing.Size(507, 58);
            this.DG_Table.TabIndex = 0;
            // 
            // Show_Button
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 478);
            this.Controls.Add(this.GB_Table);
            this.Controls.Add(this.GB_Fields);
            this.Name = "Show_Button";
            this.Text = "JSONParser";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.GB_Fields.ResumeLayout(false);
            this.GB_Table.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DG_Table)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GB_Fields;
        private System.Windows.Forms.ListBox LB_FieldsList;
        private System.Windows.Forms.Button Buttom_Show;
        private System.Windows.Forms.GroupBox GB_Table;
        private System.Windows.Forms.DataGridView DG_Table;
        private Parser.MyDataGrid myDataGrid1;
    }
}

