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
using FontAwesome.WPF;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for EventRow.xaml
    /// </summary>
    public partial class EventRow : UserControl
    {
        EventHorizonLINQ oe;

        public EventRow(EventHorizonLINQ oe)
        {
            InitializeComponent();

            this.oe = oe;
        }

        private void RepliesButton_Click(object sender, RoutedEventArgs e)
        {
            if (RepliesListView.Visibility == Visibility.Visible)
            {
                BackgroundGrid.Background = new SolidColorBrush(Colors.White);
                RepliesListView.Visibility = Visibility.Collapsed;
            }
            else
            {
                BackgroundGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f7f8f9"));
                RepliesListView.Visibility = Visibility.Visible;
            }
        }

        private void RepliesListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is EventRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            EventRow item = (EventRow)dep;

            Console.Write("Replies item = ");
            Console.WriteLine(item.Tag);

            Int32 RepliesLogListViewTagged = 0;
            EventHorizonLINQ oe = (EventHorizonLINQ)item.Tag;
            RepliesLogListViewTagged = Convert.ToInt32(oe.ID);

            Console.Write("RepliesListView_PreviewMouseDoubleClick  oe.ParentEventID = ");
            Console.WriteLine(oe.Source_ParentEventID);

            Console.Write("RepliesListView_PreviewMouseDoubleClick = ");
            Console.WriteLine(oe.Details);

            if (RepliesLogListViewTagged > 0)
            {
                EventWindow eev = new EventWindow(MainWindow.mw, oe);
                eev.Show();
            }

            //prevents parent firing
            e.Handled = true;
        }
    }
}