using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;
using System.Text.RegularExpressions;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for EventWindow.xaml
    /// </summary>
    public partial class EventWindow : Window
    {
        MainWindow mainWindow;
        int userID;
        EventHorizonLINQ eventHorizonLINQ;
        public EventWindow eventWindow;

        DateTime formOpenStartTime;
        TimeSpan eventIsReadAfterTimeSpan = TimeSpan.FromSeconds(3);

        public Int32 ParentEventID;

        public EventWindow(MainWindow mainWindow, EventHorizonLINQ eventHorizonLINQ, EventWindow eventWindow = null)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            this.eventHorizonLINQ = (EventHorizonLINQ)eventHorizonLINQ.Clone();
            this.eventWindow = eventWindow;
            this.userID = eventHorizonLINQ.UserID;

            AddItemsToEventTypeComboBox();
            AddItemsToSourceComboBox();
            AddItemsToFrequencyComboBox();

            formOpenStartTime = DateTime.Now;

            switch (eventHorizonLINQ.Source_Mode)
            {
                case EventWindowModes.ViewMainEvent:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID)
                        eventHorizonLINQ.Source_Mode = EventWindowModes.EditMainEvent;
                    else
                        eventHorizonLINQ.Source_Mode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.ViewReplyNote:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID)
                        eventHorizonLINQ.Source_Mode = EventWindowModes.EditReplyNote;
                    else
                        eventHorizonLINQ.Source_Mode = EventWindowModes.ViewReplyNote;

                    break;
                case EventWindowModes.EditMainEvent:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID)
                        eventHorizonLINQ.Source_Mode = EventWindowModes.EditMainEvent;
                    else
                        eventHorizonLINQ.Source_Mode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.EditReplyNote:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID)
                        eventHorizonLINQ.Source_Mode = EventWindowModes.EditReplyNote;
                    else
                        eventHorizonLINQ.Source_Mode = EventWindowModes.ViewReplyNote;

                    break;
                case EventWindowModes.NewEvent:
                case EventWindowModes.NewNote:
                case EventWindowModes.NewReply:
                    break;
            }

            if (eventHorizonLINQ.Source_Mode == EventWindowModes.ViewMainEvent)
            {
                EventIDLabel.Content = eventHorizonLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Main Event";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
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
                TargetDayButtonsStackPanel.Visibility = Visibility.Collapsed;
                TargetTimeButtonsStackPanel.Visibility = Visibility.Collapsed;
                AddNoteButton.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (eventHorizonLINQ.Source_Mode == EventWindowModes.ViewReplyNote)
            {
                EventIDLabel.Content = eventHorizonLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Reply Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
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
                TargetDayButtonsStackPanel.Visibility = Visibility.Collapsed;
                TargetTimeButtonsStackPanel.Visibility = Visibility.Collapsed;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (eventHorizonLINQ.Source_Mode == EventWindowModes.EditMainEvent)
            {
                EventIDLabel.Content = eventHorizonLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Main Event";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
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
                TargetDayButtonsStackPanel.Visibility = Visibility.Visible;
                TargetTimeButtonsStackPanel.Visibility = Visibility.Visible;
                AddNoteButton.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (eventHorizonLINQ.Source_Mode == EventWindowModes.EditReplyNote)
            {
                EventIDLabel.Content = eventHorizonLINQ.Source_ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Reply Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
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
                TargetDayButtonsStackPanel.Visibility = Visibility.Visible;
                TargetTimeButtonsStackPanel.Visibility = Visibility.Visible;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (eventHorizonLINQ.Source_Mode == EventWindowModes.NewEvent)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Event";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[XMLReaderWriter.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                EventTypeComboBox.SelectedIndex = eventHorizonLINQ.EventTypeID;
                EventTypeComboBox.IsEnabled = true;
                SourceComboBox.SelectedIndex = eventHorizonLINQ.SourceID;
                SourceComboBox.IsEnabled = true;
                GenButton.Visibility = Visibility.Visible;
                DetailsTextBox.IsEnabled = true;
                FrequencyComboBox.SelectedIndex = eventHorizonLINQ.FrequencyID;
                FrequencyComboBox.IsEnabled = true;
                TargetUserIDComboBox.SelectedIndex = eventHorizonLINQ.TargetUserID;
                TargetUserIDComboBox.IsEnabled = true;
                StatusComboBox.SelectedIndex = Statuses.Active;
                StatusComboBox.IsEnabled = true;
                TargetDatePicker.IsEnabled = true;
                TargetDatePicker.SelectedDate = DateTime.Now + TimeSpan.FromDays(7);
                TargetTimeHoursPicker.IsEnabled = true;
                TargetTimeMinutesPicker.IsEnabled = true;
                TargetDayButtonsStackPanel.Visibility = Visibility.Visible;
                TargetTimeButtonsStackPanel.Visibility = Visibility.Visible;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (eventHorizonLINQ.Source_Mode == EventWindowModes.NewNote)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[XMLReaderWriter.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                EventTypeComboBox.SelectedIndex = eventHorizonLINQ.EventTypeID;
                EventTypeComboBox.IsEnabled = false;
                SourceComboBox.SelectedIndex = eventHorizonLINQ.SourceID;
                SourceComboBox.IsEnabled = false;
                GenButton.Visibility = Visibility.Visible;
                DetailsTextBox.IsEnabled = true;
                FrequencyComboBox.SelectedIndex = eventHorizonLINQ.FrequencyID;
                FrequencyComboBox.IsEnabled = false;
                TargetUserIDComboBox.SelectedIndex = 0;
                TargetUserIDComboBox.IsEnabled = true;
                StatusComboBox.SelectedIndex = Statuses.Active;
                StatusComboBox.IsEnabled = true;
                TargetDatePicker.IsEnabled = true;
                TargetDatePicker.SelectedDate = DateTime.Now + TimeSpan.FromDays(7);
                TargetTimeHoursPicker.IsEnabled = true;
                TargetTimeMinutesPicker.IsEnabled = true;
                TargetDayButtonsStackPanel.Visibility = Visibility.Visible;
                TargetTimeButtonsStackPanel.Visibility = Visibility.Visible;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (eventHorizonLINQ.Source_Mode == EventWindowModes.NewReply)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[XMLReaderWriter.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                EventTypeComboBox.SelectedIndex = eventHorizonLINQ.EventTypeID;
                EventTypeComboBox.IsEnabled = false;
                SourceComboBox.SelectedIndex = eventHorizonLINQ.SourceID;
                SourceComboBox.IsEnabled = false;
                GenButton.Visibility = Visibility.Visible;
                DetailsTextBox.IsEnabled = true;
                FrequencyComboBox.SelectedIndex = eventHorizonLINQ.FrequencyID;
                FrequencyComboBox.IsEnabled = false;
                TargetUserIDComboBox.SelectedIndex = eventHorizonLINQ.TargetUserID;
                TargetUserIDComboBox.IsEnabled = false;
                StatusComboBox.SelectedIndex = Statuses.Active;
                StatusComboBox.IsEnabled = true;
                TargetDatePicker.IsEnabled = true;
                TargetDatePicker.SelectedDate = DateTime.Now + TimeSpan.FromDays(7);
                TargetTimeHoursPicker.IsEnabled = true;
                TargetTimeMinutesPicker.IsEnabled = true;
                TargetDayButtonsStackPanel.Visibility = Visibility.Visible;
                TargetTimeButtonsStackPanel.Visibility = Visibility.Visible;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }

            AddUsersToTargetUsersComboBox();
            AddStatusToStatusComboBox();

            this.Owner = Application.Current.MainWindow;
        }
        
        private void AddItemsToEventTypeComboBox()
        {
            foreach (EventType eventType in XMLReaderWriter.EventTypesList)
            {
                EventTypeComboBox.Items.Add(EventHorizonEventTypes.GetEventTypeStackPanel(eventType));
            }

            if (eventHorizonLINQ.Source_Mode != EventWindowModes.EditMainEvent || eventHorizonLINQ.Source_Mode != EventWindowModes.EditReplyNote) EventTypeComboBox.SelectedIndex = 0;
        }
        
        private void AddItemsToSourceComboBox()
        {
            foreach (SourceType sourceType in XMLReaderWriter.SourceTypesList)
            {
                SourceComboBox.Items.Add(EventHorizonSources.GetSourceStackPanel(sourceType));
            }
            
            if (eventHorizonLINQ.Source_Mode != EventWindowModes.EditMainEvent || eventHorizonLINQ.Source_Mode != EventWindowModes.EditReplyNote) SourceComboBox.SelectedIndex = 0;
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

            if (eventHorizonLINQ.Source_Mode != EventWindowModes.EditMainEvent || eventHorizonLINQ.Source_Mode != EventWindowModes.EditReplyNote) FrequencyComboBox.SelectedIndex = 0;
        }

        private void AddUsersToTargetUsersComboBox()
        {
            foreach (User user in XMLReaderWriter.UsersList)
            {
                TargetUserIDComboBox.Items.Add(EventHorizonUsers.GetUserStackPanel(user));
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
            EventTypeComboBox.SelectedIndex = eventHorizonLINQ.EventTypeID;
            SourceComboBox.SelectedIndex = eventHorizonLINQ.SourceID;
            DetailsTextBox.Text = eventHorizonLINQ.Details;
            FrequencyComboBox.SelectedIndex = eventHorizonLINQ.FrequencyID;
            TargetUserIDComboBox.SelectedIndex = eventHorizonLINQ.TargetUserID;
            StatusComboBox.SelectedIndex = eventHorizonLINQ.StatusID;

            DateTime targetDateTimet = eventHorizonLINQ.TargetDate;
            TargetDatePicker.SelectedDate = targetDateTimet;
            TargetTimeHoursPicker.Text = targetDateTimet.ToString("HH");
            TargetTimeMinutesPicker.Text = targetDateTimet.ToString("mm");

            UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
        }
        
        private void SetOracleEvent()
        {
            string detailsSafeString = DetailsTextBox.Text.Replace("'", "''");

            string targetTimeString = TargetTimeHoursPicker.Text;
            targetTimeString += ":";
            targetTimeString += TargetTimeMinutesPicker.Text;
            targetTimeString += ":00";

            DateTime targetTimeDateTime = DateTime.ParseExact(targetTimeString, "HH:mm:ss", CultureInfo.InvariantCulture);

            DateTime targetTimeDateTimeNow = DateTime.Now;

            DateTime targetDateTime = DateTime.Now;

            if (targetTimeString == "00:00:00")
                targetDateTime = new DateTime(TargetDatePicker.SelectedDate.Value.Year, TargetDatePicker.SelectedDate.Value.Month, TargetDatePicker.SelectedDate.Value.Day, 0, 0, 0);
            else
                targetDateTime = new DateTime(TargetDatePicker.SelectedDate.Value.Year, TargetDatePicker.SelectedDate.Value.Month, TargetDatePicker.SelectedDate.Value.Day, targetTimeDateTime.Hour, targetTimeDateTime.Minute, targetTimeDateTime.Second);

            eventHorizonLINQ.EventTypeID = EventTypeComboBox.SelectedIndex;
            eventHorizonLINQ.SourceID = SourceComboBox.SelectedIndex;
            eventHorizonLINQ.Details = detailsSafeString;
            eventHorizonLINQ.FrequencyID = FrequencyComboBox.SelectedIndex;
            eventHorizonLINQ.StatusID = StatusComboBox.SelectedIndex;
            eventHorizonLINQ.TargetDate = targetDateTime;

            switch (eventHorizonLINQ.Source_Mode)
            {
                case EventWindowModes.ViewMainEvent:                    
                    break;
                case EventWindowModes.ViewReplyNote:                   
                    break;
                case EventWindowModes.EditMainEvent:
                    eventHorizonLINQ.TargetUserID = TargetUserIDComboBox.SelectedIndex;                    
                    break;
                case EventWindowModes.EditReplyNote:
                    eventHorizonLINQ.TargetUserID = TargetUserIDComboBox.SelectedIndex;
                    break;
                case EventWindowModes.NewEvent:
                    eventHorizonLINQ.TargetUserID = TargetUserIDComboBox.SelectedIndex;
                    eventHorizonLINQ.UserID = XMLReaderWriter.UserID;
                    break;
                case EventWindowModes.NewNote:
                    eventHorizonLINQ.TargetUserID = TargetUserIDComboBox.SelectedIndex;
                    eventHorizonLINQ.UserID = XMLReaderWriter.UserID;
                    break;
                case EventWindowModes.NewReply:
                    eventHorizonLINQ.TargetUserID = eventHorizonLINQ.UserID;
                    eventHorizonLINQ.UserID = XMLReaderWriter.UserID;
                    break;
                default:                   
                    break;
            }

            Console.Write("eventHorizonLINQ.TargetUserID = ");
            Console.WriteLine(eventHorizonLINQ.TargetUserID);
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
            if (formOpenStartTime + eventIsReadAfterTimeSpan < DateTime.Now && eventHorizonLINQ.Source_Mode != EventWindowModes.NewEvent)
            {
                if (eventHorizonLINQ.TargetUserID == XMLReaderWriter.UserID)
                {
                    DataTableManagement.UpdateReadByMeID(eventHorizonLINQ.ID, ReadByMeModes.Yes);

                    if (StatusID == Statuses.ActiveNotified) DataTableManagement.UpdateStatusID(eventHorizonLINQ.ID, Statuses.ActiveNotifiedRead);
                }
            }
        }

        private void FormCloseButtons_ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;

            int buttonID = 0;

            bool success = Int32.TryParse(button.Tag.ToString(), out buttonID);

            if (button != null && success)
            {
                switch (buttonID)
                {
                    case EventFormCloseButtons.Cancel:
                        mainWindow.Status.Content = "Last log entry was cancelled.";
                        Close();
                        break;
                    case EventFormCloseButtons.Note:
                        SetOracleEvent();
                        eventHorizonLINQ.Source_Mode = EventWindowModes.NewNote;
                        EventWindow nevn = new EventWindow(mainWindow, eventHorizonLINQ, this);
                        nevn.Show();
                        nevn.Left += 30;
                        nevn.Top += 30;
                        break;
                    case EventFormCloseButtons.Reply:
                        SetOracleEvent();
                        eventHorizonLINQ.Source_Mode = EventWindowModes.NewReply;
                        EventWindow nev = new EventWindow(mainWindow, eventHorizonLINQ, this);
                        nev.Show();
                        nev.Left += 30;
                        nev.Top += 30;
                        break;
                    case EventFormCloseButtons.Save:
                        switch (eventHorizonLINQ.Source_Mode)
                        {
                            case EventWindowModes.ViewMainEvent:
                                SetOracleEvent();
                                DataTableManagement.SaveEvent(this, eventHorizonLINQ, EventWindowModes.ViewMainEvent);
                                break;
                            case EventWindowModes.ViewReplyNote:
                                SetOracleEvent();
                                DataTableManagement.SaveEvent(this, eventHorizonLINQ, EventWindowModes.ViewReplyNote);
                                break;
                            case EventWindowModes.EditMainEvent:
                                SetOracleEvent();
                                DataTableManagement.SaveEvent(this, eventHorizonLINQ, EventWindowModes.EditMainEvent);
                                break;
                            case EventWindowModes.EditReplyNote:
                                SetOracleEvent();
                                DataTableManagement.SaveEvent(this, eventHorizonLINQ, EventWindowModes.EditReplyNote);
                                break;
                            case EventWindowModes.NewEvent:
                                DataTableManagement.SaveEvent(this, eventHorizonLINQ, EventWindowModes.NewEvent);
                                break;
                            case EventWindowModes.NewNote:
                                DataTableManagement.SaveEvent(this, eventHorizonLINQ, EventWindowModes.NewNote);
                                break;
                            case EventWindowModes.NewReply:
                                DataTableManagement.SaveEvent(this, eventHorizonLINQ, EventWindowModes.NewReply);
                                break;
                        }
                        break;
                }
                Console.Write("eventHorizonLINQ.Source_Mode = ");
                Console.WriteLine(eventHorizonLINQ.Source_Mode);
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

        private void DaysRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = e.OriginalSource as RadioButton;

            int buttonID = 0;

            bool success = Int32.TryParse(radioButton.Tag.ToString(), out buttonID);

            if (radioButton != null && success)
            {
                switch (buttonID)
                {
                    case TargetDateButtons.Today:
                        TargetDatePicker.SelectedDate = DateTime.Now;
                        break;
                    case TargetDateButtons.OneDay:
                        TargetDatePicker.SelectedDate = DateTime.Now + TimeSpan.FromDays(1);
                        break;
                    case TargetDateButtons.TwoDays:
                        TargetDatePicker.SelectedDate = DateTime.Now + TimeSpan.FromDays(2);
                        break;
                    case TargetDateButtons.ThreeDays:
                        TargetDatePicker.SelectedDate = DateTime.Now + TimeSpan.FromDays(3);
                        break;
                    case TargetDateButtons.FiveDays:
                        TargetDatePicker.SelectedDate = DateTime.Now + TimeSpan.FromDays(5);
                        break;
                    case TargetDateButtons.SevenDays:
                        TargetDatePicker.SelectedDate = DateTime.Now + TimeSpan.FromDays(7);
                        break;
                }
            }
        }

        private void HoursRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = e.OriginalSource as RadioButton;

            int buttonID = 0;

            bool success = Int32.TryParse(radioButton.Tag.ToString(), out buttonID);

            if (radioButton != null && success)
            {
                switch (buttonID)
                {
                    case TargetTimeButtons.Now:
                        TargetDatePicker.SelectedDate = DateTime.Now;
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
    }
}