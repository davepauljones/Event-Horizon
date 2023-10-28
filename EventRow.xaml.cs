using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;
using FontAwesome.WPF;

namespace Event_Horizon
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

            HeaderGrid.Visibility = Visibility.Collapsed;
            StatusBarGrid.Visibility = Visibility.Collapsed;
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
                    UnitCostTotalCostGrid.Visibility = Visibility.Visible;
                    UnitCostTextBlock.Text = eventHorizonLINQ.UnitCost.ToString("C2", CultureInfo.CurrentCulture);

                    double totalCost = eventHorizonLINQ.UnitCost * eventHorizonLINQ.Qty;
                    TotalCostTextBlock.Visibility = Visibility.Visible;
                    TotalCostTextBlock.Text = totalCost.ToString("C2", CultureInfo.CurrentCulture);

                    FrequencyGrid.Visibility = Visibility.Collapsed;
                    DiscountTextBlock.Visibility = Visibility.Visible;
                    double discount = eventHorizonLINQ.Discount / 100;
                    DiscountTextBlock.Text = discount.ToString("P", CultureInfo.InvariantCulture);

                    StatusGrid.Visibility = Visibility.Collapsed;
                    TotalTextBlock.Visibility = Visibility.Visible;       
                    double grandTotal = (eventHorizonLINQ.UnitCost * eventHorizonLINQ.Qty) - (eventHorizonLINQ.UnitCost * eventHorizonLINQ.Qty) * eventHorizonLINQ.Discount / 100;
                    TotalTextBlock.Text = grandTotal.ToString("C2", CultureInfo.CurrentCulture);

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
                BackgroundGrid.Background = new SolidColorBrush(Colors.Transparent);
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

            MainWindow.mw.eventHorizonLINQ_SelectedItem = (EventHorizonLINQ)item.Tag;

            Console.WriteLine();
            Console.WriteLine(">S>>EventRow RepliesListView_PreviewMouseDoubleClick<<<<");
            Console.WriteLine();

            Console.Write("item.Tag MainWindow.mw.eventHorizonLINQ_SelectedItem.Source_Mode = ");
            Console.WriteLine(MainWindow.mw.eventHorizonLINQ_SelectedItem.Source_Mode);

            Console.Write("item.Tag MainWindow.mw.eventHorizonLINQ_SelectedItem.ID = ");
            Console.WriteLine(MainWindow.mw.eventHorizonLINQ_SelectedItem.ID);

            Console.Write("item.Tag MainWindow.mw.eventHorizonLINQ_SelectedItem.ParentEventID = ");
            Console.WriteLine(MainWindow.mw.eventHorizonLINQ_SelectedItem.Source_ParentEventID);

            Console.Write("item.Tag MainWindow.mw.eventHorizonLINQ_SelectedItem.Details = ");
            Console.WriteLine(MainWindow.mw.eventHorizonLINQ_SelectedItem.Details);

            Console.WriteLine();
            Console.WriteLine(">F>>EventRow RepliesListView_PreviewMouseDoubleClick<<<<");
            Console.WriteLine();

            //MiscFunctions.ConsoleWriteEventHorizonLINQ(eventHorizonLINQ);

            if (MainWindow.mw.eventHorizonLINQ_SelectedItem != null)
            {
                if (MainWindow.mw.eventHorizonLINQ_SelectedItem.EventModeID == EventModes.NoteEvent)
                {
                    EventWindow eventWindow = new EventWindow(MainWindow.mw, EventWindowModes.ViewNote, MainWindow.mw.eventHorizonLINQ_SelectedItem);
                    eventWindow.Show();
                }
                else if (MainWindow.mw.eventHorizonLINQ_SelectedItem.EventModeID == EventModes.ReplyEvent)
                {
                    EventWindow eventWindow = new EventWindow(MainWindow.mw, EventWindowModes.ViewReply, MainWindow.mw.eventHorizonLINQ_SelectedItem);
                    eventWindow.Show();
                }
            }

            //prevents parent firing
            e.Handled = true;
        }

        public static EventRow CreateEventLogRow(EventHorizonLINQ eventHorizonLINQ)
        {
            EventRow eventRow = new EventRow(eventHorizonLINQ);

            eventRow.EventIDTextBlock.Text = eventHorizonLINQ.ID.ToString("D5");

            if (eventHorizonLINQ.EventModeID == EventModes.NoteEvent)
            {
                eventRow.EventTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.LightSlateGray);
                eventRow.EventTypeFontAwesomeIcon.Icon = FontAwesomeIcon.StickyNote;
                eventRow.EventTypeTextBlock.Text = "Note";
                eventRow.BackgroundGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f7f8f9"));
                eventRow.SourceTypeFontAwesomeIconBorder.Visibility = Visibility.Hidden;
            }
            else if (eventHorizonLINQ.EventModeID == EventModes.ReplyEvent)
            {
                eventRow.EventTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.LightSlateGray);
                eventRow.EventTypeFontAwesomeIcon.Icon = FontAwesomeIcon.Exchange;
                eventRow.EventTypeTextBlock.Text = "Reply";
                eventRow.BackgroundGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f7f8f9"));
                eventRow.SourceTypeFontAwesomeIconBorder.Visibility = Visibility.Hidden;
            }
            else
            {
                if (eventHorizonLINQ.EventTypeID < XMLReaderWriter.EventTypesList.Count)
                {
                    eventRow.EventTypeFontAwesomeIconBorder.Background = new SolidColorBrush(XMLReaderWriter.EventTypesList[eventHorizonLINQ.EventTypeID].Color);
                    eventRow.EventTypeFontAwesomeIcon.Icon = XMLReaderWriter.EventTypesList[eventHorizonLINQ.EventTypeID].Icon;
                    eventRow.EventTypeTextBlock.Text = XMLReaderWriter.EventTypesList[eventHorizonLINQ.EventTypeID].Name;
                    eventRow.BackgroundGrid.Background = new SolidColorBrush(Colors.Transparent);
                    eventRow.SourceTypeFontAwesomeIconBorder.Visibility = Visibility.Visible;
                }
                else
                {
                    eventRow.EventTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.White);
                    eventRow.EventTypeFontAwesomeIcon.Icon = FontAwesomeIcon.Question;
                    eventRow.EventTypeTextBlock.Text = "Error";
                    eventRow.BackgroundGrid.Background = new SolidColorBrush(Colors.Transparent);
                    eventRow.SourceTypeFontAwesomeIconBorder.Visibility = Visibility.Visible;
                }
            }

            eventRow.CreatedDateTimeTextBlock.Text = eventHorizonLINQ.CreationDate.ToString("dd/MM/y HH:mm");

            if (eventHorizonLINQ.EventModeID == EventModes.NoteEvent || eventHorizonLINQ.EventModeID == EventModes.ReplyEvent)
            {
                eventRow.SourceIDTextBlock.Text = "";
            }
            else
            {
                if (eventHorizonLINQ.SourceID < XMLReaderWriter.SourceTypesList.Count)
                {
                    eventRow.SourceTypeFontAwesomeIconBorder.Background = new SolidColorBrush(XMLReaderWriter.SourceTypesList[eventHorizonLINQ.SourceID].Color);
                    eventRow.SourceTypeFontAwesomeIcon.Icon = XMLReaderWriter.SourceTypesList[eventHorizonLINQ.SourceID].Icon;
                    eventRow.SourceIDTextBlock.Text = XMLReaderWriter.SourceTypesList[eventHorizonLINQ.SourceID].Name;
                    eventRow.BackgroundGrid.Background = new SolidColorBrush(Colors.Transparent);
                    eventRow.SourceTypeFontAwesomeIconBorder.Visibility = Visibility.Visible;

                    if (eventHorizonLINQ.SourceID == 0)
                    {
                        eventRow.SourceIDTextBlock.Text = string.Empty;
                        eventRow.SourceTypeFontAwesomeIconBorder.Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    eventRow.SourceTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.White);
                    eventRow.SourceTypeFontAwesomeIcon.Icon = FontAwesomeIcon.Question;
                    eventRow.SourceIDTextBlock.Text = "Error";
                    eventRow.BackgroundGrid.Background = new SolidColorBrush(Colors.Transparent);
                    eventRow.SourceTypeFontAwesomeIconBorder.Visibility = Visibility.Visible;
                }
            }

            eventRow.DetailsTextBlock.Text = eventHorizonLINQ.Details;

            if (eventHorizonLINQ.EventModeID != EventModes.NoteEvent && eventHorizonLINQ.EventModeID != EventModes.ReplyEvent) eventRow.FrequencyGrid.Children.Add(Frequency.GetFrequency(eventHorizonLINQ.FrequencyID));

            eventRow.StatusGrid.Children.Add(StatusIcons.GetStatus(eventHorizonLINQ.StatusID));

            if (eventHorizonLINQ.UserID < XMLReaderWriter.UsersList.Count)
            {
                eventRow.OriginUserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonLINQ.UserID].Color);
                eventRow.OriginUserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
            }
            else
            {
                eventRow.OriginUserEllipse.Fill = new SolidColorBrush(Colors.White);
                eventRow.OriginUserLabel.Content = eventHorizonLINQ.UserID;
            }

            if (eventHorizonLINQ.TargetUserID < XMLReaderWriter.UsersList.Count)
            {
                eventRow.TargetUserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonLINQ.TargetUserID].Color);
                eventRow.TargetUserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonLINQ.TargetUserID);
            }
            else
            {
                eventRow.TargetUserEllipse.Fill = new SolidColorBrush(Colors.White);
                eventRow.TargetUserLabel.Content = eventHorizonLINQ.TargetUserID;
            }

            if (eventHorizonLINQ.TargetUserID < XMLReaderWriter.UsersList.Count)
            {
                if (eventHorizonLINQ.TargetUserID > 0)
                    eventRow.TargetUserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonLINQ.TargetUserID);
                else
                {
                    eventRow.TargetUserLabel.Content = "★";
                    eventRow.TargetUserLabel.Margin = new Thickness(0, -3, 0, 0);
                    eventRow.TargetUserLabel.FontSize = 14;
                }
            }
            else
            {
                eventRow.TargetUserEllipse.Fill = new SolidColorBrush(Colors.White);
                eventRow.TargetUserLabel.Content = eventHorizonLINQ.TargetUserID;
            }

            string totalDaysString;

            if (eventHorizonLINQ.Attributes_TotalDays < 0)
            {
                if (eventHorizonLINQ.Attributes_TotalDays < -30)
                    totalDaysString = "30";
                else
                    totalDaysString = Math.Abs(eventHorizonLINQ.Attributes_TotalDays).ToString();
            }
            else
            {
                if (eventHorizonLINQ.Attributes_TotalDays > 30)
                    totalDaysString = "30";
                else
                    totalDaysString = eventHorizonLINQ.Attributes_TotalDays.ToString();
            }

            eventRow.TotalDaysEllipse.Fill = new SolidColorBrush(eventHorizonLINQ.Attributes_TotalDaysEllipseColor);
            eventRow.TotalDaysLabel.Content = totalDaysString;

            string targetDateTimeString = eventHorizonLINQ.TargetDate.ToString("dd/MM/y HH:mm");
            DateTime targetDateTime = DateTime.MinValue;
            if (DateTime.TryParse(targetDateTimeString, out targetDateTime))
            {
                if (targetDateTime.TimeOfDay == TimeSpan.Zero)
                    targetDateTimeString = targetDateTime.ToString("dd/MM/y");
                else
                    targetDateTimeString = targetDateTime.ToString("dd/MM/y HH:mm");
            }
            else
                Console.WriteLine("Unable to parse TargetDateTimeString '{0}'", targetDateTimeString);

            eventRow.TargetDateTimeTextBlock.Text = targetDateTimeString;

            eventRow.RepliesLabel.Content = eventHorizonLINQ.Attributes_Replies;

            if (eventHorizonLINQ.Attributes_Replies == 0)
            {
                eventRow.RepliesButton.Opacity = 0.1;
                eventRow.RepliesButton.IsEnabled = false;
            }

            if (eventHorizonLINQ.EventAttributeID == EventAttributes.LineItem)
            {
                eventRow.MorphEventRow();
            }

            eventRow.Tag = eventHorizonLINQ;

            return eventRow;
        }

    }
}