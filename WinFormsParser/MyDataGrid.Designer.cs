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
            this.VerticalScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.VerticalScrollBar.Location = new System.Drawing.Point(231, 0);
            this.VerticalScrollBar.Name = "VerticalScrollBar";
            this.VerticalScrollBar.Size = new System.Drawing.Size(20, 228);
            this.VerticalScrollBar.TabIndex = 0;
            this.VerticalScrollBar.Visible = false;
            this.VerticalScrollBar.ValueChanged += new System.EventHandler(this.VScrollBar1_ValueChanged);
            this.VerticalScrollBar.VisibleChanged += new System.EventHandler(this.VerticalScrollBar_VisibleChanged);
            // 
            // HorisontalScrollBar
            // 
            this.HorisontalScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.HorisontalScrollBar.Location = new System.Drawing.Point(0, 208);
            this.HorisontalScrollBar.Name = "HorisontalScrollBar";
            this.HorisontalScrollBar.Size = new System.Drawing.Size(231, 20);
            this.HorisontalScrollBar.TabIndex = 1;
            this.HorisontalScrollBar.TabStop = true;
            this.HorisontalScrollBar.Visible = false;
            this.HorisontalScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HorisontalScrollBar_Scroll);
            // 
            // MyDataGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.HorisontalScrollBar);
            this.Controls.Add(this.VerticalScrollBar);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MyDataGrid";
            this.Size = new System.Drawing.Size(251, 228);
            this.Resize += new System.EventHandler(this.MyDataGrid_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar VerticalScrollBar;
        private System.Windows.Forms.HScrollBar HorisontalScrollBar;
    }
}
