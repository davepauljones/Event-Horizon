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
        
        private bool _Tick = false;
        
        public void SyncTime()
        {
            TimeHourLabel.Content = DateTime.Now.ToString("hh");

            _Tick = !_Tick;
            
            if (_Tick)
                TimeColonLabel.Foreground = new SolidColorBrush(Colors.SlateGray);
            else
                TimeColonLabel.Foreground = new SolidColorBrush(Colors.DarkSlateGray);

            TimeMinuteLabel.Content = DateTime.Now.ToString("mm");
            AMPMLabel.Content = DateTime.Now.ToString("tt");
        }
    }
}