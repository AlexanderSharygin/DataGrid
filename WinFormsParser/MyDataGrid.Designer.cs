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
            this.HorisontalScrollBar = new System.Windows.Forms.HScrollBar();
            this.SuspendLayout();
            // 
            // VerticalScrollBar
            // 
            this.VerticalScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VerticalScrollBar.Location = new System.Drawing.Point(168, 0);
            this.VerticalScrollBar.Name = "VerticalScrollBar";
            this.VerticalScrollBar.Size = new System.Drawing.Size(20, 185);
            this.VerticalScrollBar.TabIndex = 0;
            this.VerticalScrollBar.Visible = false;
          ;
            this.VerticalScrollBar.ValueChanged += new System.EventHandler(this.VScrollBar1_ValueChanged);
            this.VerticalScrollBar.VisibleChanged += new System.EventHandler(this.VerticalScrollBar_VisibleChanged);
            // 
            // HorisontalScrollBar
            // 
            this.HorisontalScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HorisontalScrollBar.Location = new System.Drawing.Point(0, 164);
            this.HorisontalScrollBar.Name = "HorisontalScrollBar";
            this.HorisontalScrollBar.Size = new System.Drawing.Size(167, 20);
            this.HorisontalScrollBar.TabIndex = 1;
            this.HorisontalScrollBar.TabStop = true;
            this.HorisontalScrollBar.Visible = false;
            this.HorisontalScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HorisontalScrollBar_Scroll);
            // 
            // MyDataGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.HorisontalScrollBar);
            this.Controls.Add(this.VerticalScrollBar);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MyDataGrid";
            this.Size = new System.Drawing.Size(188, 185);
         
            this.Resize += new System.EventHandler(this.MyDataGrid_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar VerticalScrollBar;
        private System.Windows.Forms.HScrollBar HorisontalScrollBar;
    }
}
