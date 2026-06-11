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
    /// Interaction logic for MethodStatementWindow.xaml
    /// </summary>
    public partial class MethodStatementWindow : Window
    {
        MainWindow mainWindow;
        int ramsWindowMode;
        int userID;
        EventHorizonJobLINQ eventHorizonJobLINQ;
        EventHorizonMethodStatementLINQ eventHorizonMethodStatementLINQ;

        public MethodStatementWindow methodStatementWindow;

        DateTime formOpenStartTime;
        TimeSpan eventIsReadAfterTimeSpan = TimeSpan.FromSeconds(3);

        public MethodStatementWindow(MainWindow mainWindow, int ramsWindowMode, EventHorizonJobLINQ eventHorizonJobLINQ, EventHorizonMethodStatementLINQ eventHorizonMethodStatementLINQ, MethodStatementWindow methodStatementWindow = null)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            this.ramsWindowMode = ramsWindowMode;
            this.eventHorizonJobLINQ = (EventHorizonJobLINQ)eventHorizonJobLINQ.Clone();
            this.methodStatementWindow = methodStatementWindow;
            this.eventHorizonMethodStatementLINQ = (EventHorizonMethodStatementLINQ)eventHorizonMethodStatementLINQ.Clone();
            this.userID = eventHorizonJobLINQ.UserID;

            formOpenStartTime = DateTime.Now;

            switch (ramsWindowMode)
            {
                case EventWindowModes.ViewMainEvent:
                    GetMethodStatement();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditMainEvent;
                    else
                        ramsWindowMode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.ViewNote:
                    GetMethodStatement();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditNote;
                    else
                        ramsWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.ViewReply:
                    GetMethodStatement();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditReply;
                    else
                        ramsWindowMode = EventWindowModes.ViewReply;

                    break;
                case EventWindowModes.EditMainEvent:
                    GetMethodStatement();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditMainEvent;
                    else
                        ramsWindowMode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.EditNote:
                    GetMethodStatement();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditNote;
                    else
                        ramsWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.EditReply:
                    GetMethodStatement();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditReply;
                    else
                        ramsWindowMode = EventWindowModes.ViewReply;

                    break;
                case EventWindowModes.NewEvent:
                case EventWindowModes.NewNote:
                case EventWindowModes.NewReply:
                    eventHorizonJobLINQ.UserID = XMLReaderWriter.UserID;
                    break;
            }

            if (ramsWindowMode == EventWindowModes.ViewMainEvent)
            {
                EventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Main Method Statement";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                RamsProfileTypeTextBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = true;
                RevisionNoComboBox.IsEnabled = false;
                ReviewedDatePicker.IsEnabled = false;
                AddNoteButton.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (ramsWindowMode == EventWindowModes.ViewNote)
            {
                EventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                RamsProfileTypeTextBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = true;
                RevisionNoComboBox.IsEnabled = false;
                ReviewedDatePicker.IsEnabled = false;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (ramsWindowMode == EventWindowModes.ViewReply)
            {
                EventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                RamsProfileTypeTextBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = true;
                RevisionNoComboBox.IsEnabled = false;
                ReviewedDatePicker.IsEnabled = false;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (ramsWindowMode == EventWindowModes.EditMainEvent)
            {
                EventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Main Method Statement";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                RamsProfileTypeTextBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = false;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                AddNoteButton.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.EditNote)
            {
                EventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                RamsProfileTypeTextBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = false;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.EditReply)
            {
                EventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                RamsProfileTypeTextBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = false;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.NewEvent)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Method Statement";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[XMLReaderWriter.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                RamsProfileTypeTextBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = false;
                RevisionNoComboBox.SelectedIndex = 0;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                ReviewedDatePicker.SelectedDate = DateTime.Now;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.NewNote)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[XMLReaderWriter.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                //RamsProfileTypeTextBox.Text = eventHorizonJobLINQ.; get RamsProfileName here
                RamsProfileTypeTextBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = false;
                RevisionNoComboBox.SelectedIndex = 0;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                ReviewedDatePicker.SelectedDate = DateTime.Now;
                ReviewedDatePicker.IsEnabled = true;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.NewReply)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                //RamsProfileTypeTextBox.Text = eventHorizonJobLINQ.; get RamsProfileName here
                RamsProfileTypeTextBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = false;
                RevisionNoComboBox.SelectedIndex = 0;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                ReviewedDatePicker.SelectedDate = DateTime.Now;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }

            this.Owner = Application.Current.MainWindow;
        }
  
        private void GetMethodStatement()
        {
            UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);

            JobNoTextBox.Text = eventHorizonJobLINQ.JobNo;
            DescriptionTextBox.Text = eventHorizonJobLINQ.Description;
            ClientNameTextBox.Text = eventHorizonJobLINQ.ClientName;
            SiteTextBox.Text = eventHorizonJobLINQ.Site;
            LocationActivityTextBox.Text = eventHorizonJobLINQ.LocationActivity;
            MSContractTitleTextBox.Text = eventHorizonJobLINQ.Description;

            DateTime reviewedDateTimet = eventHorizonMethodStatementLINQ.ReviewedDateTime;
            ReviewedDatePicker.SelectedDate = reviewedDateTimet;

            RevisionNoComboBox.SelectedIndex = eventHorizonMethodStatementLINQ.MSRevisionNo;
            ElementReviewedTextBox.Text = eventHorizonMethodStatementLINQ.ElementReviewed;
            MSContractorTextBox.Text = eventHorizonMethodStatementLINQ.MSContractor;
            RamsStatusIDComboBox.SelectedIndex = eventHorizonMethodStatementLINQ.StatusID;
        }
        
        private void SetMethodStatement()
        {
            string MSContractorSafeString = MSContractorTextBox.Text.Replace("'", "''");
            string ElementReviewedSafeString = ElementReviewedTextBox.Text.Replace("'", "''");

            eventHorizonMethodStatementLINQ.ElementReviewed = ElementReviewedSafeString;

            DateTime reviewedDateTime = DateTime.Now;
            reviewedDateTime = new DateTime(ReviewedDatePicker.SelectedDate.Value.Year, ReviewedDatePicker.SelectedDate.Value.Month, ReviewedDatePicker.SelectedDate.Value.Day, reviewedDateTime.Hour, reviewedDateTime.Minute, reviewedDateTime.Second);

            eventHorizonMethodStatementLINQ.ReviewedDateTime = reviewedDateTime;

            eventHorizonMethodStatementLINQ.MSRevisionNo = RevisionNoID;

            eventHorizonMethodStatementLINQ.MSContractor = MSContractorSafeString;
        }

        public int RevisionNoID = 1;

        private void RevisionNoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RevisionNoID = RevisionNoComboBox.SelectedIndex;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9:]+");
            e.Handled = regex.IsMatch(e.Text);
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
                        SetMethodStatement();
                        MethodStatementWindow nevn = new MethodStatementWindow(mainWindow, EventWindowModes.NewNote, eventHorizonJobLINQ, eventHorizonMethodStatementLINQ, this);
                        nevn.Show();
                        nevn.Left += 30;
                        nevn.Top += 30;
                        break;
                    case EventFormCloseButtons.Reply:
                        SetMethodStatement();
                        MethodStatementWindow nev = new MethodStatementWindow(mainWindow, EventWindowModes.NewReply, eventHorizonJobLINQ, eventHorizonMethodStatementLINQ, this);
                        nev.Show();
                        nev.Left += 30;
                        nev.Top += 30;
                        break;
                    case EventFormCloseButtons.Save:
                        SetMethodStatement();
                        switch (ramsWindowMode)
                        {
                            case EventWindowModes.ViewMainEvent:
                                DataTableManagementMethodStatement.SaveMethodStatement(this, eventHorizonJobLINQ, eventHorizonMethodStatementLINQ, EventWindowModes.ViewMainEvent);
                                break;
                            case EventWindowModes.ViewNote:
                                DataTableManagementMethodStatement.SaveMethodStatement(this, eventHorizonJobLINQ, eventHorizonMethodStatementLINQ, EventWindowModes.ViewNote);
                                break;
                            case EventWindowModes.ViewReply:
                                DataTableManagementMethodStatement.SaveMethodStatement(this, eventHorizonJobLINQ, eventHorizonMethodStatementLINQ, EventWindowModes.ViewReply);
                                break;
                            case EventWindowModes.EditMainEvent:
                                DataTableManagementMethodStatement.SaveMethodStatement(this, eventHorizonJobLINQ, eventHorizonMethodStatementLINQ, EventWindowModes.EditMainEvent);
                                break;
                            case EventWindowModes.EditNote:
                                DataTableManagementMethodStatement.SaveMethodStatement(this, eventHorizonJobLINQ, eventHorizonMethodStatementLINQ, EventWindowModes.EditNote);
                                break;
                            case EventWindowModes.EditReply:
                                DataTableManagementMethodStatement.SaveMethodStatement(this, eventHorizonJobLINQ, eventHorizonMethodStatementLINQ, EventWindowModes.EditReply);
                                break;
                            case EventWindowModes.NewEvent:
                                DataTableManagementMethodStatement.SaveMethodStatement(this, eventHorizonJobLINQ, eventHorizonMethodStatementLINQ, EventWindowModes.NewEvent);
                                break;
                            case EventWindowModes.NewNote:
                                DataTableManagementMethodStatement.SaveMethodStatement(this, eventHorizonJobLINQ, eventHorizonMethodStatementLINQ, EventWindowModes.NewNote);
                                break;
                            case EventWindowModes.NewReply:
                                DataTableManagementMethodStatement.SaveMethodStatement(this, eventHorizonJobLINQ, eventHorizonMethodStatementLINQ, EventWindowModes.NewReply);
                                break;
                        }
                        break;
                }
            }
        }

        public int RamsStatusID = RamsStatus.New;
        private void RamsStatusIDComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RamsStatusID = RamsStatusIDComboBox.SelectedIndex;
        }
    }
}