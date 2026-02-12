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
using Ookii.Dialogs.Wpf;

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for RamsWindow.xaml
    /// </summary>
    public partial class RamsWindow : Window
    {
        MainWindow mainWindow;
        int ramsWindowMode;
        int userID;
        EventHorizonRamsLINQ eventHorizonRamsLINQ;
        public RamsWindow ramsWindow;

        DateTime formOpenStartTime;
        TimeSpan eventIsReadAfterTimeSpan = TimeSpan.FromSeconds(3);

        public Int32 ParentEventID;

        public RamsWindow(MainWindow mainWindow, int ramsWindowMode, EventHorizonRamsLINQ eventHorizonRamsLINQ, RamsWindow ramsWindow = null)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            this.ramsWindowMode = ramsWindowMode;
            this.eventHorizonRamsLINQ = (EventHorizonRamsLINQ)eventHorizonRamsLINQ.Clone();
            this.ramsWindow = ramsWindow;
            this.userID = eventHorizonRamsLINQ.UserID;

            AddItemsToEventTypeComboBox();
            AddItemsToEventAttributeComboBox();
            AddItemsToSourceComboBox();
            AddItemsToFrequencyComboBox();

            formOpenStartTime = DateTime.Now;

            switch (ramsWindowMode)
            {
                case EventWindowModes.ViewMainEvent:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditMainEvent;
                    else
                        ramsWindowMode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.ViewNote:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditNote;
                    else
                        ramsWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.ViewReply:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditReply;
                    else
                        ramsWindowMode = EventWindowModes.ViewReply;

                    break;
                case EventWindowModes.EditMainEvent:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditMainEvent;
                    else
                        ramsWindowMode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.EditNote:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditNote;
                    else
                        ramsWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.EditReply:
                    GetOracleEvent();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditReply;
                    else
                        ramsWindowMode = EventWindowModes.ViewReply;

                    break;
                case EventWindowModes.NewEvent:
                case EventWindowModes.NewNote:
                case EventWindowModes.NewReply:
                    eventHorizonRamsLINQ.TargetUserID = eventHorizonRamsLINQ.UserID;
                    eventHorizonRamsLINQ.UserID = XMLReaderWriter.UserID;
                    break;
            }

            if (ramsWindowMode == EventWindowModes.ViewMainEvent)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Main Event";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
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
            else if (ramsWindowMode == EventWindowModes.ViewNote)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
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
            else if (ramsWindowMode == EventWindowModes.ViewReply)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
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
            else if (ramsWindowMode == EventWindowModes.EditMainEvent)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Main Event";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
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
            else if (ramsWindowMode == EventWindowModes.EditNote)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.Source_ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
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
            else if (ramsWindowMode == EventWindowModes.EditReply)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.Source_ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
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
            else if (ramsWindowMode == EventWindowModes.NewEvent)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Event";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[XMLReaderWriter.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                EventTypeComboBox.SelectedIndex = eventHorizonRamsLINQ.EventTypeID;
                EventTypeComboBox.IsEnabled = true;
                SourceComboBox.SelectedIndex = eventHorizonRamsLINQ.SourceID;
                SourceComboBox.IsEnabled = true;
                GenButton.Visibility = Visibility.Visible;
                DetailsTextBox.IsReadOnly = false;
                FrequencyComboBox.SelectedIndex = eventHorizonRamsLINQ.FrequencyID;
                FrequencyComboBox.IsEnabled = true;
                TargetUserIDComboBox.SelectedIndex = eventHorizonRamsLINQ.TargetUserID;
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
            else if (ramsWindowMode == EventWindowModes.NewNote)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[XMLReaderWriter.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                EventTypeComboBox.SelectedIndex = eventHorizonRamsLINQ.EventTypeID;
                EventTypeComboBox.IsEnabled = false;
                SourceComboBox.SelectedIndex = eventHorizonRamsLINQ.SourceID;
                SourceComboBox.IsEnabled = false;
                GenButton.Visibility = Visibility.Visible;
                DetailsTextBox.IsReadOnly = false;
                FrequencyComboBox.SelectedIndex = eventHorizonRamsLINQ.FrequencyID;
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
            else if (ramsWindowMode == EventWindowModes.NewReply)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                EventTypeComboBox.SelectedIndex = eventHorizonRamsLINQ.EventTypeID;
                EventTypeComboBox.IsEnabled = false;
                SourceComboBox.SelectedIndex = eventHorizonRamsLINQ.SourceID;
                SourceComboBox.IsEnabled = false;
                GenButton.Visibility = Visibility.Visible;
                DetailsTextBox.IsReadOnly = false;
                FrequencyComboBox.SelectedIndex = eventHorizonRamsLINQ.FrequencyID;
                FrequencyComboBox.IsEnabled = false;
                TargetUserIDComboBox.SelectedIndex = eventHorizonRamsLINQ.TargetUserID;
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

            if (ramsWindowMode != EventWindowModes.EditMainEvent || ramsWindowMode != EventWindowModes.EditNote || ramsWindowMode != EventWindowModes.EditReply) EventTypeComboBox.SelectedIndex = 0;
        }

        private void AddItemsToEventAttributeComboBox()
        {
            foreach (AttributeType attributeType in XMLReaderWriter.AttributeTypesList)
            {
                EventAttributeComboBox.Items.Add(EventHorizonAttributes.GetAttributeStackPanel(attributeType));
            }

            if (ramsWindowMode != EventWindowModes.EditMainEvent || ramsWindowMode != EventWindowModes.EditNote || ramsWindowMode != EventWindowModes.EditReply) EventAttributeComboBox.SelectedIndex = 0;
        }

        private void AddItemsToSourceComboBox()
        {
            foreach (SourceType sourceType in XMLReaderWriter.SourceTypesList)
            {
                SourceComboBox.Items.Add(EventHorizonSources.GetSourceStackPanel(sourceType));
            }
            
            if (ramsWindowMode != EventWindowModes.EditMainEvent || ramsWindowMode != EventWindowModes.EditNote || ramsWindowMode != EventWindowModes.EditReply) SourceComboBox.SelectedIndex = 0;
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

            if (ramsWindowMode != EventWindowModes.EditMainEvent || ramsWindowMode != EventWindowModes.EditNote || ramsWindowMode != EventWindowModes.EditReply) FrequencyComboBox.SelectedIndex = 0;
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
                eventHorizonRamsLINQ.UnitCost = unitCost;
                Console.Write("eventHorizonRamsLINQ.TargetUserID = ");
                Console.WriteLine(eventHorizonRamsLINQ.TargetUserID);
            }

            Int32 qty;
            if (Int32.TryParse(QtyTextBox.Text, out qty))
            {
                eventHorizonRamsLINQ.Qty = qty;
                Console.Write("eventHorizonRamsLINQ.Qty = ");
                Console.WriteLine(eventHorizonRamsLINQ.Qty);
            }

            double discount;
            if (double.TryParse(DiscountTextBox.Text, out discount))
            {
                eventHorizonRamsLINQ.Discount = discount;
                Console.Write("eventHorizonRamsLINQ.Discount = ");
                Console.WriteLine(eventHorizonRamsLINQ.Discount);
            }

            double grandTotal = (eventHorizonRamsLINQ.UnitCost * eventHorizonRamsLINQ.Qty) - ((eventHorizonRamsLINQ.UnitCost * eventHorizonRamsLINQ.Qty) * eventHorizonRamsLINQ.Discount / 100);

            return grandTotal;
        }

        private void GetOracleEvent()
        {
            EventTypeComboBox.SelectedIndex = eventHorizonRamsLINQ.EventTypeID;
            SourceComboBox.SelectedIndex = eventHorizonRamsLINQ.SourceID;
            DetailsTextBox.Text = eventHorizonRamsLINQ.Details;
            FrequencyComboBox.SelectedIndex = eventHorizonRamsLINQ.FrequencyID;
            TargetUserIDComboBox.SelectedIndex = eventHorizonRamsLINQ.TargetUserID;
            StatusComboBox.SelectedIndex = eventHorizonRamsLINQ.StatusID;

            EventAttributeComboBox.SelectedIndex = eventHorizonRamsLINQ.EventAttributeID;

            TryLoadImage(eventHorizonRamsLINQ.PathFileName);

            QtyTextBox.Text = eventHorizonRamsLINQ.Qty.ToString();

            UnitCostTextBox.Text = $"{eventHorizonRamsLINQ.UnitCost:0.00}";

            DiscountTextBox.Text = $"{eventHorizonRamsLINQ.Discount:0.00}";

            GrandTotalTextBox.Text = $"{CalcGrandTotal():0.00}";

            StockTextBox.Text = eventHorizonRamsLINQ.Stock.ToString();

            PathFileNameLabel.Content = eventHorizonRamsLINQ.PathFileName;

            DateTime targetDateTimet = eventHorizonRamsLINQ.TargetDate;
            TargetDatePicker.SelectedDate = targetDateTimet;
            TargetTimeHoursPicker.Text = targetDateTimet.ToString("HH");
            TargetTimeMinutesPicker.Text = targetDateTimet.ToString("mm");

            UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
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

            eventHorizonRamsLINQ.EventTypeID = EventTypeComboBox.SelectedIndex;
            eventHorizonRamsLINQ.SourceID = SourceComboBox.SelectedIndex;
            eventHorizonRamsLINQ.Details = detailsSafeString;
            eventHorizonRamsLINQ.FrequencyID = FrequencyComboBox.SelectedIndex;
            eventHorizonRamsLINQ.StatusID = StatusComboBox.SelectedIndex;
            eventHorizonRamsLINQ.TargetDate = targetDateTime;

            eventHorizonRamsLINQ.EventAttributeID = EventAttributeComboBox.SelectedIndex;
            //eventHorizonLINQ.PathFileName = string.Empty;
            
            double unitCost;
            if (double.TryParse(UnitCostTextBox.Text, out unitCost))
            {
                eventHorizonRamsLINQ.UnitCost = unitCost;
                Console.Write("eventHorizonRamsLINQ.TargetUserID = ");
                Console.WriteLine(eventHorizonRamsLINQ.TargetUserID);
            }

            Int32 qty;
            if (Int32.TryParse(QtyTextBox.Text, out qty))
            {
                eventHorizonRamsLINQ.Qty = qty;
                Console.Write("eventHorizonRamsLINQ.Qty = ");
                Console.WriteLine(eventHorizonRamsLINQ.Qty);
            }

            double discount;
            if (double.TryParse(DiscountTextBox.Text, out discount))
            {
                eventHorizonRamsLINQ.Discount = discount;
                Console.Write("eventHorizonRamsLINQ.Discount = ");
                Console.WriteLine(eventHorizonRamsLINQ.Discount);
            }

            Int32 stock;
            if (Int32.TryParse(StockTextBox.Text, out stock))
            {
                eventHorizonRamsLINQ.Stock = stock;
                Console.Write("eventHorizonRamsLINQ.Stock = ");
                Console.WriteLine(eventHorizonRamsLINQ.Stock);
            }

            Console.Write("eventHorizonRamsLINQ.TargetUserID = ");
            Console.WriteLine(eventHorizonRamsLINQ.TargetUserID);
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
                        string PathFolderNameString = string.Empty;

                        var dialog = new VistaFolderBrowserDialog
                        {
                            Description = PathFolderNameString,
                            UseDescriptionForTitle = true,
                            ShowNewFolderButton = true
                        };

                        bool? result = dialog.ShowDialog();

                        if (result == true)
                        {
                            PathFolderNameString = dialog.SelectedPath;
                        }

                        PathFileNameLabel.Content = PathFolderNameString;
                        eventHorizonRamsLINQ.PathFileName = PathFolderNameString;
                        break;
                    case 1:
                        string PathFileNameString = EventHorizonDatabaseCreate.OpenFile("All supported files|*.jpg;*.jpeg;*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|Portable Network Graphic (*.png)|*.png|PDF (*.pdf)|*.pdf|All files (*.*)|*.*");

                        TryLoadImage(PathFileNameString);

                        PathFileNameLabel.Content = PathFileNameString;
                        eventHorizonRamsLINQ.PathFileName = PathFileNameString;
                        break;
                    case 2:
                        if (eventHorizonRamsLINQ.PathFileName != string.Empty)
                        {
                            if (File.Exists(eventHorizonRamsLINQ.PathFileName))
                            {
                                Topmost = false;
                                Process.Start(eventHorizonRamsLINQ.PathFileName);
                            }
                            else if (Directory.Exists(eventHorizonRamsLINQ.PathFileName))
                            {
                                Topmost = false;
                                Process.Start(new ProcessStartInfo("explorer.exe", eventHorizonRamsLINQ.PathFileName) { UseShellExecute = true }); // open folder
                            }
                        }
                        break;
                    case 3:
                        double unitCost;
                        if (double.TryParse(UnitCostTextBox.Text, out unitCost))
                        {
                            eventHorizonRamsLINQ.UnitCost = unitCost;
                            Console.Write("eventHorizonRamsLINQ.TargetUserID = ");
                            Console.WriteLine(eventHorizonRamsLINQ.TargetUserID);
                        }

                        UnitCostTextBox.Text = $"{eventHorizonRamsLINQ.UnitCost:0.00}";

                        double discount;
                        if (double.TryParse(DiscountTextBox.Text, out discount))
                        {
                            eventHorizonRamsLINQ.Discount = discount;
                            Console.Write("eventHorizonRamsLINQ.Discount = ");
                            Console.WriteLine(eventHorizonRamsLINQ.Discount);
                        }

                        DiscountTextBox.Text = $"{eventHorizonRamsLINQ.Discount:0.00}";

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
            if (formOpenStartTime + eventIsReadAfterTimeSpan < DateTime.Now && ramsWindowMode != EventWindowModes.NewEvent)
            {
                if (eventHorizonRamsLINQ.TargetUserID == XMLReaderWriter.UserID)
                {
                    DataTableManagement.UpdateReadByMeID(eventHorizonRamsLINQ.ID, ReadByMeModes.Yes);

                    if (StatusID == Statuses.ActiveNotified) DataTableManagement.UpdateStatusID(eventHorizonRamsLINQ.ID, Statuses.ActiveNotifiedRead);
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
                        RamsWindow nevn = new RamsWindow(mainWindow, EventWindowModes.NewNote, eventHorizonRamsLINQ, this);
                        nevn.Show();
                        nevn.Left += 30;
                        nevn.Top += 30;
                        break;
                    case EventFormCloseButtons.Reply:
                        SetOracleEvent();
                        RamsWindow nev = new RamsWindow(mainWindow, EventWindowModes.NewReply, eventHorizonRamsLINQ, this);
                        nev.Show();
                        nev.Left += 30;
                        nev.Top += 30;
                        break;
                    case EventFormCloseButtons.Save:
                        SetOracleEvent();
                        switch (ramsWindowMode)
                        {
                            case EventWindowModes.ViewMainEvent:
                                DataTableManagementRams.SaveEvent(this, eventHorizonRamsLINQ, EventWindowModes.ViewMainEvent);
                                break;
                            case EventWindowModes.ViewNote:
                                DataTableManagementRams.SaveEvent(this, eventHorizonRamsLINQ, EventWindowModes.ViewNote);
                                break;
                            case EventWindowModes.ViewReply:
                                DataTableManagementRams.SaveEvent(this, eventHorizonRamsLINQ, EventWindowModes.ViewReply);
                                break;
                            case EventWindowModes.EditMainEvent:
                                DataTableManagementRams.SaveEvent(this, eventHorizonRamsLINQ, EventWindowModes.EditMainEvent);
                                break;
                            case EventWindowModes.EditNote:
                                DataTableManagementRams.SaveEvent(this, eventHorizonRamsLINQ, EventWindowModes.EditNote);
                                break;
                            case EventWindowModes.EditReply:
                                DataTableManagementRams.SaveEvent(this, eventHorizonRamsLINQ, EventWindowModes.EditReply);
                                break;
                            case EventWindowModes.NewEvent:
                                DataTableManagementRams.SaveEvent(this, eventHorizonRamsLINQ, EventWindowModes.NewEvent);
                                break;
                            case EventWindowModes.NewNote:
                                DataTableManagementRams.SaveEvent(this, eventHorizonRamsLINQ, EventWindowModes.NewNote);
                                break;
                            case EventWindowModes.NewReply:
                                DataTableManagementRams.SaveEvent(this, eventHorizonRamsLINQ, EventWindowModes.NewReply);
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