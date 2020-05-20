using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Parser
{
    public partial class ProgressScreen : UserControl
    {
      
        public ProgressScreen()
        {
            InitializeComponent();
        }
        public void RunProgress(CancellationToken token)
        {
            Task.Run(() =>
            {
            int value = 0;  
            while (!token.IsCancellationRequested)
                {

                    Thread.Sleep(50);
                    value++;
                    progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = value)); 
                    if (value==99)
                    {
                        value = 0;
                        progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value =value));
                    }
                  
                }
            }, token);
        }
        private  void ProgressScreen_Load(object sender, EventArgs e)
        {
          
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
