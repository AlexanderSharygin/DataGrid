using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser
{
    partial class MyDataGrid
    {

        private async void AsyncGetCount()
        {
            await Task.Factory.StartNew(() => {
                _Pages.Clear();
                _TotalRowsCount = _ItemsSource.Count();
                int pagesCount = (int)(Math.Ceiling(Convert.ToDecimal(_TotalRowsCount / BuferSize)));
                if (pagesCount == 0)
                {
                    pagesCount = 1;
                }
                Dictionary<string, Type> columnsInfo = GetColumnsInfo();
                _ViewPortRowsCount = (this.Height) / (RowHeight) - 1;
                for (int i = 0; i < pagesCount; i++)
                {
                    _Pages.Add(CreateNewPage(i));
                }
                _CurrentPage = _Pages.FirstOrDefault();
                if (_TotalRowsCount > _ViewPortRowsCount + 1)
                {


                    VerticalScrollBar.Invoke((MethodInvoker)(() => VerticalScrollBar.Visible = true));
                }
                else
                {
                    VerticalScrollBar.Invoke((MethodInvoker)(() => VerticalScrollBar.Visible = false));
                }
                if (VerticalScrollBar.Value < 0)
                {
                    VerticalScrollBar.Invoke((MethodInvoker)(() => VerticalScrollBar.Minimum = 0));
                    VerticalScrollBar.Invoke((MethodInvoker)(() => VerticalScrollBar.Value = 0));
                }
                VerticalScrollBar.Invoke((MethodInvoker)(() => VerticalScrollBar.Maximum = ((_TotalRowsCount - _ViewPortRowsCount) * _VerticalScrollValueRatio) - 1));
            });
        }
        private async Task<List<object>> AsyncToggleSorting(int skipCount, int takeCount, CancellationToken token)
        {

           
            List<object> list = new List<object>();
            await Task.Run(() =>
            {
                try
                {
                    Thread.Sleep(500);

                    list = TooggleSorting(skipCount, takeCount)?.ToList();
                }
                catch (Exception ex)
                {

                }
            }, token);

            return list;





        }

    }
}
