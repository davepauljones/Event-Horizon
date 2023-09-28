using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;

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

        public void MorphEventRow()
        {
            switch (eventHorizonLINQ.EventAttributeID)
            {
                case EventAttributes.Standard:
                    //doo nothing
                    break;
                case EventAttributes.LineItem:
                    //change UI to suit LineItem
                    EventTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.Green);
                    EventTypeFontAwesomeIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Dollar;
                    EventTypeTextBlock.Text = "Line Item";

                    TargetUserTokenGrid.Visibility = Visibility.Collapsed;
                    TotalDaysTokenGrid.Visibility = Visibility.Collapsed;

                    TargetDateTimeTextBlock.Visibility = Visibility.Collapsed;
                    QtyTextBlock.Visibility = Visibility.Visible;
                    QtyTextBlock.Text = eventHorizonLINQ.Qty.ToString();

                    SourceIDTextBlock.Visibility = Visibility.Collapsed;
                    UnitCostTextBlock.Visibility = Visibility.Visible;
                    UnitCostTextBlock.Text = eventHorizonLINQ.UnitCost.ToString("C2", CultureInfo.CurrentCulture);

                    FrequencyGrid.Visibility = Visibility.Collapsed;
                    DiscountTextBlock.Visibility = Visibility.Visible;
                    double discount = eventHorizonLINQ.Discount / 100;
                    DiscountTextBlock.Text = discount.ToString("P", CultureInfo.InvariantCulture);

                    StatusGrid.Visibility = Visibility.Collapsed;
                    GrandTotalTextBlock.Visibility = Visibility.Visible;       
                    double grandTotal = (eventHorizonLINQ.UnitCost * eventHorizonLINQ.Qty) - ((eventHorizonLINQ.UnitCost * eventHorizonLINQ.Qty) * eventHorizonLINQ.Discount / 100);
                    GrandTotalTextBlock.Text = grandTotal.ToString("C2", CultureInfo.CurrentCulture);

                    break;
                case EventAttributes.FooBar:
                    //change UI to suit FooBar 
                    break;
            }
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

            MainWindow.mw.eventHorizonLINQ = (EventHorizonLINQ)item.Tag;

            Console.WriteLine();
            Console.WriteLine(">S>>EventRow RepliesListView_PreviewMouseDoubleClick<<<<");
            Console.WriteLine();

            Console.Write("item.Tag MainWindow.mw.eventHorizonLINQ.Source_Mode = ");
            Console.WriteLine(MainWindow.mw.eventHorizonLINQ.Source_Mode);

            Console.Write("item.Tag MainWindow.mw.eventHorizonLINQ.ID = ");
            Console.WriteLine(MainWindow.mw.eventHorizonLINQ.ID);

            Console.Write("item.Tag MainWindow.mw.eventHorizonLINQ.ParentEventID = ");
            Console.WriteLine(MainWindow.mw.eventHorizonLINQ.Source_ParentEventID);

            Console.Write("item.Tag MainWindow.mw.eventHorizonLINQ.Details = ");
            Console.WriteLine(MainWindow.mw.eventHorizonLINQ.Details);

            Console.WriteLine();
            Console.WriteLine(">F>>EventRow RepliesListView_PreviewMouseDoubleClick<<<<");
            Console.WriteLine();

            //MiscFunctions.ConsoleWriteEventHorizonLINQ(eventHorizonLINQ);

            if (MainWindow.mw.eventHorizonLINQ != null)
            {
                if (MainWindow.mw.eventHorizonLINQ.EventModeID == EventModes.NoteEvent)
                {
                    EventWindow eventWindow = new EventWindow(MainWindow.mw, EventWindowModes.ViewNote, MainWindow.mw.eventHorizonLINQ);
                    eventWindow.Show();
                }
                else if (MainWindow.mw.eventHorizonLINQ.EventModeID == EventModes.ReplyEvent)
                {
                    EventWindow eventWindow = new EventWindow(MainWindow.mw, EventWindowModes.ViewReply, MainWindow.mw.eventHorizonLINQ);
                    eventWindow.Show();
                }
            }

            //prevents parent firing
            e.Handled = true;
        }
    }
}