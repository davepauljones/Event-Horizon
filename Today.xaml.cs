using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for Today.xaml
    /// </summary>
    public partial class Today : UserControl
    {
        public Today()
        {
            InitializeComponent();

            Init();
        }
        private void Init()
        {
            //Init_RefreshTimer();

            SyncDate();
        }
        private void Init_RefreshTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {

        }
        public void SyncDate()
        {
            DayOfWeekLabel.Content = DateTime.Now.ToString("dddd").ToUpper();
            DayOfMonthLabel.Content = DateTime.Now.Day.ToString("");
            MonthOfYearLabel.Content = DateTime.Now.ToString("MMMM").ToUpper();
        }
    }
}
