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
using System.Windows.Shapes;

using System.Data.OleDb;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Reflection;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for EventWindow.xaml
    /// </summary>
    public partial class EventWindow : Window
    {
        MainWindow mw;
        int UserID;
        EventHorizonLINQ oe;
        public EventWindow ew;

        DateTime FormOpenStartTime;
        TimeSpan EventIsReadAfterTimeSpan = TimeSpan.FromSeconds(3);

        public Int32 ParentEventID;

        public EventWindow(MainWindow mw, EventHorizonLINQ oe, EventWindow ew = null)
        {
            InitializeComponent();

            this.mw = mw;
            this.oe = oe;
            this.ew = ew;
            this.UserID = oe.UserID;

            AddItemsToEventTypeComboBox();
            AddItemsToSourceComboBox();
            AddItemsToFrequencyComboBox();

            FormOpenStartTime = DateTime.Now;

            switch (oe.Source_Mode)
            {
                case EventWindowModes.ViewEvent:
                    GetOracleEvent();

                    if (UserID == XMLReaderWriter.UserID)
                        oe.Source_Mode = EventWindowModes.EditEvent;
                    else
                        oe.Source_Mode = EventWindowModes.ViewEvent;

                    break;
                case EventWindowModes.ViewReply:
                    GetOracleEvent();

                    if (UserID == XMLReaderWriter.UserID)
                        oe.Source_Mode = EventWindowModes.EditEvent;
                    else
                        oe.Source_Mode = EventWindowModes.ViewEvent;

                    break;
                case EventWindowModes.EditEvent:
                    GetOracleEvent();

                    if (UserID == XMLReaderWriter.UserID)
                        oe.Source_Mode = EventWindowModes.EditEvent;
                    else
                        oe.Source_Mode = EventWindowModes.ViewEvent;

                    break;
                case EventWindowModes.EditReply:
                    GetOracleEvent();

                    if (UserID == XMLReaderWriter.UserID)
                        oe.Source_Mode = EventWindowModes.EditEvent;
                    else
                        oe.Source_Mode = EventWindowModes.ViewEvent;

                    break;
                case EventWindowModes.NewEvent:
                    break;
                case EventWindowModes.NewReply:
                    break;
            }

            if (oe.Source_Mode == EventWindowModes.ViewEvent)
            {
                EventIDLabel.Content = oe.ID.ToString("D5");
                ParentEventIDLabel.Content = oe.Source_ParentEventID.ToString("D5");
                EventTitleLabel.Content = "View Event";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[oe.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, oe.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, oe.UserID);
                EventTypeComboBox.IsEnabled = false;
                SourceComboBox.IsEnabled = false;
                GenButton.Visibility = Visibility.Collapsed;
                DetailsTextBox.IsEnabled = false;
                FrequencyComboBox.IsEnabled = false;
                TargetUserIDComboBox.IsEnabled = false;
                StatusComboBox.IsEnabled = false;
                TargetDatePicker.IsEnabled = false;
                TargetTimeHoursPicker.IsEnabled = false;
                TargetTimeMinutesPicker.IsEnabled = false;
                TargetTimeButtonsStackPanel.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (oe.Source_Mode == EventWindowModes.ViewReply)
            {
                EventIDLabel.Content = oe.ID.ToString("D5");
                ParentEventIDLabel.Content = oe.Source_ParentEventID.ToString("D5");
                EventTitleLabel.Content = "View Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[oe.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, oe.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, oe.UserID);
                EventTypeComboBox.IsEnabled = false;
                SourceComboBox.IsEnabled = false;
                GenButton.Visibility = Visibility.Collapsed;
                DetailsTextBox.IsEnabled = false;
                FrequencyComboBox.IsEnabled = false;
                TargetUserIDComboBox.IsEnabled = false;
                StatusComboBox.IsEnabled = false;
                TargetDatePicker.IsEnabled = false;
                TargetTimeHoursPicker.IsEnabled = false;
                TargetTimeMinutesPicker.IsEnabled = false;
                TargetTimeButtonsStackPanel.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (oe.Source_Mode == EventWindowModes.EditEvent)
            {
                EventIDLabel.Content = oe.ID.ToString("D5");
                ParentEventIDLabel.Content = oe.Source_ParentEventID.ToString("D5");
                EventTitleLabel.Content = "Edit Event";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[oe.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, oe.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, oe.UserID);
                EventTypeComboBox.IsEnabled = true;
                SourceComboBox.IsEnabled = true;
                GenButton.Visibility = Visibility.Visible;
                DetailsTextBox.IsEnabled = true;
                FrequencyComboBox.IsEnabled = true;
                TargetUserIDComboBox.IsEnabled = true;
                StatusComboBox.IsEnabled = true;
                TargetDatePicker.IsEnabled = true;
                TargetTimeHoursPicker.IsEnabled = true;
                TargetTimeMinutesPicker.IsEnabled = true;
                TargetTimeButtonsStackPanel.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (oe.Source_Mode == EventWindowModes.EditReply)
            {
                EventIDLabel.Content = oe.Source_ParentEventID.ToString("D5");
                ParentEventIDLabel.Content = oe.Source_ParentEventID.ToString("D5");
                EventTitleLabel.Content = "Edit Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[oe.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, oe.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, oe.UserID);
                EventTypeComboBox.IsEnabled = true;
                SourceComboBox.IsEnabled = true;
                GenButton.Visibility = Visibility.Visible;
                DetailsTextBox.IsEnabled = true;
                FrequencyComboBox.IsEnabled = true;
                TargetUserIDComboBox.IsEnabled = true;
                StatusComboBox.IsEnabled = true;
                TargetDatePicker.IsEnabled = true;
                TargetTimeHoursPicker.IsEnabled = true;
                TargetTimeMinutesPicker.IsEnabled = true;
                TargetTimeButtonsStackPanel.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (oe.Source_Mode == EventWindowModes.NewEvent)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = oe.Source_ParentEventID.ToString("D5");
                EventTitleLabel.Content = "New Event";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[oe.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, oe.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                EventTypeComboBox.SelectedIndex = oe.EventTypeID;
                EventTypeComboBox.IsEnabled = true;
                SourceComboBox.SelectedIndex = oe.SourceID;
                SourceComboBox.IsEnabled = true;
                GenButton.Visibility = Visibility.Visible;
                DetailsTextBox.IsEnabled = true;
                FrequencyComboBox.SelectedIndex = oe.FrequencyID;
                FrequencyComboBox.IsEnabled = true;
                TargetUserIDComboBox.SelectedIndex = oe.TargetUserID;
                TargetUserIDComboBox.IsEnabled = true;
                StatusComboBox.IsEnabled = true;
                TargetDatePicker.IsEnabled = true;
                TargetTimeHoursPicker.IsEnabled = true;
                TargetTimeMinutesPicker.IsEnabled = true;
                TargetTimeButtonsStackPanel.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (oe.Source_Mode == EventWindowModes.NewReply)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = oe.Source_ParentEventID.ToString("D5");
                EventTitleLabel.Content = "New Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[oe.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, oe.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, oe.UserID);
                EventTypeComboBox.SelectedIndex = oe.EventTypeID;
                EventTypeComboBox.IsEnabled = false;
                SourceComboBox.SelectedIndex = oe.SourceID;
                SourceComboBox.IsEnabled = false;
                GenButton.Visibility = Visibility.Visible;
                DetailsTextBox.IsEnabled = true;
                FrequencyComboBox.SelectedIndex = oe.FrequencyID;
                FrequencyComboBox.IsEnabled = false;
                TargetUserIDComboBox.SelectedIndex = oe.TargetUserID;
                TargetUserIDComboBox.IsEnabled = false;
                StatusComboBox.IsEnabled = true;
                TargetDatePicker.IsEnabled = true;
                TargetTimeHoursPicker.IsEnabled = true;
                TargetTimeMinutesPicker.IsEnabled = true;
                TargetTimeButtonsStackPanel.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }

            AddUsersToTargetUsersComboBox();
            AddStatusToStatusComboBox();

            this.Owner = Application.Current.MainWindow;
        }
        
        private void AddItemsToEventTypeComboBox()
        {
            foreach (EventType e in XMLReaderWriter.EventTypesList)
            {
                EventTypeComboBox.Items.Add(EventHorizonEventTypes.GetEventTypeStackPanel(e));
            }

            if (oe.Source_Mode != EventWindowModes.EditEvent) EventTypeComboBox.SelectedIndex = 0;
        }
        
        private void AddItemsToSourceComboBox()
        {
            foreach (SourceType s in XMLReaderWriter.SourceTypesList)
            {
                SourceComboBox.Items.Add(EventHorizonSources.GetSourceStackPanel(s));
            }
            
            if (oe.Source_Mode != EventWindowModes.EditEvent) SourceComboBox.SelectedIndex = 0;
        }

        private void AddItemsToFrequencyComboBox()
        {
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Common_OneTime, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Common_Quarterly, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Common_SixMonthly, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Common_Yearly, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Common_TwoYearly, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Common_ThreeYearly, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Common_FiveYearly, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Minutes_01, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Minutes_05, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Minutes_10, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Minutes_30, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Hours_01, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Hours_02, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Hours_03, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Hours_08, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Hours_12, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Days_01, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Days_02, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Days_03, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Weeks_01, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Weeks_02, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Months_01, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Months_02, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Months_03, false));
            FrequencyComboBox.Items.Add(Frequency.GetFrequency(EventFrequencys.Months_09, false));

            if (oe.Source_Mode != EventWindowModes.EditEvent) FrequencyComboBox.SelectedIndex = 0;
        }

        private void AddUsersToTargetUsersComboBox()
        {
            foreach (User u in XMLReaderWriter.UsersList)
            {
                TargetUserIDComboBox.Items.Add(EventHorizonUsers.GetUserStackPanel(u));
            }
        }

        private void AddStatusToStatusComboBox()
        {
            StatusComboBox.Items.Add(StatusIcons.GetStatus(Statuses.Inactive));
            StatusComboBox.Items.Add(StatusIcons.GetStatus(Statuses.Active));
            StatusComboBox.Items.Add(StatusIcons.GetStatus(Statuses.ActiveNotified));
            StatusComboBox.Items.Add(StatusIcons.GetStatus(Statuses.ActiveNotifiedRead));
            StatusComboBox.Items.Add(StatusIcons.GetStatus(Statuses.ActiveNotifiedReadArchived));
        }

        private void GetOracleEvent()
        {
            EventTypeComboBox.SelectedIndex = oe.EventTypeID;
            SourceComboBox.SelectedIndex = oe.SourceID;
            DetailsTextBox.Text = oe.Details;
            FrequencyComboBox.SelectedIndex = oe.FrequencyID;
            TargetUserIDComboBox.SelectedIndex = oe.TargetUserID;
            StatusComboBox.SelectedIndex = oe.StatusID;

            DateTime tdt = oe.TargetDate;
            TargetDatePicker.SelectedDate = tdt;
            TargetTimeHoursPicker.Text = tdt.ToString("HH");
            TargetTimeMinutesPicker.Text = tdt.ToString("mm");

            UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, oe.UserID);
        }
        
        private void SetOracleEvent()
        {
            string DetailsSafeString = DetailsTextBox.Text.Replace("'", "''");

            DateTime? cdt = DateTime.Now;

            string ttt = string.Empty;

            //ttt = TargetTimeTextBox.Text;
            //ttt += ":00";

            ttt = TargetTimeHoursPicker.Text;
            ttt += ":";
            ttt += TargetTimeMinutesPicker.Text;
            ttt += ":00";

            DateTime ttimedt = DateTime.ParseExact(ttt, "HH:mm:ss", CultureInfo.InvariantCulture);

            DateTime tdtnow = DateTime.Now;

            DateTime tdt = DateTime.Now;

            if (ttt == "00:00:00")
                tdt = new DateTime(TargetDatePicker.SelectedDate.Value.Year, TargetDatePicker.SelectedDate.Value.Month, TargetDatePicker.SelectedDate.Value.Day, 0, 0, 0);
            else
                tdt = new DateTime(TargetDatePicker.SelectedDate.Value.Year, TargetDatePicker.SelectedDate.Value.Month, TargetDatePicker.SelectedDate.Value.Day, ttimedt.Hour, ttimedt.Minute, ttimedt.Second);

            oe.EventTypeID = EventTypeComboBox.SelectedIndex;
            oe.SourceID = SourceComboBox.SelectedIndex;
            oe.Details = DetailsTextBox.Text;
            oe.FrequencyID = FrequencyComboBox.SelectedIndex;
            oe.StatusID = StatusComboBox.SelectedIndex;
            oe.TargetDate = tdt;
            oe.TargetUserID = oe.UserID;
            oe.UserID = XMLReaderWriter.UserID;
            
            
            Console.Write("oe.TargetUserID = ");
            Console.WriteLine(oe.TargetUserID);
        }

        public int FrequencyID = EventFrequencys.Common_OneTime;
        
        private void FrequencyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FrequencyID = FrequencyComboBox.SelectedIndex;
        }
        
        public int StatusID = Statuses.Inactive;
        
        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusID = StatusComboBox.SelectedIndex;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9:]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Gen_ButtonClick(object sender, RoutedEventArgs e)
        {
            DetailsTextBox.Text = EventTypeName + " " + SourceTypeName;
        }
        
        public int TargetUserID = 0;
        
        private void TargetUserIDComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TargetUserID = TargetUserIDComboBox.SelectedIndex;

            Console.Write("TargetUserID=");
            Console.WriteLine(TargetUserID);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (FormOpenStartTime + EventIsReadAfterTimeSpan < DateTime.Now && oe.Source_Mode != EventWindowModes.NewEvent)
            {
                if (oe.TargetUserID == XMLReaderWriter.UserID)
                {
                    DataTableManagement.UpdateReadByMeID(oe.ID, ReadByMeModes.Yes);

                    if (StatusID == Statuses.ActiveNotified) DataTableManagement.UpdateStatusID(oe.ID, Statuses.ActiveNotifiedRead);
                }
            }
        }

        private void TargetTime_ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;

            int ButtonID = 0;

            bool success = Int32.TryParse(button.Tag.ToString(), out ButtonID);

            if (button != null && success)
            {
                switch (ButtonID)
                {
                    case TargetTimeButtons.Now:
                        TargetTimeHoursPicker.Text = DateTime.Now.ToString("HH");
                        TargetTimeMinutesPicker.Text = DateTime.Now.ToString("mm");
                        break;
                    case TargetTimeButtons.OneHour:
                        TargetTimeHoursPicker.Text = (DateTime.Now + TimeSpan.FromMinutes(60)).ToString("HH");
                        TargetTimeMinutesPicker.Text = (DateTime.Now + TimeSpan.FromMinutes(60)).ToString("mm");
                        break;
                    case TargetTimeButtons.TwoHours:
                        TargetTimeHoursPicker.Text = (DateTime.Now + TimeSpan.FromMinutes(120)).ToString("HH");
                        TargetTimeMinutesPicker.Text = (DateTime.Now + TimeSpan.FromMinutes(120)).ToString("mm");
                        break;
                    case TargetTimeButtons.ThreeHours:
                        TargetTimeHoursPicker.Text = (DateTime.Now + TimeSpan.FromMinutes(180)).ToString("HH");
                        TargetTimeMinutesPicker.Text = (DateTime.Now + TimeSpan.FromMinutes(180)).ToString("mm");
                        break;
                    case TargetTimeButtons.FourHours:
                        TargetTimeHoursPicker.Text = (DateTime.Now + TimeSpan.FromMinutes(240)).ToString("HH");
                        TargetTimeMinutesPicker.Text = (DateTime.Now + TimeSpan.FromMinutes(240)).ToString("mm");
                        break;
                    case TargetTimeButtons.FiveHours:
                        TargetTimeHoursPicker.Text = (DateTime.Now + TimeSpan.FromMinutes(300)).ToString("HH");
                        TargetTimeMinutesPicker.Text = (DateTime.Now + TimeSpan.FromMinutes(300)).ToString("mm");
                        break;
                }
            }
        }

        private void FormCloseButtons_ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;

            int ButtonID = 0;

            bool success = Int32.TryParse(button.Tag.ToString(), out ButtonID);

            if (button != null && success)
            {
                switch (ButtonID)
                {
                    case EventFormCloseButtons.Cancel:
                        mw.Status.Content = "Last log entry was cancelled.";
                        Close();
                        break;
                    case EventFormCloseButtons.Reply:
                        SetOracleEvent();
                        oe.Source_Mode = EventWindowModes.NewReply;
                        EventWindow nev = new EventWindow(mw, oe, this);
                        nev.Show();
                        nev.Left += 30;
                        nev.Top += 30;
                        break;
                    case EventFormCloseButtons.Save:
                        switch (oe.Source_Mode)
                        {
                            case EventWindowModes.NewEvent:
                                DataTableManagement.SaveEvent(this, oe, EventWindowModes.NewEvent);
                                break;
                            case EventWindowModes.EditEvent:
                                DataTableManagement.SaveEvent(this, oe, EventWindowModes.EditEvent);
                                break;
                            case EventWindowModes.NewReply:
                                DataTableManagement.SaveEvent(this, oe, EventWindowModes.NewReply);
                                break;
                            case EventWindowModes.EditReply:
                                SetOracleEvent();
                                DataTableManagement.SaveEvent(this, oe, EventWindowModes.EditReply);
                                break;
                        }
                        break;
                }
            }
        }

        String EventTypeName = string.Empty;

        private void EventTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is StackPanel))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            var selectedTag = ((StackPanel)EventTypeComboBox.SelectedItem).Tag.ToString();

            EventTypeName = selectedTag;
           
            Console.Write("** EventTypeComboBox_SelectedIndex = ");
            Console.WriteLine(EventTypeComboBox.SelectedIndex);
            Console.Write("** item.Tag EventTypeName = ");
            Console.WriteLine(EventTypeName);
        }

        String SourceTypeName = string.Empty;

        private void SourceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is StackPanel))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            var selectedTag = ((StackPanel)SourceComboBox.SelectedItem).Tag.ToString();

            SourceTypeName = selectedTag;

            Console.Write("** SourceComboBox_SelectedIndex = ");
            Console.WriteLine(SourceComboBox.SelectedIndex);
            Console.Write("** item.Tag SourceTypeName = ");
            Console.WriteLine(SourceTypeName);
        }
    }
}