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

            AddItemsToRamsProfileTypeComboBox();

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
                EventTitleLabel.Content = "View Main Rams";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = false;
                DetailsTextBox.IsReadOnly = true;
                RevisionNoComboBox.IsEnabled = false;
                ReviewedDatePicker.IsEnabled = false;
                MSRevisionNoComboBox.IsEnabled = false;
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
                RamsProfileTypeComboBox.IsEnabled = false;
                DetailsTextBox.IsReadOnly = true;
                RevisionNoComboBox.IsEnabled = false;
                ReviewedDatePicker.IsEnabled = false;
                MSRevisionNoComboBox.IsEnabled = false;
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
                RamsProfileTypeComboBox.IsEnabled = false;
                DetailsTextBox.IsReadOnly = true;
                RevisionNoComboBox.IsEnabled = false;
                ReviewedDatePicker.IsEnabled = false;
                MSRevisionNoComboBox.IsEnabled = false;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (ramsWindowMode == EventWindowModes.EditMainEvent)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Main Rams";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = true;
                DetailsTextBox.IsReadOnly = false;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                MSRevisionNoComboBox.IsEnabled = true;
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
                RamsProfileTypeComboBox.IsEnabled = true;
                DetailsTextBox.IsReadOnly = false;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                MSRevisionNoComboBox.IsEnabled = true;
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
                RamsProfileTypeComboBox.IsEnabled = true;
                DetailsTextBox.IsReadOnly = false;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                MSRevisionNoComboBox.IsEnabled = true;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.NewEvent)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.Source_ParentEventID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Rams";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[XMLReaderWriter.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                RamsProfileTypeComboBox.SelectedIndex = eventHorizonRamsLINQ.EventTypeID;
                RamsProfileTypeComboBox.IsEnabled = true;
                DetailsTextBox.IsReadOnly = false;
                RevisionNoComboBox.SelectedIndex = 0;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                ReviewedDatePicker.SelectedDate = DateTime.Now;
                MSRevisionNoComboBox.SelectedIndex = 0;
                MSRevisionNoComboBox.IsEnabled = true;
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
                RamsProfileTypeComboBox.SelectedIndex = eventHorizonRamsLINQ.EventTypeID;
                RamsProfileTypeComboBox.IsEnabled = false;
                DetailsTextBox.IsReadOnly = false;
                RevisionNoComboBox.SelectedIndex = 0;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                ReviewedDatePicker.SelectedDate = DateTime.Now;
                MSRevisionNoComboBox.SelectedIndex = 0;
                MSRevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
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
                RamsProfileTypeComboBox.SelectedIndex = eventHorizonRamsLINQ.EventTypeID;
                RamsProfileTypeComboBox.IsEnabled = false;
                DetailsTextBox.IsReadOnly = false;
                RevisionNoComboBox.SelectedIndex = 0;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                ReviewedDatePicker.SelectedDate = DateTime.Now;
                MSRevisionNoComboBox.SelectedIndex = 0;
                MSRevisionNoComboBox.IsEnabled = true;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }

            this.Owner = Application.Current.MainWindow;
        }
        
        private void AddItemsToRamsProfileTypeComboBox()
        {
            foreach (RamsProfileType ramsProfileType in DataTableManagementRams.RamsProfileTypesList)
            {
                RamsProfileTypeComboBox.Items.Add(EventHorizonRamsProfileTypes.GetRamsProfileTypeStackPanel(ramsProfileType));
            }

            if (ramsWindowMode != EventWindowModes.EditMainEvent || ramsWindowMode != EventWindowModes.EditNote || ramsWindowMode != EventWindowModes.EditReply) RamsProfileTypeComboBox.SelectedIndex = 0;
        }
  
        private void GetOracleEvent()
        {
            RamsProfileTypeComboBox.SelectedIndex = eventHorizonRamsLINQ.EventTypeID;
            DetailsTextBox.Text = eventHorizonRamsLINQ.Details;
            RevisionNoComboBox.SelectedIndex = eventHorizonRamsLINQ.StatusID;

            DateTime reviewedDateTimet = eventHorizonRamsLINQ.TargetDate;
            ReviewedDatePicker.SelectedDate = reviewedDateTimet;

            UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
        }
        
        private void SetOracleEvent()
        {
            string detailsSafeString = DetailsTextBox.Text.Replace("'", "''");

            DateTime targetTimeDateTimeNow = DateTime.Now;

            DateTime reviewedDateTime = DateTime.Now;
            reviewedDateTime = new DateTime(ReviewedDatePicker.SelectedDate.Value.Year, ReviewedDatePicker.SelectedDate.Value.Month, ReviewedDatePicker.SelectedDate.Value.Day, reviewedDateTime.Hour, reviewedDateTime.Minute, reviewedDateTime.Second);

            eventHorizonRamsLINQ.EventTypeID = RamsProfileTypeComboBox.SelectedIndex;
            eventHorizonRamsLINQ.Details = detailsSafeString;
            eventHorizonRamsLINQ.StatusID = RevisionNoComboBox.SelectedIndex;
            eventHorizonRamsLINQ.TargetDate = reviewedDateTime;
        }
        
        public int RevisionNoID = 1;
        
        private void RevisionNoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RevisionNoID = RevisionNoComboBox.SelectedIndex;
        }

        public int MSRevisionNoID = 1;

        private void MSRevisionNoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MSRevisionNoID = RevisionNoComboBox.SelectedIndex;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9:]+");
            e.Handled = regex.IsMatch(e.Text);
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

                        eventHorizonRamsLINQ.PathFileName = PathFolderNameString;
                        break;
                    case 1:
                       
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
                        
                        break;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
         
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

        String RamsProfileTypeName = string.Empty;

        private void RamsProfileTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is StackPanel))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            var selectedTag = ((StackPanel)RamsProfileTypeComboBox.SelectedItem).Tag.ToString();

            RamsProfileTypeName = selectedTag;
           
            Console.Write("** RamsProfileTypeComboBox_SelectedIndex = ");
            Console.WriteLine(RamsProfileTypeComboBox.SelectedIndex);
            Console.Write("** item.Tag EventTypeName = ");
            Console.WriteLine(RamsProfileTypeName);
        }
    }
}