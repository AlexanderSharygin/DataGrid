namespace Parser
{
    partial class MyDataGrid
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.VerticalScrollBar = new System.Windows.Forms.VScrollBar();
            this.SuspendLayout();
            // 
            // VerticalScrollBar
            // 
            this.VerticalScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VerticalScrollBar.Location = new System.Drawing.Point(167, 0);
            this.VerticalScrollBar.Name = "VerticalScrollBar";
            this.VerticalScrollBar.Size = new System.Drawing.Size(21, 185);
            this.VerticalScrollBar.TabIndex = 0;
            this.VerticalScrollBar.Visible = false;
            this.VerticalScrollBar.ValueChanged += new System.EventHandler(this.VScrollBar1_ValueChanged);
            // 
            // MyDataGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.VerticalScrollBar);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MyDataGrid";
            this.Size = new System.Drawing.Size(188, 185);
            this.Load += new System.EventHandler(this.MyDataGrid_Load);
            this.Resize += new System.EventHandler(this.MyDataGrid_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar VerticalScrollBar;
    }
}
