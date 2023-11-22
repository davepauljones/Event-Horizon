using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for EventWindow.xaml
    /// </summary>
    public partial class EventWindow : Window
    {
        MainWindow mainWindow;
        int eventWindowMode;
        int userID;
        EventHorizonLINQ eventHorizonLINQ;
        public EventWindow eventWindow;

        DateTime formOpenStartTime;
        TimeSpan eventIsReadAfterTimeSpan = TimeSpan.FromSeconds(3);

        public Int32 ParentEventID;

        public EventWindow(MainWindow mainWindow, int eventWindowMode, EventHorizonLINQ eventHorizonLINQ, EventWindow eventWindow = null)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            this.eventWindowMode = eventWindowMode;
            this.eventHorizonLINQ = (EventHorizonLINQ)eventHorizonLINQ.Clone();
            this.eventWindow = eventWindow;
            this.userID = eventHorizonLINQ.UserID;

            AddItemsToEventTypeComboBox();
            AddItemsToEventAttributeComboBox();
            AddItemsToSourceComboBox();
            AddItemsToFrequencyComboBox();

            formOpenStartTime = DateTime.Now;

            switch (eventWindowMode)
            {
                case EventWindowModes.ViewMainEvent:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID)
                        eventWindowMode = EventWindowModes.EditMainEvent;
                    else
                        eventWindowMode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.ViewNote:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID)
                        eventWindowMode = EventWindowModes.EditNote;
                    else
                        eventWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.ViewReply:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID)
                        eventWindowMode = EventWindowModes.EditReply;
                    else
                        eventWindowMode = EventWindowModes.ViewReply;

                    break;
                case EventWindowModes.EditMainEvent:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID)
                        eventWindowMode = EventWindowModes.EditMainEvent;
                    else
                        eventWindowMode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.EditNote:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID)
                        eventWindowMode = EventWindowModes.EditNote;
                    else
                        eventWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.EditReply:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID)
                        eventWindowMode = EventWindowModes.EditReply;
                    else
                        eventWindowMode = EventWindowModes.ViewReply;

                    break;
                case EventWindowModes.NewEvent:
                case EventWindowModes.NewNote:
                case EventWindowModes.NewReply:
                    eventHorizonLINQ.TargetUserID = eventHorizonLINQ.UserID;
                    eventHorizonLINQ.UserID = XMLReaderWriter.UserID;
                    break;
            }

            if (eventWindowMode == EventWindowModes.ViewMainEvent)
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
                DetailsTextBox.IsReadOnly = true;
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
            else if (eventWindowMode == EventWindowModes.ViewNote)
            {
                EventIDLabel.Content = eventHorizonLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
                EventTypeComboBox.IsEnabled = false;
                SourceComboBox.IsEnabled = false;
                GenButton.Visibility = Visibility.Collapsed;
                DetailsTextBox.IsReadOnly = true;
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
            else if (eventWindowMode == EventWindowModes.ViewReply)
            {
                EventIDLabel.Content = eventHorizonLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
                EventTypeComboBox.IsEnabled = false;
                SourceComboBox.IsEnabled = false;
                GenButton.Visibility = Visibility.Collapsed;
                DetailsTextBox.IsReadOnly = true;
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
            else if (eventWindowMode == EventWindowModes.EditMainEvent)
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
                DetailsTextBox.IsReadOnly = false;
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
            else if (eventWindowMode == EventWindowModes.EditNote)
            {
                EventIDLabel.Content = eventHorizonLINQ.Source_ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
                EventTypeComboBox.IsEnabled = true;
                SourceComboBox.IsEnabled = true;
                GenButton.Visibility = Visibility.Visible;
                DetailsTextBox.IsReadOnly = false;
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
            else if (eventWindowMode == EventWindowModes.EditReply)
            {
                EventIDLabel.Content = eventHorizonLINQ.Source_ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
                EventTypeComboBox.IsEnabled = true;
                SourceComboBox.IsEnabled = true;
                GenButton.Visibility = Visibility.Visible;
                DetailsTextBox.IsReadOnly = false;
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
            else if (eventWindowMode == EventWindowModes.NewEvent)
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
                DetailsTextBox.IsReadOnly = false;
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
            else if (eventWindowMode == EventWindowModes.NewNote)
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
                DetailsTextBox.IsReadOnly = false;
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
            else if (eventWindowMode == EventWindowModes.NewReply)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
                EventTypeComboBox.SelectedIndex = eventHorizonLINQ.EventTypeID;
                EventTypeComboBox.IsEnabled = false;
                SourceComboBox.SelectedIndex = eventHorizonLINQ.SourceID;
                SourceComboBox.IsEnabled = false;
                GenButton.Visibility = Visibility.Visible;
                DetailsTextBox.IsReadOnly = false;
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

            if (eventWindowMode != EventWindowModes.EditMainEvent || eventWindowMode != EventWindowModes.EditNote || eventWindowMode != EventWindowModes.EditReply) EventTypeComboBox.SelectedIndex = 0;
        }

        private void AddItemsToEventAttributeComboBox()
        {
            foreach (AttributeType attributeType in XMLReaderWriter.AttributeTypesList)
            {
                EventAttributeComboBox.Items.Add(EventHorizonAttributes.GetAttributeStackPanel(attributeType));
            }

            if (eventWindowMode != EventWindowModes.EditMainEvent || eventWindowMode != EventWindowModes.EditNote || eventWindowMode != EventWindowModes.EditReply) EventAttributeComboBox.SelectedIndex = 0;
        }

        private void AddItemsToSourceComboBox()
        {
            foreach (SourceType sourceType in XMLReaderWriter.SourceTypesList)
            {
                SourceComboBox.Items.Add(EventHorizonSources.GetSourceStackPanel(sourceType));
            }
            
            if (eventWindowMode != EventWindowModes.EditMainEvent || eventWindowMode != EventWindowModes.EditNote || eventWindowMode != EventWindowModes.EditReply) SourceComboBox.SelectedIndex = 0;
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

            if (eventWindowMode != EventWindowModes.EditMainEvent || eventWindowMode != EventWindowModes.EditNote || eventWindowMode != EventWindowModes.EditReply) FrequencyComboBox.SelectedIndex = 0;
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

        private double CalcGrandTotal()
        {
            double unitCost;
            if (double.TryParse(UnitCostTextBox.Text, out unitCost))
            {
                eventHorizonLINQ.UnitCost = unitCost;
                Console.Write("eventHorizonLINQ.TargetUserID = ");
                Console.WriteLine(eventHorizonLINQ.TargetUserID);
            }

            Int32 qty;
            if (Int32.TryParse(QtyTextBox.Text, out qty))
            {
                eventHorizonLINQ.Qty = qty;
                Console.Write("eventHorizonLINQ.Qty = ");
                Console.WriteLine(eventHorizonLINQ.Qty);
            }

            double discount;
            if (double.TryParse(DiscountTextBox.Text, out discount))
            {
                eventHorizonLINQ.Discount = discount;
                Console.Write("eventHorizonLINQ.Discount = ");
                Console.WriteLine(eventHorizonLINQ.Discount);
            }

            double grandTotal = (eventHorizonLINQ.UnitCost * eventHorizonLINQ.Qty) - ((eventHorizonLINQ.UnitCost * eventHorizonLINQ.Qty) * eventHorizonLINQ.Discount / 100);

            return grandTotal;
        }

        private void GetOracleEvent()
        {
            EventTypeComboBox.SelectedIndex = eventHorizonLINQ.EventTypeID;
            SourceComboBox.SelectedIndex = eventHorizonLINQ.SourceID;
            DetailsTextBox.Text = eventHorizonLINQ.Details;
            FrequencyComboBox.SelectedIndex = eventHorizonLINQ.FrequencyID;
            TargetUserIDComboBox.SelectedIndex = eventHorizonLINQ.TargetUserID;
            StatusComboBox.SelectedIndex = eventHorizonLINQ.StatusID;

            EventAttributeComboBox.SelectedIndex = eventHorizonLINQ.EventAttributeID;

            TryLoadImage(eventHorizonLINQ.PathFileName);

            QtyTextBox.Text = eventHorizonLINQ.Qty.ToString();

            UnitCostTextBox.Text = $"{eventHorizonLINQ.UnitCost:0.00}";

            DiscountTextBox.Text = $"{eventHorizonLINQ.Discount:0.00}";

            GrandTotalTextBox.Text = $"{CalcGrandTotal():0.00}";

            StockTextBox.Text = eventHorizonLINQ.Stock.ToString();

            PathFileNameLabel.Content = eventHorizonLINQ.PathFileName;

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

            eventHorizonLINQ.EventAttributeID = EventAttributeComboBox.SelectedIndex;
            //eventHorizonLINQ.PathFileName = string.Empty;
            
            double unitCost;
            if (double.TryParse(UnitCostTextBox.Text, out unitCost))
            {
                eventHorizonLINQ.UnitCost = unitCost;
                Console.Write("eventHorizonLINQ.TargetUserID = ");
                Console.WriteLine(eventHorizonLINQ.TargetUserID);
            }

            Int32 qty;
            if (Int32.TryParse(QtyTextBox.Text, out qty))
            {
                eventHorizonLINQ.Qty = qty;
                Console.Write("eventHorizonLINQ.Qty = ");
                Console.WriteLine(eventHorizonLINQ.Qty);
            }

            double discount;
            if (double.TryParse(DiscountTextBox.Text, out discount))
            {
                eventHorizonLINQ.Discount = discount;
                Console.Write("eventHorizonLINQ.Discount = ");
                Console.WriteLine(eventHorizonLINQ.Discount);
            }

            Int32 stock;
            if (Int32.TryParse(StockTextBox.Text, out stock))
            {
                eventHorizonLINQ.Stock = stock;
                Console.Write("eventHorizonLINQ.Stock = ");
                Console.WriteLine(eventHorizonLINQ.Stock);
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

        private void BrowseButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;

            int buttonID = 0;

            bool success = Int32.TryParse(button.Tag.ToString(), out buttonID);

            if (button != null && success)
            {
                switch (buttonID)
                {
                    case 0:
                        string PathFileNameString = EventHorizonDatabaseCreate.OpenFile("All supported files|*.jpg;*.jpeg;*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|Portable Network Graphic (*.png)|*.png|PDF (*.pdf)|*.pdf|All files (*.*)|*.*");

                        TryLoadImage(PathFileNameString);

                        PathFileNameLabel.Content = PathFileNameString;
                        eventHorizonLINQ.PathFileName = PathFileNameString;
                        break;
                    case 1:
                        if (eventHorizonLINQ.PathFileName != string.Empty)
                        {
                            if (File.Exists(eventHorizonLINQ.PathFileName))
                            {
                                Topmost = false;
                                Process.Start(eventHorizonLINQ.PathFileName);
                            }
                        }
                        break;
                    case 2:
                        double unitCost;
                        if (double.TryParse(UnitCostTextBox.Text, out unitCost))
                        {
                            eventHorizonLINQ.UnitCost = unitCost;
                            Console.Write("eventHorizonLINQ.TargetUserID = ");
                            Console.WriteLine(eventHorizonLINQ.TargetUserID);
                        }

                        UnitCostTextBox.Text = $"{eventHorizonLINQ.UnitCost:0.00}";

                        double discount;
                        if (double.TryParse(DiscountTextBox.Text, out discount))
                        {
                            eventHorizonLINQ.Discount = discount;
                            Console.Write("eventHorizonLINQ.Discount = ");
                            Console.WriteLine(eventHorizonLINQ.Discount);
                        }

                        DiscountTextBox.Text = $"{eventHorizonLINQ.Discount:0.00}";

                        GrandTotalTextBox.Text = $"{CalcGrandTotal():0.00}";
                        break;
                }
            }
        }

        internal void TryLoadImage(string pathFileName)
        {
            string FileExtension = Path.GetExtension(pathFileName);

            FileExtension = FileExtension.Replace(".", string.Empty);

            FileExtension = FileExtension.ToLower();

            Console.Write("File Extension is ");
            Console.WriteLine(FileExtension);

            PathFileNameImage.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri("pack://application:,,/Images/EventHorizonLogoNewSmall.png"));
            
            if (FileExtension == FileExtensionsImage.png || FileExtension == FileExtensionsImage.jpg || FileExtension == FileExtensionsImage.bmp)
            {
                try
                {
                    if (File.Exists(pathFileName))
                    {
                        Uri src = new Uri(pathFileName, UriKind.RelativeOrAbsolute);
                        BitmapImage small_image_bmp = new BitmapImage();
                        small_image_bmp.BeginInit();
                        small_image_bmp.CacheOption = BitmapCacheOption.OnLoad;
                        small_image_bmp.UriSource = src;
                        small_image_bmp.EndInit();

                        PathFileNameImage.Source = small_image_bmp;
                    }
                }
                catch
                {
                    Console.WriteLine();
                    //UserPhoto.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri("pack://application:,,,/Images/face.jpg"));
                }
            }
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
            if (formOpenStartTime + eventIsReadAfterTimeSpan < DateTime.Now && eventWindowMode != EventWindowModes.NewEvent)
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
                        EventWindow nevn = new EventWindow(mainWindow, EventWindowModes.NewNote, eventHorizonLINQ, this);
                        nevn.Show();
                        nevn.Left += 30;
                        nevn.Top += 30;
                        break;
                    case EventFormCloseButtons.Reply:
                        SetOracleEvent();
                        EventWindow nev = new EventWindow(mainWindow, EventWindowModes.NewReply, eventHorizonLINQ, this);
                        nev.Show();
                        nev.Left += 30;
                        nev.Top += 30;
                        break;
                    case EventFormCloseButtons.Save:
                        SetOracleEvent();
                        switch (eventWindowMode)
                        {
                            case EventWindowModes.ViewMainEvent:
                                DataTableManagement.SaveEvent(this, eventHorizonLINQ, EventWindowModes.ViewMainEvent);
                                break;
                            case EventWindowModes.ViewNote:
                                DataTableManagement.SaveEvent(this, eventHorizonLINQ, EventWindowModes.ViewNote);
                                break;
                            case EventWindowModes.ViewReply:
                                DataTableManagement.SaveEvent(this, eventHorizonLINQ, EventWindowModes.ViewReply);
                                break;
                            case EventWindowModes.EditMainEvent:
                                DataTableManagement.SaveEvent(this, eventHorizonLINQ, EventWindowModes.EditMainEvent);
                                break;
                            case EventWindowModes.EditNote:
                                DataTableManagement.SaveEvent(this, eventHorizonLINQ, EventWindowModes.EditNote);
                                break;
                            case EventWindowModes.EditReply:
                                DataTableManagement.SaveEvent(this, eventHorizonLINQ, EventWindowModes.EditReply);
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
                //Console.Write("eventHorizonLINQ.Source_Mode = ");
                //Console.WriteLine(eventHorizonLINQ.Source_Mode);
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

        String EventAttributeName = string.Empty;

        private void EventAttributeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is StackPanel))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            var selectedTag = ((StackPanel)EventAttributeComboBox.SelectedItem).Tag.ToString();

            EventAttributeName = selectedTag;

            Console.Write("** EventAttributeComboBox_SelectedIndex = ");
            Console.WriteLine(EventAttributeComboBox.SelectedIndex);
            Console.Write("** item.Tag EventAttributeName = ");
            Console.WriteLine(EventAttributeName);
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