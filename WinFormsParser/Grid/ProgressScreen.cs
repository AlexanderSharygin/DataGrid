using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser
{
    public partial class ProgressScreen : UserControl
    {
      
        public ProgressScreen()
        {
            InitializeComponent();
        }
        public void RunProgressBar(CancellationToken token)
        {
            Task.Factory.StartNew(() =>
            {
            int progressBarValue = 0;  
            while (!token.IsCancellationRequested)
                {

                    Thread.Sleep(50);
                    progressBarValue++;
                    ProgressBar.Invoke((MethodInvoker)(() => ProgressBar.Value = progressBarValue)); 
                    if (progressBarValue==99)
                    {
                        progressBarValue = 0;
                        ProgressBar.Invoke((MethodInvoker)(() => ProgressBar.Value =progressBarValue));
                    }
                  
                }
            }, token);
        }
       
    }
}
