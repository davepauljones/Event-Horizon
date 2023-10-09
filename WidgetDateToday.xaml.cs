using System;
using System.Windows.Controls;

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for WidgetDateToday.xaml
    /// </summary>
    public partial class WidgetDateToday : UserControl
    {
        public WidgetDateToday()
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