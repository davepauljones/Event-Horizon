using System;
using System.Windows.Controls;

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
            SyncDate();
        }

        public void SyncDate()
        {
            DayOfWeekLabel.Content = DateTime.Now.ToString("dddd").ToUpper();
            DayOfMonthLabel.Content = DateTime.Now.Day.ToString("");
            MonthOfYearLabel.Content = DateTime.Now.ToString("MMMM").ToUpper();
        }
    }
}