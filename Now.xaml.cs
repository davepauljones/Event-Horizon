using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for Now.xaml
    /// </summary>
    public partial class Now : UserControl
    {
        public Now()
        {
            InitializeComponent();

            Init();
        }
        private void Init()
        {
            SyncTime();
        }
        private void Init_RefreshTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            SyncTime();
        }
        
        private bool tick = false;
        
        public void SyncTime()
        {
            TimeHourLabel.Content = DateTime.Now.ToString("hh");

            tick = !tick;
            
            if (tick)
                TimeColonLabel.Foreground = new SolidColorBrush(Colors.SlateGray);
            else
                TimeColonLabel.Foreground = new SolidColorBrush(Colors.DarkSlateGray);

            TimeMinuteLabel.Content = DateTime.Now.ToString("mm");
            AMPMLabel.Content = DateTime.Now.ToString("tt");
        }
    }
}