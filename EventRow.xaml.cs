using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for EventRow.xaml
    /// </summary>
    public partial class EventRow : UserControl
    {
        EventHorizonLINQ eventHorizonLINQ;

        public EventRow(EventHorizonLINQ eventHorizonLINQ)
        {
            InitializeComponent();

            this.eventHorizonLINQ = eventHorizonLINQ;
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

            EventHorizonLINQ eventHorizonLINQ = (EventHorizonLINQ)item.Tag;

            Int32 repliesLogListViewTagged = Convert.ToInt32(eventHorizonLINQ.ID);

            Console.Write("RepliesListView_PreviewMouseDoubleClick  eventHorizonLINQ.ParentEventID = ");
            Console.WriteLine(eventHorizonLINQ.Source_ParentEventID);

            Console.Write("RepliesListView_PreviewMouseDoubleClick = ");
            Console.WriteLine(eventHorizonLINQ.Details);

            if (repliesLogListViewTagged > 0)
            {
                EventWindow eventWindow = new EventWindow(MainWindow.mw, eventHorizonLINQ);
                eventWindow.Show();
            }

            //prevents parent firing
            e.Handled = true;
        }
    }
}